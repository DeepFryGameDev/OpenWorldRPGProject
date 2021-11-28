using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseQuestGatherRequirement
{
    public int itemID;
    public int quantity;
    [ReadOnly] public int inInventory;

    public GameObject waypoint;
}
