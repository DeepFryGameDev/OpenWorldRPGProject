using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseQuestBoolRequirement
{
    public string objectiveDescription;
    [ReadOnly] public bool objectiveFulfilled;

    public GameObject waypoint;
}
