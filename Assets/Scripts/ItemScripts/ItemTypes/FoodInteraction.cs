using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeepFry
{

    public class FoodInteraction : MonoBehaviour
    {
        BaseHero playerHero;

        /*Script Values

        0 - Test Food


        */

        public Item item;

        public void ProcessFood()
        {
            playerHero = GameObject.Find("PlayerManager").GetComponent<PlayerManager>().playersCharacter;

            switch (item.scriptValue)
            {
                case 0: //Test Food
                    AddRegenBuff(2, 10.5f, 150);
                    RestoreHunger(10);
                    break;
            }
        }

        void AddRegenBuff(int val, float secondsBetweenTicks, float totalTime)
        {
            Debug.Log("Adding regen buff that will tick for " + val + " every " + secondsBetweenTicks + " for a total of " + totalTime + " seconds.");
        }

        void RestoreHunger(int val)
        {
            Debug.Log("Restoring " + val + " hunger.");
        }
    }

}