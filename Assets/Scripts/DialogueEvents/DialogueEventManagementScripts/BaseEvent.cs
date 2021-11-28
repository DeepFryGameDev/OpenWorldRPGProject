using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class BaseEvent
{
    public int index; //which event to run
    public float waitTime; //how long before running
    public string method; //which method from event script to run

    [System.NonSerialized] public string scriptType = "event";
}
