using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseEnemy
{
    public string name;

    public GameObject prefab;

    public string description;

    public int curHP; //current HP
    public int maxHP; //max HP

    public int curPP; //current PP
    public int maxPP; //max PP

    int curEP; //current SP
    [ReadOnly] int maxEP = 1000; //max SP

    public int baseMGT;
    public int basePOW;
    public int baseDEF;
    public int basePDEF;

    public int baseSTR; //for calculating physical attack damage
    public int baseEND;
    public int baseAGI; //for calculating ATB gauge speed
    public int baseCHA; //for calculating dodge/crit (not yet implemented)
    public int baseINT; //for calculating magic damage
    public int baseRES; //for calculating END regeneration, magic defense (not yet implemented)
    public int baseWIL;
    public int baseLUK;
    public int baseWIS;
    public int basePER;

    public int baseSlash;
    public int baseBlunt;
    public int basePierce;
    public int baseRanged;

    public int baseFire;
    public int baseFrost;
    public int baseThun;
    public int baseWater;
    public int baseNature;
    public int baseEarth;
    public int baseWind;
    public int baseMind;
    public int baseLight;
    public int baseDark;

    public List<HeroAttackDBEntry> Powers = new List<HeroAttackDBEntry>(); //unit's magic attacks

    public int exp;
    public int gold;
    public List<BaseNPCInventoryItem> items = new List<BaseNPCInventoryItem>();

    /// <summary>
    /// Sets all stat variables to base stats for later manipulation
    /// </summary>
    public void InitializeStats()
    {
        curHP = maxHP;
        curPP = maxPP;
        curEP = maxEP;
    }

    public BaseEnemy NewEnemy()
    {
        BaseEnemy copy = new BaseEnemy();

        copy.name = this.name;
        copy.prefab = this.prefab;
        copy.description = this.description;
        copy.curHP = this.curHP;
        copy.maxHP = this.maxHP;
        copy.curPP = this.curPP;
        copy.maxPP = this.maxPP;
        copy.curEP = this.curEP;
        copy.maxEP = this.maxEP;

        copy.baseMGT = this.baseMGT;
        copy.basePOW = this.basePOW;
        copy.baseDEF = this.baseDEF;
        copy.basePDEF = this.basePDEF;

        copy.baseSTR = this.baseSTR; //for calculating physical attack damage
        copy.baseEND = this.baseEND;
        copy.baseAGI = this.baseAGI; //for calculating ATB gauge speed
        copy.baseCHA = this.baseCHA; //for calculating dodge/crit (not yet implemented)
        copy.baseINT = this.baseINT; //for calculating magic damage
        copy.baseRES = this.baseRES; //for calculating END regeneration, magic defense (not yet implemented)
        copy.baseWIL = this.baseWIL;
        copy.baseLUK = this.baseLUK;
        copy.baseWIS = this.baseWIS;
        copy.basePER = this.basePER;

        copy.baseSlash = this.baseSlash;
        copy.baseBlunt = this.baseBlunt;
        copy.basePierce = this.basePierce;
        copy.baseRanged = this.baseRanged;

        copy.baseFire = this.baseFire;
        copy.baseFrost = this.baseFrost;
        copy.baseThun = this.baseThun;
        copy.baseWater = this.baseWater;
        copy.baseNature = this.baseNature;
        copy.baseEarth = this.baseEarth;
        copy.baseWind = this.baseWind;
        copy.baseMind = this.baseMind;
        copy.baseLight = this.baseLight;
        copy.baseDark = this.baseDark;

        copy.Powers = this.Powers;

        copy.exp = this.exp;
        copy.gold = this.gold;
        copy.items = this.items;


        return copy;
    }

    //----------------------------------------------------------------------------
    //Formulas for setting secondary stats
    //----------------------------------------------------------------------------

    public int GetATK(int strength, int attack)
    {
        int ATK = Mathf.RoundToInt(attack + (strength * .5f));

        return ATK;
    }

    public int GetMATK(int intelligence, int magicAttack)
    {
        int MATK = Mathf.RoundToInt(magicAttack + (intelligence * .5f));

        return MATK;
    }

    public int GetDEF(int stamina, int defense)
    {
        int DEF = Mathf.RoundToInt(defense + (stamina * .6f));

        return DEF;
    }

    public int GetMDEF(int stamina, int magicDefense)
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

    public int GetHitChance(int hitRating, int agility)
    {
        int hit = Mathf.FloorToInt(agility * .12f) + Mathf.FloorToInt(hitRating * .25f) + 75;

        if (hit > 100)
        {
            hit = 100;
        }

        return hit;
    }

    public int GetCritChance(int critRating, int dexterity)
    {
        int crit = Mathf.FloorToInt(dexterity * .12f) + Mathf.FloorToInt(critRating * .25f);

        if (crit > 100)
        {
            crit = 100;
        }
        return crit;
    }

    public int GetMoveRating(int moveRating, int dexterity)
    {
        int move = Mathf.FloorToInt(dexterity * .02f) + Mathf.CeilToInt(moveRating * .05f);

        return move;
    }

    public int GetRegen(int regenRating, int spirit)
    {
        int regen = Mathf.CeilToInt(spirit * .15f) + regenRating;

        return regen;
    }
}
