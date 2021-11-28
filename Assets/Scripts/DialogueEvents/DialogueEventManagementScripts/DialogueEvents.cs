using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RichTextSubstringHelper;

namespace DeepFry
{
    [System.Serializable]
    public class DialogueEvents : MonoBehaviour
    {
        public List<BaseDialogueEvent> eventOrDialogue = new List<BaseDialogueEvent>(); //all possible events/dialogues to add to the GameObject

        GameObject playerGO; //player's game object

        [System.NonSerialized] public float textSpeed = 0.030225f; //how fast the text fills up in dialogue

        Text messageText; //text component assigned to dialogue panel text

        bool dialogueStarted = false; //to check if the dialogue has started

        bool buttonPressed = false; //makes sure pressing action button is only processed once

        object[] attachedScripts; //all attached scripts to the game object for events
        BaseScriptedEvent theScript; //script to access
        List<BaseScriptedEvent> dialogueEvents = new List<BaseScriptedEvent>(); //all event scripts to the game object
                                                                                //[System.NonSerialized] public int currentEvent; //to track which dialogue/event in the list is accessible (might be able to remove)
        List<BaseDialogueEvent> eventsToRun = new List<BaseDialogueEvent>(); //stores events to be run for the current interacted event
        public List<BaseScriptedEvent> activeScripts = new List<BaseScriptedEvent>(); //scripts currently running
                                                                                      //string facingBefore = ""; //direction that object is facing before turning toward player, so they can be returned to original position after dialogue

        bool pauseParallel; //holds event from running if it is parallel trigger

        [System.NonSerialized] public bool startAutomatically; //for AUTOSTART
        bool runOnce = true; //to ensure AUTOSTART runs once

        Collider2D thisCollider; //collider for the game object
        Collider2D playerCollider; //collider for the player

        Animator messagePanelAnimator;
        AudioSource audioSource;
        AudioClip openWindowSE;
        AudioClip confirmSE;

        AudioSource voiceAudioSource;

        // Start is called before the first frame update
        void Start()
        {
            playerGO = GameObject.Find("Player"); //assigns player game object to playerGO
                                                  //messageText = GameManager.instance.DialogueCanvas.GetComponentInChildren<Text>(); //assigns messageText to dialogue canvas text component

            openWindowSE = Resources.Load<AudioClip>("Sounds/OpenMenu");
            confirmSE = Resources.Load<AudioClip>("Sounds/000 - Cursor Move");

            attachedScripts = FindObjectsOfType(typeof(BaseScriptedEvent)); //gets all scripts attached to object
                                                                            //System.Array.Reverse(attachedScripts); //reverses array, as the scripts are added from bottom to top.  this makes the list more readable

            for (int i = 0; i < attachedScripts.Length; i++) //adds event scripts to list from all scripts on object (this will be updated)
            {
                theScript = attachedScripts[i] as BaseScriptedEvent; //make sure script is of type 'BaseScriptedEvent'
                if (this.gameObject.name == theScript.name)
                {
                    dialogueEvents.Add(theScript); //adds all attached scripts to 'dialogueEvents' list
                }
            }
            thisCollider = this.gameObject.GetComponent<Collider2D>(); //assigns thisCollider to collider on game object
                                                                       //playerCollider = playerGO.GetComponent<Collider2D>(); //assigns playerCollider to player's collider
                                                                       //currentEvent = 0; (might be able to remove)
            pauseParallel = false; //starts the DialogueEvent script out with pauseParallel=false
            SetCurrentEvents(); //sets events to be currently running (based on bools)
                                //GameManager.instance.DisplayPanel(false); //hides message panel
        }

        // Update is called once per frame
        void Update()
        {
            StartCoroutine(RunEvents());
        }

