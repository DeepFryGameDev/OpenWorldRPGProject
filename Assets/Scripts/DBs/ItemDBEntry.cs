using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemDBEntry
{
    [HideInInspector] public string Name;
    [ReadOnly] public int ID;
    public Item item;

    public void SetName()
    {
        Name = ID.ToString() + ") " + item.name;
    }
}
