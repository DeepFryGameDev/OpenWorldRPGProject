using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeepFry
{

    public class PotionInteraction
    {
        BaseHero playerHero;

        /*Script Values

        0 - Test Potion


        */

        public Item item;
        public bool removeItem;

        public void ProcessPotion()
        {
            playerHero = GameObject.Find("PlayerManager").GetComponent<PlayerManager>().playersCharacter;

            switch (item.scriptValue)
            {
                case 0: //Test Potion
                    if (!isFullHealth())
                    {
                        RestoreHP(50);
                        removeItem = true;
                    }
                    break;
            }

        }

        void RestoreHP(int val)
        {
            Debug.Log("Restoring " + val + " HP.");
            playerHero.curHP += val;

            if (playerHero.curHP > playerHero.finalMaxHP)
            {
                playerHero.curHP = playerHero.finalMaxHP;
            }
        }

        bool isFullHealth()
        {
            if (playerHero.curHP == playerHero.finalMaxHP)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

}