        IEnumerator RunEvents() //run all active events based on trigger
        {
            //ONACTION requires confirm button to be pressed next to and facing the object
            //ONTOUCH requires player's collider to touch object's collider
            //AUTOSTART starts the event automatically and runs it once
            //PARALLEL continuously runs the event

            for (int i = 0; i < eventsToRun.Count; i++)
            {
                BaseDialogueEvent eventToRun = eventsToRun[i]; //pass which event to run to ProcessEvents()
                                                               //if (!menu.inMenu)
                                                               //{
                                                               //if confirm button pressed and trigger is ONACTION
                if (Input.GetButtonDown("Confirm") && dialogueStarted == false && buttonPressed == false && eventToRun.triggerAction == BaseDialogueEvent.TriggerActions.ONACTION)
                {
                    //checks that player is touching collider with this object
                    if (thisCollider.IsTouching(playerCollider))
                    {
                        //Debug.Log("ONACTION - event: " + i);
                        //CheckConfirmButtonStatus(); //keeps checking on button status to make sure event only triggers once
                        pauseParallel = true;
                        yield return ProcessEvent(eventToRun);
                        pauseParallel = false;
                    }
                }

                //if player and this collider are touching, and trigger is ONTOUCH
                if (eventToRun.triggerAction == BaseDialogueEvent.TriggerActions.ONTOUCH && thisCollider.IsTouching(playerCollider))
                {
                    runOnce = false; //keeps from running multiple times if touching
                    pauseParallel = true;
                    StartCoroutine(ProcessEvent(eventToRun)); //displays dialogue
                    pauseParallel = false;
                }

                //if trigger is AUTOSTART (this could maybe be updated)
                if (eventToRun.triggerAction == BaseDialogueEvent.TriggerActions.AUTOSTART)
                {
                    startAutomatically = true;
                }
                else
                {
                    startAutomatically = false;
                }

                if (startAutomatically && runOnce)
                {
                    runOnce = false;
                    startAutomatically = false;
                    pauseParallel = true;
                    StartCoroutine(ProcessEvent(eventToRun));
                    pauseParallel = false;
                }

                //if trigger is PARALLEL
                if (eventToRun.triggerAction == BaseDialogueEvent.TriggerActions.PARALLEL && pauseParallel == false)
                {
                    //Debug.Log("PARALLEL - event: " + i);
                    StartCoroutine(ProcessEvent(eventToRun));
                }
                //}
            }

            //checks if confirm button is pressed
            CheckConfirmButtonStatus();
            yield return null;
        }

        IEnumerator ProcessEvent(BaseDialogueEvent eventToRun) //processes 'RunBeforeEvents', the dialogue (if any), 'RunAfterEvents', sets the booleans from the event, and sets the new current events based on bools changed
        {
            RunBeforeEvents(eventToRun); //runs events that should be run before dialogue

            if (eventToRun.dialogText.Length > 0 && !buttonPressed) //runs the dialogue event
            {
                CheckPlayerMovement(eventToRun); //if player movement should be disabled, it is disabled
                yield return RunDialogue(eventToRun); //runs actual dialogue
                EnableMovementAfterDialogue(); //enables movement after dialogue is processed
            }

            RunAfterEvents(eventToRun); //runs events that should be run after dialogue

            SetBools(eventToRun); //sets booleans configured in event

            SetCurrentEvents(); //updates events based on bools set in previous step
        }

        void CheckPlayerMovement(BaseDialogueEvent eventToRun) //if movement should be disabled, it is disabled
        {
            if (!eventToRun.enablePlayerMovementDuringDialogue)
            {
                //playerGO.GetComponent<PlayerController2D>().enabled = false; //disables player movement
                playerGO.GetComponent<Animator>().SetBool("isMoving", false); //disables walking animation
            }
        }

        void EnableMovementAfterDialogue() //enables movement again after dialogue
        {
            //playerGO.GetComponent<PlayerController2D>().enabled = true; //enables player movement
        }

