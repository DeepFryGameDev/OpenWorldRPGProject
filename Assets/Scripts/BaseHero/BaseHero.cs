using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DeepFry
{

    [System.Serializable]
    public class BaseHero
    {
        #region HideInInspector
        [HideInInspector] public int preEquipmentHP;
        [HideInInspector] public int preEquipmentEP;
        [HideInInspector] public int preEquipmentPP;

        [HideInInspector] public int preEquipmentSTR; //for calculating physical attack damage
        [HideInInspector] public int preEquipmentSTA;
        [HideInInspector] public int preEquipmentAGI; //for calculating ATB gauge speed
        [HideInInspector] public int preEquipmentCHA; //for calculating dodge/crit (not yet implemented)
        [HideInInspector] public int preEquipmentINT; //for calculating magic damage
        [HideInInspector] public int preEquipmentEND; //for calculating END regeneration, magic defense (not yet implemented)
        [HideInInspector] public int preEquipmentCON;
        [HideInInspector] public int preEquipmentLUK;
        [HideInInspector] public int preEquipmentWIS;
        [HideInInspector] public int preEquipmentPER;

        [HideInInspector] public int preEquipmentMGT;
        [HideInInspector] public int preEquipmentPOW;
        [HideInInspector] public int preEquipmentDEF;
        [HideInInspector] public int preEquipmentPDEF;

        [HideInInspector] public int fromEquipmentHP;
        [HideInInspector] public int fromEquipmentEP;
        [HideInInspector] public int fromEquipmentPP;

        [HideInInspector] public int fromEquipmentSTR; //for calculating physical attack damage
        [HideInInspector] public int fromEquipmentSTA;
        [HideInInspector] public int fromEquipmentAGI; //for calculating ATB gauge speed
        [HideInInspector] public int fromEquipmentCHA; //for calculating dodge/crit (not yet implemented)
        [HideInInspector] public int fromEquipmentINT; //for calculating magic damage
        [HideInInspector] public int fromEquipmentEND; //for calculating END regeneration, magic defense (not yet implemented)
        [HideInInspector] public int fromEquipmentCON;
        [HideInInspector] public int fromEquipmentLUK;
        [HideInInspector] public int fromEquipmentWIS;
        [HideInInspector] public int fromEquipmentPER;

        [HideInInspector] public int fromEquipmentMGT;
        [HideInInspector] public int fromEquipmentPOW;
        [HideInInspector] public int fromEquipmentDEF;
        [HideInInspector] public int fromEquipmentPDEF;

        [HideInInspector] public int postEquipmentHP;
        [HideInInspector] public int postEquipmentEP;
        [HideInInspector] public int postEquipmentPP;

        [HideInInspector] public int postEquipmentSTR; //for calculating physical attack damage
        [HideInInspector] public int postEquipmentSTA;
        [HideInInspector] public int postEquipmentAGI; //for calculating ATB gauge speed
        [HideInInspector] public int postEquipmentCHA; //for calculating dodge/crit (not yet implemented)
        [HideInInspector] public int postEquipmentINT; //for calculating magic damage
        [HideInInspector] public int postEquipmentEND; //for calculating END regeneration, magic defense (not yet implemented)
        [HideInInspector] public int postEquipmentCON;
        [HideInInspector] public int postEquipmentLUK;
        [HideInInspector] public int postEquipmentWIS;
        [HideInInspector] public int postEquipmentPER;

        [HideInInspector] public int postEquipmentMGT;
        [HideInInspector] public int postEquipmentPOW;
        [HideInInspector] public int postEquipmentDEF;
        [HideInInspector] public int postEquipmentPDEF;
        #endregion

        [ReadOnly] public string name;

        [ReadOnly] public int currentLevel = 1;
        [ReadOnly] public int currentExp = 0;

        [ReadOnly] public int curHP; //current HP
        [ReadOnly] public int baseMaxHP; //max HP

        [ReadOnly] public int curPP; //current HP
        [ReadOnly] public int baseMaxPP; //max HP

        [ReadOnly] public int curEP; //current HP
        [ReadOnly] public int baseMaxEP = 1000; //max MP

        [ReadOnly] public AttackDB.powerClasses primaryPowerClass;
        [ReadOnly] public AttackDB.powerClasses secondaryPowerClass;
        [ReadOnly] public AttackDB.travelPowerClasses travelPowerClass;

        [ReadOnly] public int oneHandExp;
        [ReadOnly] public int twoHandExp;
        [ReadOnly] public int explosivesExp;
        [ReadOnly] public int rangedExp;

        [ReadOnly] public int resilienceExp;
        [ReadOnly] public int fitnessExp;
        [ReadOnly] public int willpowerExp;
        [ReadOnly] public int lifestealExp;

        [ReadOnly] public int fireExp;
        [ReadOnly] public int frostExp;
        [ReadOnly] public int thunderExp;
        [ReadOnly] public int waterExp;
        [ReadOnly] public int natureExp;
        [ReadOnly] public int earthExp;
        [ReadOnly] public int windExp;
        [ReadOnly] public int mindExp;
        [ReadOnly] public int lightExp;
        [ReadOnly] public int darkExp;

        [ReadOnly] public Equipment[] equipment = new Equipment[System.Enum.GetNames(typeof(EquipmentSlot)).Length];

        [ReadOnly] public List<HeroAttackDBEntry> powers = new List<HeroAttackDBEntry>(); //unit's magic attacks

        [ReadOnly] public BasePower leftHandPower;
        [ReadOnly] public BasePower rightHandPower;

        [ReadOnly] public int baseMGT;
        [ReadOnly] public int basePOW;
        [ReadOnly] public int baseDEF;
        [ReadOnly] public int basePDEF;

        [ReadOnly] public int baseSTR; //for calculating physical attack damage
        [ReadOnly] public int baseSTA;
        [ReadOnly] public int baseAGI; //for calculating ATB gauge speed
        [ReadOnly] public int baseCHA; //for calculating dodge/crit (not yet implemented)
        [ReadOnly] public int baseINT; //for calculating magic damage
        [ReadOnly] public int baseEND; //for calculating END regeneration, magic defense (not yet implemented)
        [ReadOnly] public int baseCON;
        [ReadOnly] public int baseLUK;
        [ReadOnly] public int baseWIS;
        [ReadOnly] public int basePER;

        //later can add [ReadOnly] when done testing
        [ReadOnly] public int baseOneHand;
        [ReadOnly] public int baseTwoHand;
        [ReadOnly] public int baseExplosives;
        [ReadOnly] public int baseRanged;

        [ReadOnly] public int baseResilience;
        [ReadOnly] public int baseFitness;
        [ReadOnly] public int baseWillpower;
        [ReadOnly] public int baseLifesteal;

        [ReadOnly] public int baseFire;
        [ReadOnly] public int baseFrost;
        [ReadOnly] public int baseThun;
        [ReadOnly] public int baseWater;

        [ReadOnly] public int baseNature;
        [ReadOnly] public int baseEarth;
        [ReadOnly] public int baseWind;
        [ReadOnly] public int baseMind;
        [ReadOnly] public int baseLight;
        [ReadOnly] public int baseDark;

        //modifiers for leveling purposes.  The higher the modifier, the more effect they are at gaining that particular stat
        [ReadOnly] public float strMod;
        [ReadOnly] public float staMod;
        [ReadOnly] public float agiMod;
        [ReadOnly] public float chaMod;
        [ReadOnly] public float intMod;
        [ReadOnly] public float endMod;
        [ReadOnly] public float conMod;
        [ReadOnly] public float lukMod;
        [ReadOnly] public float wisMod;
        [ReadOnly] public float perMod;

        [ReadOnly] public float oneHandMod;
        [ReadOnly] public float twoHandMod;
        [ReadOnly] public float explosivesMod;
        [ReadOnly] public float rangedMod;

        [ReadOnly] public float resilienceMod;
        [ReadOnly] public float fitnessMod;
        [ReadOnly] public float willpowerMod;
        [ReadOnly] public float lifestealMod;

        [ReadOnly] public float fireMod;
        [ReadOnly] public float frostMod;
        [ReadOnly] public float thunMod;
        [ReadOnly] public float waterMod;
        [ReadOnly] public float natureMod;
        [ReadOnly] public float earthMod;
        [ReadOnly] public float windMod;
        [ReadOnly] public float mindMod;
        [ReadOnly] public float lightMod;
        [ReadOnly] public float darkMod;

        [ReadOnly] public int finalMaxHP;
        [ReadOnly] public int finalMaxPP;
        [ReadOnly] public int finalMaxEP;

        [ReadOnly] public int finalSTR; //for calculating physical attack damage
        [ReadOnly] public int finalSTA;
        [ReadOnly] public int finalAGI; //for calculating ATB gauge speed
        [ReadOnly] public int finalCHA; //for calculating dodge/crit (not yet implemented)
        [ReadOnly] public int finalINT; //for calculating magic damage
        [ReadOnly] public int finalEND; //for calculating END regeneration, magic defense (not yet implemented)
        [ReadOnly] public int finalCON;
        [ReadOnly] public int finalLUK;
        [ReadOnly] public int finalWIS;
        [ReadOnly] public int finalPER;

        [ReadOnly] public int finalMGT;
        [ReadOnly] public int finalPOW;
        [ReadOnly] public int finalDEF;
        [ReadOnly] public int finalPDEF;

        /// <summary>
        /// Sets all stat variables to base stats for later manipulation
        /// </summary>
        public void InitializeStats()
        {
            preEquipmentHP = baseMaxHP;
            preEquipmentEP = baseMaxEP;
            preEquipmentPP = baseMaxPP;

            preEquipmentSTR = baseSTR;
            preEquipmentSTA = baseSTA;
            preEquipmentAGI = baseAGI;
            preEquipmentCHA = baseCHA;
            preEquipmentINT = baseINT;
            preEquipmentEND = baseEND;
            preEquipmentCON = baseCON;
            preEquipmentLUK = baseLUK;
            preEquipmentWIS = baseWIS;
            preEquipmentPER = basePER;

            preEquipmentMGT = baseMGT;
            preEquipmentPOW = basePOW;
            preEquipmentDEF = baseDEF;
            preEquipmentPDEF = basePDEF;

            Debug.Log("preEquipHP: " + preEquipmentHP);

            UpdateStats();

            curHP = finalMaxHP;
            curEP = finalMaxEP;
            curPP = finalMaxPP;
        }

        /// <summary>
        /// Sets given equip to appropriate slot on hero's equipment slot, and removes it from the inventory
        /// </summary>
        /// <param name="newEquip">Equip to be set to equipment slot</param>
        public void Equip(Equipment newEquip)
        {
            if (GetEquipSlot(newEquip.equipmentSlot).GetComponent<ActiveEquipSlotInteraction>().hasEquip)
            {
                GameObject.Find("GameManager/PlayerManager").GetComponent<PlayerInventory>().Add(GetEquipID(equipment[(int)newEquip.equipmentSlot]), true);
            }

            equipment[(int)newEquip.equipmentSlot] = newEquip;

            GameObject.Find("GameManager/PlayerManager").GetComponent<PlayerInventory>().Remove(GetEquipID(newEquip), true);

            UpdateStats();
        }

        GameObject GetEquipSlot(EquipmentSlot equipSlot)
        {
            GameObject equipObj = null;

            switch (equipSlot)
            {
                case EquipmentSlot.CHEST:
                    equipObj = GameObject.Find("GameManager/Menus/InventoryMenuCanvas/MenuPanel/EquipPanel/" +
        "EquipColPanelL/EquipColL/ChestEquipSlot");
                    break;
                case EquipmentSlot.FEET:
                    equipObj = GameObject.Find("GameManager/Menus/InventoryMenuCanvas/MenuPanel/EquipPanel/" +
    "EquipColPanelL/EquipColL/BootsEquipSlot");
                    break;
                case EquipmentSlot.HANDS:
                    equipObj = GameObject.Find("GameManager/Menus/InventoryMenuCanvas/MenuPanel/EquipPanel/" +
        "EquipColPanelR/EquipColR/HandEquipSlot");
                    break;
                case EquipmentSlot.HEAD:
                    equipObj = GameObject.Find("GameManager/Menus/InventoryMenuCanvas/MenuPanel/EquipPanel/" +
    "EquipColPanelL/EquipColL/HelmEquipSlot");
                    break;
                case EquipmentSlot.LEFTARM:
                    equipObj = GameObject.Find("GameManager/Menus/InventoryMenuCanvas/MenuPanel/EquipPanel/" +
    "EquipRowPanel/EquipRow/LeftHandEquipSlot");
                    break;
                case EquipmentSlot.NECK:
                    equipObj = GameObject.Find("GameManager/Menus/InventoryMenuCanvas/MenuPanel/EquipPanel/" +
        "EquipColPanelR/EquipColR/NeckEquipSlot");
                    break;
                case EquipmentSlot.RELIC:
                    equipObj = GameObject.Find("GameManager/Menus/InventoryMenuCanvas/MenuPanel/EquipPanel/" +
    "EquipRowPanel/EquipRow/RelicEquipSlot");
                    break;
                case EquipmentSlot.RIGHTARM:
                    equipObj = GameObject.Find("GameManager/Menus/InventoryMenuCanvas/MenuPanel/EquipPanel/" +
    "EquipRowPanel/EquipRow/RightHandEquipSlot");
                    break;
                case EquipmentSlot.RING:
                    equipObj = GameObject.Find("GameManager/Menus/InventoryMenuCanvas/MenuPanel/EquipPanel/" +
        "EquipColPanelR/EquipColR/RingEquipSlot");
                    break;
                case EquipmentSlot.WRIST:
                    equipObj = GameObject.Find("GameManager/Menus/InventoryMenuCanvas/MenuPanel/EquipPanel/" +
        "EquipColPanelR/EquipColR/WristEquipSlot");
                    break;
                case EquipmentSlot.SHOULDERS:
                    equipObj = GameObject.Find("GameManager/Menus/InventoryMenuCanvas/MenuPanel/EquipPanel/" +
        "EquipColPanelL/EquipColL/ShoulderEquipSlot");
                    break;
            }

            return equipObj;
        }

        int GetEquipID(Equipment equip)
        {
            for (int i = 0; i < EquipmentDB.instance.equipment.Count; i++)
            {
                if (equip == EquipmentDB.instance.equipment[i].equipment)
                    return EquipmentDB.instance.equipment[i].ID;
            }

            return 0;
        }

        /// <summary>
        /// Removes equipment from given equipment slot, and adds it back to the inventory
        /// </summary>
        /// <param name="slotIndex">Index of the equipment slot to be removed</param>
        public void Unequip(int slotIndex)
        {
            if (equipment[slotIndex].name != null && equipment[slotIndex].name.Length > 0)
            {
                Equipment oldEquipment = equipment[slotIndex];

                BasePlayerItem bpi = new BasePlayerItem();
                bpi.isEquip = true;
                bpi.itemID = GetEquipID(oldEquipment);

                GameObject.Find("GameManager/PlayerManager").GetComponent<PlayerInventory>().inventory.Add(bpi);

                equipment[slotIndex] = null;
            }

            UpdateStats();
        }

        /// <summary>
        /// Removes all equipment from hero and adds them back to the inventory
        /// </summary>
        public void UnequipAll()
        {
            for (int i = 0; i < equipment.Length; i++)
            {
                Unequip(i);
            }

            UpdateStats();
        }

        /// <summary>
        /// Increases hero level and calls methods for stats to be increased
        /// </summary>
        public void LevelUp()
        {
            currentLevel++;
            //Debug.Log(name + " has leveled up from " + levelBeforeExp + " to " + currentLevel);
            ProcessStatLevelUps();
        }

        /// <summary>
        /// Increases base stat values using their modifiers and adds any potential new attacks
        /// </summary>
        public void ProcessStatLevelUps()
        {
            //Debug.Log("Strength: " + strength + ", strengthMod: " + strengthMod);
            //Debug.Log("New strength: " + strength);

            //Debug.Log("Stamina: " + stamina + ", staminaModifier: " + staminaModifier);
            //Debug.Log("New stamina: " + stamina);

            //Debug.Log("Intelligence: " + intelligence + ", intelligenceModifer: " + intelligenceModifier);
            //Debug.Log("New intelligence: " + intelligence);

            //Debug.Log("Spirit: " + spirit + ", spiritModifier: " + spiritModifier);
            //Debug.Log("New spirit: " + spirit);

            //Debug.Log("Dexterity: " + dexterity + ", dexterityModifier: " + dexterityModifier);
            //Debug.Log("New dexterity: " + dexterity);

            //Debug.Log("Agility: " + agility + ", agilityModifier: " + agilityModifier);
            //Debug.Log("New agility: " + agility);

            curHP = baseMaxHP; //if full heal should occur on levelup, using for debugging purposes for now
            curEP = baseMaxEP; //if END should be restored on levelup, using for debugging purposes for now
            curPP = baseMaxPP;

            UpdateStats();
        }

        /// <summary>
        /// Updates secondary stats and updates other variables for stats (from equipment, talents, etc)
        /// </summary>
        public void UpdateStats()
        {
            GetCurrentStatsFromEquipment();

            UpdateStatsFromTalents();
        }


        /// <summary>
        /// Sets hero stats after taking equipped items into account
        /// </summary>
        public void GetCurrentStatsFromEquipment()
        {
            preEquipmentHP = baseMaxHP;
            preEquipmentEP = baseMaxEP;
            preEquipmentPP = baseMaxPP;

            preEquipmentSTR = baseSTR;
            preEquipmentSTA = baseSTA;
            preEquipmentAGI = baseAGI;
            preEquipmentCHA = baseCHA;
            preEquipmentINT = baseINT;
            preEquipmentEND = baseEND;
            preEquipmentCON = baseCON;
            preEquipmentLUK = baseLUK;
            preEquipmentWIS = baseWIS;
            preEquipmentPER = basePER;

            preEquipmentMGT = baseMGT;
            preEquipmentPOW = basePOW;
            preEquipmentDEF = baseDEF;
            preEquipmentPDEF = basePDEF;

            int tempHP = 0, tempEP = 0, tempPP = 0;
            int tempStrength = 0, tempStamina = 0, tempAgility = 0, tempCharisma = 0, tempIntelligence = 0, tempEndurance = 0,
                tempConstitution = 0, tempLuck = 0, tempWisdom = 0, tempPerception = 0;
            int tempMGT = 0, tempPOW = 0, tempDEF = 0, tempPDEF = 0;

            foreach (Equipment equipment in equipment)
            {
                if (equipment != null)
                {
                    tempHP += Mathf.RoundToInt(equipment.Stamina * .75f);
                    tempEP += Mathf.RoundToInt(equipment.Endurance * .5f);
                    tempPP += Mathf.RoundToInt(equipment.Intelligence * .5f);

                    tempStrength += equipment.Strength;
                    tempStamina += equipment.Stamina;
                    tempAgility += equipment.Agility;
                    tempCharisma += equipment.Charisma;
                    tempIntelligence += equipment.Intelligence;
                    tempEndurance += equipment.Endurance;
                    tempConstitution += equipment.Constitution;
                    tempLuck += equipment.Luck;
                    tempWisdom += equipment.Wisdom;
                    tempPerception += equipment.Perception;

                    tempMGT += equipment.Might;
                    tempPOW += equipment.Power;
                    tempDEF += equipment.Defense;
                    tempPDEF += equipment.PowerDefense;
                }
            }

            fromEquipmentHP = GetMaxHP(fromEquipmentSTA, baseMaxHP);
            fromEquipmentEP = GetMaxEND(fromEquipmentEND, baseMaxEP);
            fromEquipmentPP = GetMaxHP(fromEquipmentPOW, baseMaxPP);

            fromEquipmentSTR = tempStrength;
            fromEquipmentSTA = tempStamina;
            fromEquipmentAGI = tempAgility;
            fromEquipmentCHA = tempCharisma;
            fromEquipmentINT = tempIntelligence;
            fromEquipmentEND = tempEndurance;
            fromEquipmentCON = tempConstitution;
            fromEquipmentLUK = tempLuck;
            fromEquipmentWIS = tempWisdom;
            fromEquipmentPER = tempPerception;

            fromEquipmentMGT = tempMGT;
            fromEquipmentPOW = tempPOW;
            fromEquipmentDEF = tempDEF;
            fromEquipmentPDEF = tempPDEF;

            Debug.Log("fromEquipHP: " + fromEquipmentHP);

            UpdatePostEquipmentStats();
        }

        /// <summary>
        /// Sets newly updated stats by adding base and equipment stats
        /// </summary>
        void UpdatePostEquipmentStats()
        {
            postEquipmentSTR = baseSTR + fromEquipmentSTR;
            postEquipmentSTA = baseSTA + fromEquipmentSTA;
            postEquipmentAGI = baseAGI + fromEquipmentAGI;
            postEquipmentCHA = baseCHA + fromEquipmentCHA;
            postEquipmentINT = baseINT + fromEquipmentINT;
            postEquipmentEND = baseEND + fromEquipmentEND;
            postEquipmentCON = baseCON + fromEquipmentCON;
            postEquipmentLUK = baseLUK + fromEquipmentLUK;
            postEquipmentWIS = baseWIS + fromEquipmentWIS;
            postEquipmentPER = basePER + fromEquipmentPER;

            postEquipmentMGT = baseMGT + fromEquipmentMGT;
            postEquipmentPOW = basePOW + fromEquipmentPOW;
            postEquipmentDEF = baseDEF + fromEquipmentDEF;
            postEquipmentPDEF = basePDEF + fromEquipmentPDEF;

            postEquipmentHP = GetMaxHP(postEquipmentHP, baseMaxHP);
            postEquipmentEP = GetMaxEND(postEquipmentEP, baseMaxEP);
            postEquipmentPP = GetMaxHP(postEquipmentPP, baseMaxPP);
        }

        /// <summary>
        /// Adds in post-equipment stats with any potential talents set as active
        /// </summary>
        public void UpdateStatsFromTalents()
        {
            finalSTR = postEquipmentSTR;
            finalSTA = postEquipmentSTA;
            finalAGI = postEquipmentAGI;
            finalCHA = postEquipmentCHA;
            finalINT = postEquipmentINT;
            finalEND = postEquipmentEND;
            finalCON = postEquipmentAGI;
            finalLUK = postEquipmentLUK;
            finalWIS = postEquipmentWIS;
            finalPER = postEquipmentPER;

            finalMGT = postEquipmentMGT;
            finalPOW = postEquipmentPOW;
            finalDEF = postEquipmentDEF;
            finalPDEF = postEquipmentPDEF;

            finalMaxHP = postEquipmentHP;
            finalMaxEP = postEquipmentEP;
            finalMaxPP = postEquipmentPP;

            TalentEffects effect = new TalentEffects();

            //insert talents or w/e here

            UpdateFinalStats();
        }

        /// <summary>
        /// Sets final stats and updates UI if needed
        /// </summary>
        void UpdateFinalStats()
        {
            finalMGT = GetMGT(finalSTR, finalMGT);
            finalPOW = GetPOW(finalINT, finalPOW);
            finalDEF = GetDEF(finalEND, finalDEF);
            finalPDEF = GetPDEF(finalEND, finalPDEF);

            finalMaxHP = GetMaxHP(finalEND, finalMaxHP);
            finalMaxEP = GetMaxEND(finalINT, finalMaxEP);
            finalMaxPP = GetMaxHP(finalPOW, finalMaxPP);

            if (curHP > finalMaxHP)
            {
                curHP = finalMaxHP;
            }

            if (curEP > finalMaxEP)
            {
                curEP = finalMaxEP;
            }

            if (curPP > finalMaxPP)
            {
                curPP = finalMaxPP;
            }
        }

        //----------------------------------------------------------------------------
        //Formulas for setting secondary stats
        //----------------------------------------------------------------------------

        public int GetMGT(int strength, int attack)
        {
            int ATK = Mathf.RoundToInt(attack + (strength * .5f));

            return ATK;
        }

        public int GetPOW(int intelligence, int magicAttack)
        {
            int MATK = Mathf.RoundToInt(magicAttack + (intelligence * .5f));

            return MATK;
        }

        public int GetDEF(int stamina, int defense)
        {
            int DEF = Mathf.RoundToInt(defense + (stamina * .6f));

            return DEF;
        }

        public int GetPDEF(int stamina, int magicDefense)
        {
            int MDEF = Mathf.RoundToInt(magicDefense + (stamina * .5f));

            return MDEF;
        }

        public int GetMaxHP(int stamina, int hp)
        {
            int HP = Mathf.RoundToInt(hp + (stamina * .75f));

            return HP;
        }

        public int GetMaxEND(int intelligence, int end)
        {
            int END = Mathf.RoundToInt(end + (intelligence * .75f));

            return END;
        }
    }

}