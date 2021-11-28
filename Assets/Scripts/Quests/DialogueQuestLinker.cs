using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

namespace DeepFry
{
    public class DialogueQuestLinker : MonoBehaviour
    {
        BaseScriptedEvent bse;
        PlayerInventory pi;
        GameManager gm;

        // Start is called before the first frame update
        void Start()
        {
            bse = gameObject.GetComponent<BaseScriptedEvent>();
            pi = GameObject.Find("PlayerManager").GetComponent<PlayerInventory>();
            gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnEnable()
        {
            #region ---QUEST MANAGEMENT---

            //SCRIPTS
            Lua.RegisterFunction("AcceptQuest", this, SymbolExtensions.GetMethodInfo(() => AcceptQuest((double)0)));
            Lua.RegisterFunction("CompleteQuest", this, SymbolExtensions.GetMethodInfo(() => CompleteQuest((double)0)));
            Lua.RegisterFunction("SetQuestBool", this, SymbolExtensions.GetMethodInfo(() => SetQuestBool((double)0, (double)0, (bool)false)));

            //CONDITIONS
            Lua.RegisterFunction("IfQuestIsActive", this, SymbolExtensions.GetMethodInfo(() => IfQuestIsActive((double)0)));
            Lua.RegisterFunction("IfQuestIsCompleted", this, SymbolExtensions.GetMethodInfo(() => IfQuestIsCompleted((double)0)));
            Lua.RegisterFunction("QuestObjectivesFulfilled", this, SymbolExtensions.GetMethodInfo(() => QuestObjectivesFulfilled((double)0)));
            Lua.RegisterFunction("GetQuestBool", this, SymbolExtensions.GetMethodInfo(() => GetQuestBool((double)0, (double)0)));
            #endregion

            #region ---GAME MANAGEMENT---
            //Game Management

            //SCRIPTS
            Lua.RegisterFunction("SetGlobalBool", this, SymbolExtensions.GetMethodInfo(() => SetGlobalBool((double)0, (bool)false)));

            //CONDITIONS
            Lua.RegisterFunction("GetGlobalBool", this, SymbolExtensions.GetMethodInfo(() => GetGlobalBool((double)0)));
            #endregion

            #region ---PLAYER MANAGEMENT---
            //SCRIPTS
            Lua.RegisterFunction("ChangeDefaultMoveSpeed", this, SymbolExtensions.GetMethodInfo(() => ChangeDefaultMoveSpeed((double)0)));
            Lua.RegisterFunction("EnableWalkingAnimation", this, SymbolExtensions.GetMethodInfo(() => EnableWalkingAnimation((bool)false)));
            Lua.RegisterFunction("EnablePlayerMovement", this, SymbolExtensions.GetMethodInfo(() => EnablePlayerMovement((bool)false)));
            Lua.RegisterFunction("SetTravelMode", this, SymbolExtensions.GetMethodInfo(() => SetTravelMode((double)0)));

            //CONDITIONS

            #endregion

            #region ---AUDIO/SE MANAGEMENT---
            //SCRIPTS
            //Lua.RegisterFunction("PlaySE", this, SymbolExtensions.GetMethodInfo(() => PlaySE((AudioClip)null, (AudioSource)null))); <-- need to figure out how to pass objects in

            //CONDITIONS

            #endregion

            #region ---SHOP MANAGEMENT---
            Lua.RegisterFunction("OpenShop", this, SymbolExtensions.GetMethodInfo(() => OpenShop()));
            #endregion

            #region ---ITEM MANAGEMENT---
            //SCRIPTS
            Lua.RegisterFunction("AddToPlayerInventory", this, SymbolExtensions.GetMethodInfo(() => AddToPlayerInventory((double)0, (bool)false)));
            Lua.RegisterFunction("RemoveFromPlayerInventory", this, SymbolExtensions.GetMethodInfo(() => RemoveFromPlayerInventory((double)0, (bool)false)));

            //CONDITIONS

            #endregion

            #region ---SEQUENCER---
            //SHORTCUTS

            #endregion
        }

        #region ---QUEST MANAGEMENT---

        public void AcceptQuest(double ID)
        {
            Debug.Log("Adding " + ID + " to active quests");

            BaseQuest newQuest = new BaseQuest();

            newQuest = QuestDB.instance.quests[(int)ID].quest.NewQuest();

            QuestManager.instance.activeQuests.Add(newQuest);

            SetDialogueSystemVariable("quest_" + ID.ToString() + "_accepted", true);

            //perform any other actions for adding quest to active
        }

        public void CompleteQuest(double ID)
        {
            Debug.Log("Adding " + ID + " to complete quests");

            BaseQuest newQuest = QuestManager.instance.activeQuests[GetActiveQuestIndex((int)ID)];

            QuestManager.instance.completedQuests.Add(newQuest);
            QuestManager.instance.activeQuests.Remove(newQuest);

            SetDialogueSystemVariable("quest_" + ID.ToString() + "_completed", true);

            //perform any other actions for adding quest to active
        }

        /// <summary>
        /// Returns quest.fulfilled for given quest
        /// </summary>
        /// <param name="quest">Quest to check</param>
        public bool QuestObjectivesFulfilled(double ID)
        {
            return bse.QuestObjectivesFulfilled((int)ID);
        }

        /// <summary>
        /// Marks bool of given quest as given value if quest type is 'bool'
        /// </summary>
        /// <param name="quest">Quest to check</param>
        /// <param name="index">Index of bool in quest</param>
        /// <param name="value">To mark the bool as true or false</param>
        public void SetQuestBool(double ID, double index, bool value)
        {
            bse.SetQuestBool((int)ID, (int)index, value);
        }

        /// <summary>
        /// Returns given quest and index bool value
        /// </summary>
        /// <param name="quest">Quest to check</param>
        /// <param name="index">Index of bool in quest</param>
        public bool GetQuestBool(double ID, double index)
        {
            return bse.GetQuestBool((int)ID, (int)index);
        }

        /// <summary>
        /// Returns if given quest is in active quest list
        /// </summary>
        /// <param name="quest">Quest to check</param>
        public bool IfQuestIsActive(double ID)
        {
            return bse.IfQuestIsActive((int)ID);
        }

        /// <summary>
        /// Returns if given quest is in completed quest list
        /// </summary>
        /// <param name="quest">Quest to check</param>
        public bool IfQuestIsCompleted(double ID)
        {
            return bse.IfQuestIsCompleted((int)ID);
        }

        #endregion

        #region ---GAME MANAGEMENT---

        public bool GetGlobalBool(double ID)
        {
            return GlobalBoolsDB.instance.globalBools[(int)ID];
        }

        public void SetGlobalBool(double ID, bool value)
        {
            GlobalBoolsDB.instance.globalBools[(int)ID] = value;
        }

        /// <summary>
        /// Forces menu to be opened
        /// </summary>
        public void OpenMenu()
        {
            bse.OpenMenu();
        }

        #endregion

        #region ---PLAYER MANAGEMENT---

        public void ChangeDefaultMoveSpeed(double newMoveSpeed)
        {
            bse.ChangeDefaultMoveSpeed((float)newMoveSpeed);
        }

        /// <summary>
        /// Sets player animation
        /// </summary>
        public void EnableWalkingAnimation(bool enable)
        {
            bse.EnableWalkingAnimation(enable);
        }

        /// <summary>
        /// Turns player's movement on/off
        /// </summary>
        public void EnablePlayerMovement(bool enable)
        {
            bse.EnablePlayerMovement(enable);
        }

        /// <summary>
        /// Changes player's current equipped travel power
        /// </summary>
        public void SetTravelMode(double mode)
        {
            TravelPowers travelPowers = GameObject.Find("PlayerManager").GetComponent<TravelPowers>();

            switch (mode)
            {
                case 0:
                    travelPowers.travelMode = TravelPowers.TravelModes.IDLE;
                    break;
                case 1:
                    travelPowers.travelMode = TravelPowers.TravelModes.FLIGHT;
                    break;

                case 2:
                    travelPowers.travelMode = TravelPowers.TravelModes.JUMP;
                    break;

                case 3:
                    travelPowers.travelMode = TravelPowers.TravelModes.SPEED;
                    break;
            }
        }

        #endregion

        #region ---SHOP MANAGEMENT---
        public void OpenShop()
        {
            Debug.Log("Opening shop for " + gm.lastInteracted.name);
            bse.OpenShop(gm.lastInteracted);
        }
        #endregion

        #region ---ITEM MANAGEMENT---
        void AddToPlayerInventory(double ID, bool isEquip)
        {
            pi.Add((int)ID, isEquip);
        }

        void RemoveFromPlayerInventory(double ID, bool isEquip)
        {
            pi.Remove((int)ID, isEquip);
        }

        #endregion

        #region ---SCENE MANAGEMENT---

        /// <summary>
        /// Saves the position of player for loadPosition method (Not yet implemented)
        /// </summary>
        /// <param name="objectToSave">GameObject to save position</param>
        public void SavePosition(GameObject objectToSave)
        {
            bse.SavePosition(objectToSave);
        }

        #endregion

        #region ---AUDIO/SE MANAGEMENT---

        /// <summary>
        /// Plays given sound effect once
        /// </summary>
        /// <param name="SE">Sound effect to play</param>
        public void PlaySE(AudioClip SE, AudioSource source)
        {
            bse.PlaySE(SE, source);
        }

        #endregion

        //Tools

        int GetActiveQuestIndex(double ID)
        {
            for (int i = 0; i < QuestManager.instance.activeQuests.Count; i++)
            {
                if (QuestManager.instance.activeQuests[i].ID == (int)ID)
                {
                    return i;
                }
            }
            return 0;
        }

        void SetDialogueSystemVariable(string variableName, bool value)
        {
            Debug.Log("Setting " + variableName + " to " + value);
            DialogueLua.SetVariable(variableName, value);
        }
    }
}