        void SetCurrentEvents() //updates active events
        {
            bool processEvent; //initiating check if message should be processed
            BaseDialogueEvent thisEvent;
            for (int e = 0; e < eventOrDialogue.Count; e++)
            {
                processEvent = true;
                thisEvent = eventOrDialogue[e];
                //-----check for process if true options-----
                if (thisEvent.processOptions == BaseDialogueEvent.ProcessIfTrueOptions.ANY) //if any bools can be true
                {

                    if (thisEvent.processIfTrue.Count > 0) //if any processIfTrue vals exist
                    {
                        for (int i = 0; i < thisEvent.processIfTrue.Count; i++) //loop through each processIfTrue val
                        {
                            int whichBool = thisEvent.processIfTrue[i]; //gets the bool value
                                                                        //Debug.Log("Check do process: " + e + " - " + whichBool);
                            if (GameObject.Find("GameManager/DBs/GlobalBoolsDB").GetComponent<GlobalBoolsDB>().globalBools[whichBool]) //checks global booleans from assigned processIfTrue val. if matches:
                            {
                                //Debug.Log("Found do process: " + whichBool);
                                processEvent = true; //set process message to true
                            }
                        }
                    }
                }
                if (thisEvent.processOptions == BaseDialogueEvent.ProcessIfTrueOptions.ALL) //if all bools should be true
                {
                    if (thisEvent.processIfTrue.Count > 0) //if any processIfTrue vals exist
                    {
                        for (int i = 0; i < thisEvent.processIfTrue.Count; i++) //loops through each processIfTrue val
                        {
                            int whichBool = thisEvent.processIfTrue[i]; //gets the bool value
                                                                        //Debug.Log("Check do process: " + i + " - " + whichBool);
                            if (GameObject.Find("GameManager/DBs/GlobalBoolsDB").GetComponent<GlobalBoolsDB>().globalBools[whichBool]) //checks global bools from assigned processIfTrue val. if matches:
                            {
                                //Debug.Log("setting to true in ALL");
                                processEvent = true; //set process message to true and break from this loop
                                continue;
                            }
                            else
                            {
                                processEvent = false; //set process message to false and break from the dialogue loop
                                break;
                            }
                        }
                    }
                }

                //-----check for dont process if true options-----

                if (thisEvent.dontProcessOptions == BaseDialogueEvent.DontProcessIfTrueOptions.ANY) //if any bools can be false
                {
                    if (thisEvent.dontProcessIfTrue.Count > 0) //if any dontProcessIfTrue vals exist
                    {
                        for (int i = 0; i < thisEvent.dontProcessIfTrue.Count; i++) //loops through each dontProcessIfTrue val
                        {
                            int whichBool = thisEvent.dontProcessIfTrue[i]; //gets the bool value
                                                                            //Debug.Log("Check dont process: " + e + " - " + whichBool);
                            if (GameObject.Find("GameManager/DBs/GlobalBoolsDB").GetComponent<GlobalBoolsDB>().globalBools[whichBool]) //checks global bools from assigned dontProcessIfTrue val. if matches:
                            {
                                //Debug.Log("Found dont process: " + e + " - " + whichBool);
                                processEvent = false; //sets process message to false
                            }
                        }
                    }
                }
                if (thisEvent.dontProcessOptions == BaseDialogueEvent.DontProcessIfTrueOptions.ALL) //if all bools should be false
                {
                    if (thisEvent.dontProcessIfTrue.Count > 0) //if any dontProcessIfTrue vals exist
                    {

                        for (int i = 0; i < thisEvent.dontProcessIfTrue.Count; i++) //loops through each dontProcessIfTrue val
                        {
                            int whichBool = thisEvent.dontProcessIfTrue[i]; //gets the bool value
                                                                            //Debug.Log("Check dont process: " + i + " - " + whichBool);
                            if (GameObject.Find("GameManager/DBs/GlobalBoolsDB").GetComponent<GlobalBoolsDB>().globalBools[whichBool]) //checks global bools from assigned dontProcessIfTrue val. if matches:
                            {
                                //Debug.Log("Found dont process: " + whichBool);
                                processEvent = false; //sets process message to false and breaks from this loop
                                continue;
                            }
                            else
                            {
                                processEvent = true; //sets process message to true and breaks from dialogue loop
                                break;
                            }
                        }
                    }
                }

                if (e == 0 && eventsToRun.Count == 0)
                {
                    //Debug.Log("Setting to true as this is first event");
                    processEvent = true;
                }

                //Debug.Log("processMessage - " + e + " - " + processMessage);
                if (processEvent)
                {
                    //currentEvent = e;
                    //eventToRun = eventOrDialogue[currentEvent];

                    //---testing
                    if (!eventsToRun.Contains(thisEvent))
                    {
                        //Debug.Log("Adding event " + e + " to events list");
                        eventsToRun.Add(thisEvent);
                    }

                    foreach (BaseEvent runEvent in thisEvent.eventsBefore) //adds all events to be run before dialogue to the active scripts
                    {
                        if (!activeScripts.Contains(dialogueEvents[runEvent.index]))
                        {
                            Debug.Log("Adding to active scripts - " + dialogueEvents[runEvent.index]);
                            activeScripts.Add(dialogueEvents[runEvent.index]);
                        }
                    }
                    foreach (BaseEvent runEvent in thisEvent.eventsAfter) //adds all events to be run after dialogue to the active scripts
                    {
                        if (!activeScripts.Contains(dialogueEvents[runEvent.index]))
                        {
                            Debug.Log("Adding to active scripts - " + dialogueEvents[runEvent.index]);
                            activeScripts.Add(dialogueEvents[runEvent.index]);
                        }
                    }
                }
                else //if processEvent is false, all scripts that shouldn't be run are removed from activeScripts
                {
                    if (eventsToRun.Contains(thisEvent))
                    {
                        //Debug.Log("Removing event " + e + " from events list");
                        eventsToRun.Remove(thisEvent);
                    }
                    foreach (BaseEvent runEvent in thisEvent.eventsBefore)
                    {
                        if (activeScripts.Contains(dialogueEvents[runEvent.index]))
                        {
                            Debug.Log("Removing from active scripts - " + dialogueEvents[runEvent.index]);
                            activeScripts.Remove(dialogueEvents[runEvent.index]);
                        }
                    }
                    foreach (BaseEvent runEvent in thisEvent.eventsAfter)
                    {
                        if (activeScripts.Contains(dialogueEvents[runEvent.index]))
                        {
                            Debug.Log("Removing from active scripts - " + dialogueEvents[runEvent.index]);
                            activeScripts.Remove(dialogueEvents[runEvent.index]);
                        }
                    }
                }
            }
        }

