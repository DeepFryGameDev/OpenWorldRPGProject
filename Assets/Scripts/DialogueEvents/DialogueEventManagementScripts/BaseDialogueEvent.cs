using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class BaseDialogueEvent
{
    public enum TriggerActions //how to interact to begin event
    {
        ONACTION, //when pressing confirm button
        ONTOUCH, //when collders of player and this game object are touching
        AUTOSTART, //starts immediately and runs once
        PARALLEL //starts immediately and continues running
    }

    public TriggerActions triggerAction; //to access Trigger Action

    //not yet used but will be private bools specific to this event
    public bool switch1; 
    public bool switch2;

    //graphic for face - to implement

    public AudioClip voiceTone;

    public List<BaseEvent> eventsBefore = new List<BaseEvent>(); //For events to trigger at start of event interaction

    public bool enablePlayerMovementDuringDialogue;

    [TextArea(1, 5)]  //Dialog text for the event
    public string[] dialogText;
       
    public List<BaseEvent> eventsAfter = new List<BaseEvent>(); //For events to trigger at end of event interaction
    
    public enum ProcessIfTrueOptions //checking bools to trigger the event
    {
        ANY, //if any of the bools are true
        ALL //if all of the bools are true
    }
    public ProcessIfTrueOptions processOptions; //to access Process If True Options
    public List<int> processIfTrue = new List<int>(); //list of global event bool indexes

    public enum DontProcessIfTrueOptions //checking bools to skip the event
    {
        ANY, //if any of the bools are true
        ALL //if all of the bools are true
    }
    public DontProcessIfTrueOptions dontProcessOptions; //to access Dont Process If True Options
    public List<int> dontProcessIfTrue = new List<int>(); //list of global event bool indexes

    public List<int> markAsTrue = new List<int>(); //which global event bools to mark as true
    public List<int> markAsFalse = new List<int>(); //which global event bools to mark as false
}
