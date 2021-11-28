using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyDB : MonoBehaviour
{
    public List<BaseEnemyDBEntry> enemies = new List<BaseEnemyDBEntry>();

    #region Singleton
    public static EnemyDB instance; //call instance to get the single active EnemyDB for the game

    private void Awake()
    {
        if (instance != null)
        {
            //Debug.LogWarning("More than one instance of EnemyDB found!");
            return;
        }

        instance = this;
    }
    #endregion

    public BaseEnemy GetEnemy(int ID)
    {
        return enemies[ID].enemy;
    }
}
