using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalBoolsDB : MonoBehaviour
{
    public List<bool> globalBools = new List<bool>();

    #region Singleton
    public static GlobalBoolsDB instance; //call instance to get the single active GlobalBoolsDB for the game

    private void Awake()
    {
        if (instance != null)
        {
            //Debug.LogWarning("More than one instance of GlobalBoolsDB found!");
            return;
        }

        instance = this;
    }
    #endregion
}
