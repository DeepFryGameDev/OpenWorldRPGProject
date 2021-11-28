using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LocationDBEntry
{
    public string sector;
    public enum Cities
    {
        Debug_Land
    }

    public Cities city;
}
