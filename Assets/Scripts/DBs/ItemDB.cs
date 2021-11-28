using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class ItemDB : MonoBehaviour
{
    public List<ItemDBEntry> items = new List<ItemDBEntry>();
   
    #region Singleton
    public static ItemDB instance; //call instance to get the single active ItemDB for the game

    private void Awake()
    {
        if (instance != null)
        {
            //Debug.LogWarning("More than one instance of ItemDB found!");
            return;
        }

        instance = this;
    }
    #endregion

    public ItemDBEntry GetItem(int ID)
    {
        return items[ID];
    }
}


