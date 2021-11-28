using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DeepFry
{

    public class ItemShopSlotInteraction : MonoBehaviour
    {
        [ReadOnly] public bool buyMode;

        [ReadOnly] public Item item;
        [ReadOnly] public int ID;
        [ReadOnly] public int count;
        [ReadOnly] public int cost;

        Text nameText;
        GameObject descObject;
        Text descText;
        Image icon;
        Text weightText;

        GameObject infoPanel;

        bool visible;

        BaseHero playerHero;
        PlayerInventory playerInventory;
        GameMenu menu;
        GameObject playerObj;

        ShopInteraction si;

        private void Start()
        {
            infoPanel = GameObject.Find("GameManager/ShopCanvas/ShopBG/InfoPanel");

            nameText = infoPanel.transform.Find("Name").GetComponent<Text>();
            descObject = infoPanel.transform.Find("ItemDesc").gameObject;

            descText = descObject.transform.Find("Description").GetComponent<Text>();

            icon = infoPanel.transform.Find("Icon").GetComponent<Image>();
            weightText = infoPanel.transform.Find("WeightVal").GetComponent<Text>();

            playerHero = GameObject.Find("GameManager/PlayerManager").GetComponent<PlayerManager>().playersCharacter;
            playerInventory = GameObject.Find("GameManager/PlayerManager").GetComponent<PlayerInventory>();
            menu = GameObject.Find("GameManager/Menus").GetComponent<GameMenu>();

            playerObj = GameObject.Find("Player");

            si = GameObject.Find("GameManager/ShopCanvas").GetComponent<ShopInteraction>();
        }

        private void Update()
        {
            if (visible && !si.inQuantitySelect && buyMode && (Input.GetButtonDown("Confirm") || Input.GetButtonDown("Fire1")))
            {
                si.tempCost = cost;
                si.tempID = ID;
                si.tempIsEquip = false;

                //Debug.Log(count + "," + si.quantityThreshold);

                if (count >= si.quantityThreshold) //open quantity window
                {
                    si.vendorsQuantityOfItem = count;
                    si.inQuantitySelect = true;

                    si.OpenQuantityPanel();
                }
                else //direct sale
                {
                    si.ProcessBuy();
                }
            }

            if (visible && !si.inQuantitySelect && !buyMode && (Input.GetButtonDown("Confirm") || Input.GetButtonDown("Fire1")))
            {
                si.tempCost = cost;
                si.tempID = ID;
                si.tempIsEquip = false;

                if (count >= si.quantityThreshold) //open quantity window
                {
                    si.inQuantitySelect = true;

                    si.OpenQuantityPanel();
                }
                else //direct sale
                {
                    si.ProcessSell();
                }
            }
        }

        public void Enter()
        {
            visible = true;
            ShowItemDetails();
        }

        public void Exit()
        {
            visible = false;
            HideItemDetails();
        }

        void ShowItemDetails()
        {
            nameText.text = item.name;
            descText.text = item.description;
            weightText.text = item.weight.ToString();
            icon.sprite = item.icon;

            descObject.GetComponent<CanvasGroup>().alpha = 1.0f;
            infoPanel.GetComponent<CanvasGroup>().alpha = 1.0f;
        }

        void HideItemDetails()
        {
            infoPanel.GetComponent<CanvasGroup>().alpha = 0.0f;
            descObject.GetComponent<CanvasGroup>().alpha = 0.0f;

            nameText.text = "";
            descText.text = "";
            weightText.text = "";
            icon.sprite = null;
        }
    }

}