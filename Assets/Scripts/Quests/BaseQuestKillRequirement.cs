using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseQuestKillRequirement
{
    public int enemyID;
    public int quantity;
    [ReadOnly] public int targetsKilled;

    public GameObject waypoint;
}