        void RunBeforeEvents(BaseDialogueEvent eventToRun)
        {
            if (eventToRun.eventsBefore.Count > 0) //if any events to trigger before exist
            {
                foreach (BaseEvent runEvent in eventToRun.eventsBefore) //for each event to run before
                {
                    if (runEvent.method.Length > 0) //if there is a method added (there should be, but skips over if there is nothing)
                    {
                        dialogueEvents[runEvent.index].Invoke(runEvent.method, runEvent.waitTime); //run the method
                    }
                }
            }
        }

        IEnumerator RunDialogue(BaseDialogueEvent eventToRun)
        {
            if (!buttonPressed)
            {
                dialogueStarted = true;
                //bool messageFinished = false; //to check if message has finished processing
                messageText.text = "";
                //GameManager.instance.DisplayPanel(true); //shows message panel

                DisableOtherScripts();

                yield return AnimateMessageBox();

                for (int j = 0; j < eventToRun.dialogText.Length; j++) //actual dialog texts
                {
                    PlayVoice(eventToRun.voiceTone);

                    //messageFinished = false; //starting the dialogue text, sets to true once all text is added
                    var text = eventToRun.dialogText[j]; //gets the text to be put in dialogue UI
                                                         //var fullText = ""; //sets current fullText to blank as letters will be added individually


                    for (int i = 0; i < text.RichTextLength(); i++) //for each letter in the text
                    {
                        CheckConfirmButtonStatus(); //checks if confirm button is pressed to be able to skip message (not yet implemented)
                                                    //fullText += text[i]; //adds current letter to fullText
                                                    //messageText.text = fullText; //sets dialogue UI text to the current fullText
                                                    //fullText += text.RichTextSubString(i);
                        messageText.text = text.RichTextSubString(i + 1);
                        yield return new WaitForSecondsRealtime(textSpeed); //waits before entering next letter, based on textSpeed
                    }

                    StopVoice();

                    yield return new WaitUntil(() => Input.GetButtonDown("Confirm")); //wait until confirm button pressed before continuing
                    PlaySE(confirmSE);
                }
            }


            yield return new WaitUntil(() => Input.GetButtonDown("Confirm")); //wait until confirm button pressed before continuing

            yield return AnimateMessageBox();

            EnableOtherScripts();

            messageText.text = ""; //resets message text
                                   //GameManager.instance.DisplayPanel(false); //hides message panel
            dialogueStarted = false;
        }

