using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class Inventory : MonoBehaviour //contains all items in active inventory
{
    public delegate void OnItemchanged();

    public OnItemchanged onItemChangedCallback;

    public List<Item> items = new List<Item>(); //active items

    public void Add (Item item) //adds item to inventory
    {
        items.Add(item);

        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }
    }

    public void Remove (Item item) //removes item from inventory
    {
        items.Reverse();
        items.Remove(item);
        items.Reverse();

        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }
    }

    
}
