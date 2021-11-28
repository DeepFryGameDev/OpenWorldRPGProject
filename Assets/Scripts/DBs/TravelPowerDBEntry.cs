using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TravelPowerDBEntry
{
    [HideInInspector] public string Name;
    [ReadOnly] public int ID;

    public BaseTravelPower power;

    public void SetName()
    {
        Name = ID.ToString() + ")" + power.name;
    }
}
