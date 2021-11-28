using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EquipmentDBEntry
{
    [HideInInspector] public string Name;    
    [ReadOnly] public int ID;

    public Equipment equipment;

    public void SetName()
    {
        Name = ID.ToString() + ") " + equipment.name;
    }
}
