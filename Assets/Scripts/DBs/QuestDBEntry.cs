using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestDBEntry
{
    [HideInInspector] public string Name;
    [HideInInspector] public int ID;

    public BaseQuest quest;

    public void SetName()
    {
        Name = ID.ToString() + ") " + quest.name;
    }
}