        void RunAfterEvents(BaseDialogueEvent eventToRun)
        {
            if (eventToRun.eventsAfter.Count > 0) //if any events to be run after dialogue
            {
                bool dontProcess = false;
                foreach (int index in eventToRun.dontProcessIfTrue)
                {
                    if (GameObject.Find("GameManager/DBs/GlobalBoolsDB").GetComponent<GlobalBoolsDB>().globalBools[index])
                    {
                        dontProcess = true;
                    }
                }
                if (!dontProcess)
                {
                    foreach (BaseEvent runEvent in eventToRun.eventsAfter)
                    {
                        if (runEvent.method.Length > 0)
                        {
                            dialogueEvents[runEvent.index].Invoke(runEvent.method, runEvent.waitTime); //run the script from the assigned event
                        }
                    }
                }
            }
        }

        void SetBools(BaseDialogueEvent eventToRun)
        {
            if (eventToRun.markAsTrue.Count > 0) //if any bools in markAsTrue
            {
                foreach (int boolToChange in eventToRun.markAsTrue)
                {
                    if (!GameObject.Find("GameManager/DBs/GlobalBoolsDB").GetComponent<GlobalBoolsDB>().globalBools[boolToChange])
                    {
                        Debug.Log("Marking global bool " + boolToChange + " as true");
                        GameObject.Find("GameManager/DBs/GlobalBoolsDB").GetComponent<GlobalBoolsDB>().globalBools[boolToChange] = true; //sets the global bool from input to true
                    }
                }
            }

            if (eventToRun.markAsFalse.Count > 0) //if any bools in markAsFalse
            {
                foreach (int boolToChange in eventToRun.markAsFalse)
                {
                    Debug.Log("Marking global bool " + boolToChange + " as false");
                    GameObject.Find("GameManager/DBs/GlobalBoolsDB").GetComponent<GlobalBoolsDB>().globalBools[boolToChange] = false; //sets the global bool from input to false
                }
            }
        }

        void CheckConfirmButtonStatus() //keeps confirm button sensory to only be input once
        {
            if (Input.GetButtonDown("Confirm"))
            {
                //Debug.Log("buttonPressed");
                buttonPressed = true;
            }
            if (buttonPressed)
            {
                if (Input.GetButtonUp("Confirm"))
                {
                    //Debug.Log("button released");
                    buttonPressed = false;
                }
            }
        }

        void PlayVoice(AudioClip voiceTone)
        {
            voiceAudioSource.clip = voiceTone;
            voiceAudioSource.Play();
        }

        void StopVoice()
        {
            voiceAudioSource.Stop();
        }

        void DisableOtherScripts() //disables other scripts being run while dialogue is processed
        {
            for (int i = 0; i < activeScripts.Count; i++)
            {
                activeScripts[i].otherEventRunning = true;
            }
        }

        void EnableOtherScripts() //enables other scripts again after dialogue is done
        {
            for (int i = 0; i < activeScripts.Count; i++)
            {
                activeScripts[i].otherEventRunning = false;
            }
        }

        /// <summary>
        /// Coroutine.  Facilitates processing of message panel animations
        /// </summary>
        IEnumerator AnimateMessageBox()
        {
            if (messagePanelAnimator != null)
            {
                bool isOpen = messagePanelAnimator.GetBool("open");

                messagePanelAnimator.SetBool("open", !isOpen);
            }

            yield return new WaitForSeconds(GetAnimationTime(messagePanelAnimator));
        }

        /// <summary>
        /// Returns full animation time in seconds by given animator component
        /// </summary>
        /// <param name="anim">Animator component to measure animation time</param>
        float GetAnimationTime(Animator anim)
        {
            float animTime = 0f;
            RuntimeAnimatorController ac = anim.runtimeAnimatorController;    //Get Animator controller
            for (int i = 0; i < ac.animationClips.Length; i++)                 //For all animations
            {
                animTime = ac.animationClips[i].length;
            }

            return animTime;
        }

        /// <summary>
        /// Plays given sound effect once
        /// </summary>
        /// <param name="SE">Sound effect to play</param>
        void PlaySE(AudioClip SE)
        {
            audioSource.PlayOneShot(SE);
        }
    }
}
