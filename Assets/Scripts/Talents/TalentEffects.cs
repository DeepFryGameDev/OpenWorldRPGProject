using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeepFry
{
    public class TalentEffects
    {
        public void AddEffect(string effect, BaseHero hero)
        {
            hero.GetCurrentStatsFromEquipment();

            Debug.Log("Adding Talent Effect: " + effect);

            if (effect == "Test1")
            {
                //hero.finalStamina = hero.postEquipmentStamina * 2;
            }
            if (effect == "Test2")
            {
                //hero.finalAgility = hero.postEquipmentAgility * 2;
            }
            if (effect == "Test3")
            {
                //hero.finalStrength = hero.postEquipmentStrength * 2;
            }
        }
    }
}

