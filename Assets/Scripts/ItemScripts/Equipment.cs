using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Equipment : Item
{
    //new public string name = "New Equipment";
    //public string description = "Equipment Description";
    //public Sprite icon = null;

    public EquipmentSlot equipmentSlot;

    public int Might;
    public int Defense;

    public int Power;
    public int PowerDefense;

    public int Strength;
    public int Stamina;
    public int Constitution;
    public int Endurance;
    public int Agility;
    public int Charisma;
    public int Intelligence;
    public int Wisdom;
    public int Luck;    
    public int Perception;

    public Equipment NewEquipment()
    {
        Equipment copy = new Equipment();

        copy.name = name;

        copy.equipmentSlot = equipmentSlot;

        copy.Might = Might;
        copy.Defense = Defense;
        copy.Power = Power;
        copy.PowerDefense = PowerDefense;

        copy.Strength = Strength;
        copy.Stamina = Stamina;
        copy.Constitution = Constitution;
        copy.Endurance = Endurance;
        copy.Agility = Agility;
        copy.Charisma = Charisma;
        copy.Intelligence = Intelligence;
        copy.Wisdom = Wisdom;
        copy.Luck = Luck;
        copy.Perception = Perception;

        return copy;
    }
}

public enum EquipmentSlot { HEAD, CHEST, SHOULDERS, WRIST, HANDS, FEET, RING, RELIC, NECK, RIGHTARM, LEFTARM }
