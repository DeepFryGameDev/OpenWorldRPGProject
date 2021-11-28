using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeepFry
{

    public class ScrollInteraction : MonoBehaviour
    {
        BaseHero playerHero;

        /*Script Values

        0 - Firestorm


        */

        public Item item;
        public bool removeItem;

        public void ProcessScroll()
        {
            playerHero = GameObject.Find("PlayerManager").GetComponent<PlayerManager>().playersCharacter;

            switch (item.scriptValue)
            {
                case 0: //Firestorm
                    if (!HasPowerLearned(6))
                    {
                        TeachPower(6);
                        removeItem = true;
                    }
                    break;
            }
        }

        bool HasPowerLearned(int ID)
        {
            foreach (HeroAttackDBEntry hadbe in playerHero.powers)
            {
                if (hadbe.ID == ID)
                {
                    return true;
                }
            }
            return false;
        }

        void TeachPower(int ID)
        {
            HeroAttackDBEntry hadbe = new HeroAttackDBEntry
            {
                ID = ID
            };

            playerHero.powers.Add(hadbe);
        }
    }

}