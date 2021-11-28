using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeepFry
{

    public class BeverageInteraction : MonoBehaviour
    {
        BaseHero playerHero;

        /*Script Values

        0 - Test Water


        */

        public Item item;

        public void ProcessBeverage()
        {
            playerHero = GameObject.Find("PlayerManager").GetComponent<PlayerManager>().playersCharacter;

            switch (item.scriptValue)
            {
                case 0: //Test Water
                    AddEPRegenBuff(1, 2, 60);
                    RestoreThirst(20);
                    break;
            }
        }

        void AddEPRegenBuff(int val, float secondsBetweenTicks, float totalTime)
        {
            Debug.Log("Adding regen buff that will tick for " + val + " every " + secondsBetweenTicks + " for a total of " + totalTime + " seconds.");
        }

        void RestoreThirst(int val)
        {
            Debug.Log("Restoring " + val + " thirst.");
        }
    }

}