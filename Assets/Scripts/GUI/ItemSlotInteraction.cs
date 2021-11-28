using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DeepFry
{

    public class ItemSlotInteraction : MonoBehaviour
    {
        [HideInInspector] public Item item;
        [HideInInspector] public int ID;

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

        private void Start()
        {
            infoPanel = GameObject.Find("GameManager/Menus/InventoryMenuCanvas/MenuPanel/InfoPanel");

            nameText = infoPanel.transform.Find("Name").GetComponent<Text>();
            descObject = infoPanel.transform.Find("ItemDesc").gameObject;

            descText = descObject.transform.Find("Description").GetComponent<Text>();

            icon = infoPanel.transform.Find("Icon").GetComponent<Image>();
            weightText = infoPanel.transform.Find("WeightVal").GetComponent<Text>();

            playerHero = GameObject.Find("GameManager/PlayerManager").GetComponent<PlayerManager>().playersCharacter;
            playerInventory = GameObject.Find("GameManager/PlayerManager").GetComponent<PlayerInventory>();
            menu = GameObject.Find("GameManager/Menus").GetComponent<GameMenu>();

            playerObj = GameObject.Find("Player");
        }

        private void Update()
        {
            if (visible && Input.GetButtonDown("Confirm"))
            {
                ActivateItem();
            }
            else if (visible && Input.GetButtonDown("Secondary"))
            {
                DropItem();
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

        void ActivateItem()
        {
            bool removeItem = false;

            //do item processing
            if (item.usableInMenu)
            {
                switch (item.type)
                {
                    case Item.Types.POTION:
                        PotionInteraction pi = new PotionInteraction
                        {
                            item = item
                        };

                        pi.ProcessPotion();

                        if (pi.removeItem) removeItem = true;

                        break;
                    case Item.Types.SCROLL:
                        ScrollInteraction si = new ScrollInteraction
                        {
                            item = item
                        };

                        si.ProcessScroll();

                        if (si.removeItem) removeItem = true;

                        break;
                    case Item.Types.FOOD:
                        FoodInteraction fi = new FoodInteraction
                        {
                            item = item
                        };

                        fi.ProcessFood();

                        removeItem = true; //always remove food
                        break;
                    case Item.Types.BEVERAGE:
                        BeverageInteraction bi = new BeverageInteraction
                        {
                            item = item
                        };

                        bi.ProcessBeverage();

                        removeItem = true; //always remove beverage
                        break;
                    case Item.Types.BOOK:
                        BookInteraction book = new BookInteraction
                        {
                            item = item
                        };

                        book.ProcessBook();

                        break;
                }

                if (removeItem)
                {
                    playerInventory.Remove(ID, false);

                    menu.DrawItems(menu.selectedItemTypeIndex);
                }

            }
            else
            {
                Debug.Log("not usable in menu");
            }
        }

        void DropItem()
        {
            //prepare item to for instantiate
            GameObject obj = item.fieldPrefab;
            Vector3 newPos = new Vector3(playerObj.transform.position.x, playerObj.transform.position.y, playerObj.transform.position.z) + playerObj.transform.forward;
            Quaternion newRot = new Quaternion(0, 0, 0, 0);

            //instantiate it
            Instantiate(obj, newPos, newRot, GameObject.Find("[DROPPEDITEMS]").transform);

            //remove from inventory
            playerInventory.Remove(ID, false);

            //redraw ui
            menu.DrawItems(menu.selectedItemTypeIndex);
        }
    }

}