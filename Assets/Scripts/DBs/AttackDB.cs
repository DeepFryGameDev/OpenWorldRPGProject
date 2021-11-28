using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackDB : MonoBehaviour
{
    public List<AttackDBEntry> attacks = new List<AttackDBEntry>();
    public List<TravelPowerDBEntry> travelPowers = new List<TravelPowerDBEntry>();

    public enum powerClasses
    {
        FIRE,
        ICE,
        THUNDER
    }

    public enum travelPowerClasses
    {
        FLIGHT,
        LEAPING,
        RUNNING
    }

    #region Singleton
    public static AttackDB instance; //call instance to get the single active AttackDB for the game

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

    public BasePower GetAttack(int ID)
    {
        return attacks[ID].power;
    }
}
