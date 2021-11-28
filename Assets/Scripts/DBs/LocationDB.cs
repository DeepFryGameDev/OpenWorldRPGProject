using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class LocationDB : MonoBehaviour
{
    public List<LocationDBEntry> locations = new List<LocationDBEntry>();

    #region Singleton
    public static LocationDB instance; //call instance to get the single active ItemDB for the game

    private void Awake()
    {
        if (instance != null)
        {
            //Debug.LogWarning("More than one instance of ItemDB found!");
            return;
        }

        instance = this;
    }
    #endregion

    public void DisplaySector(string sector)
    {
        GameObject.Find("GameManager/Menus/MainMenuCanvas/MainMenuPanel/LocationPanel/SectorText").GetComponent<Text>().text = sector;
        DisplayCity(sector); //city is which area the sector is located in
    }

    /// <summary>
    /// Displays city based on sector
    /// </summary>
    /// <param name="sector">Based on the scene name</param>
    public void DisplayCity(string sector) //could be updated for easier access
    {
        string city = "";

        foreach (LocationDBEntry dbe in locations)
        {
            string checkSector = dbe.sector.Replace("_", " ");

            if (checkSector.Equals(sector))
            {
                city = dbe.city.ToString().Replace("_", " ");
                break;
            }
        }        

        GameObject.Find("GameManager/Menus/MainMenuCanvas/MainMenuPanel/LocationPanel/CityText").GetComponent<Text>().text = city;
    }
}
