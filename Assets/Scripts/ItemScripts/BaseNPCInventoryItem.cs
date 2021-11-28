using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseNPCInventoryItem
{
    public int ID;
    public bool isEquip;
    public float chance;

    public bool onlyOnQuest;
    public int questID;
}
