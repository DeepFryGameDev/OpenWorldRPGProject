using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionData : MonoBehaviour //for encounter regions
{
    //public int maxAmountEnemies = 4; //only 4 spawn points for now but can change in inspector
    //public List<GameObject> possibleEnemies = new List<GameObject>(); //which enemies to be able to be encountered - change in inspector
    public List<TroopEncounters> troopEncounters = new List<TroopEncounters>();
}
