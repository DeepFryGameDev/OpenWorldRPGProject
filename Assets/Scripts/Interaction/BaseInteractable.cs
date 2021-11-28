using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseInteractable : MonoBehaviour
{
    public enum TriggerActions //how to interact to begin event
    {
        ONACTION, //when pressing confirm button
        ONTOUCH, //when collders of player and this game object are touching
        AUTOSTART, //starts immediately and runs once
        PARALLEL //starts immediately and continues running
    }

    public TriggerActions triggerAction; //to access Trigger Action

    public List<BaseEvent> events = new List<BaseEvent>(); //For events to trigger at start of event interaction

    public enum ProcessIfTrueOptions //checking bools to trigger the event
    {
        ANY, //if any of the bools are true
        ALL //if all of the bools are true
    }

    public List<int> processIfTrue = new List<int>(); //list of global event bool indexes

    public List<int> dontProcessIfTrue = new List<int>(); //list of global event bool indexes

    public List<int> markAsTrue = new List<int>(); //which global event bools to mark as true
    public List<int> markAsFalse = new List<int>(); //which global event bools to mark as false
}
