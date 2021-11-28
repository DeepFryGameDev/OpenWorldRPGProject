using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public string name;

    public Sprite icon = null;
    public GameObject fieldPrefab;

    public string description;
    public float weight;
    public bool usableInMenu;
    public int sellValue;

    [HideInInspector] public bool isFavorite;

    public enum Types
    {
        MISC,
        WEAPON,
        ARMOR,
        CLOTHING,
        POTION,
        SCROLL,
        FOOD,
        BEVERAGE,
        BOOK,
        INGREDIENT,
        QUEST,
        GOLD
    }

    public Types type;

    public int scriptValue;
}
