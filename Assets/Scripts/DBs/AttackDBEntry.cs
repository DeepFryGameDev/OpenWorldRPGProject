using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackDBEntry
{
    [HideInInspector] public string Name;
    [ReadOnly] public int ID;

    public BasePower power;

    public void SetName()
    {
        Name = ID.ToString() + ") " + power.name;
    }
}
