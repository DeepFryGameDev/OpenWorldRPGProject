using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DeepFry
{

    public class NPCItemSlotInteraction : MonoBehaviour
    {
        [ReadOnly] public bool isEquip;
        int ID;

        Text nameText;
        Text descText;
        Image icon;
        Text weightText;

        GameObject infoPanel;

        bool visible;

        BaseHero playerHero;
        PlayerInventory playerInventory;
        [HideInInspector] public NPCInventory npcInventory;
        [HideInInspector] public GameObject npcItemPrefab;
        private GameObject npcInventoryCanvas, npcInventoryList, player;

        GameMenu menu;

        private void Start()
        {
            ID = int.Parse(gameObject.name.Split(')')[0]);

            npcInventoryCanvas = GameObject.Find("GameManager/NPCInventory/NPCInventoryCanvas");
            infoPanel = GameObject.Find("GameManager/NPCInventory/NPCInventoryCanvas/NPCInventoryPanel/InfoPanel");
            npcInventoryList = GameObject.Find("GameManager/NPCInventory/NPCInventoryCanvas/NPCInventoryPanel/InventoryListPanel" +
                "/Scroll View/Viewport/Content/InventoryList");

            player = GameObject.Find("Player");

            nameText = infoPanel.transform.Find("Name").GetComponent<Text>();
            descText = infoPanel.transform.Find("Description").GetComponent<Text>();
            icon = infoPanel.transform.Find("Icon").GetComponent<Image>();
            weightText = infoPanel.transform.Find("WeightVal").GetComponent<Text>();

            playerHero = GameObject.Find("GameManager/PlayerManager").GetComponent<PlayerManager>().playersCharacter;
            playerInventory = GameObject.Find("GameManager/PlayerManager").GetComponent<PlayerInventory>();

            menu = GameObject.Find("GameManager/Menus").GetComponent<GameMenu>();
        }

        private void Update()
        {
            if (visible && Input.GetButtonDown("Confirm"))
            {
                AddItem();
            }
        }

        public void AddItem()
        {
            //add item to player inventory
            AddItemToPlayerInventory();

            //remove item from enemy inventory
            RemoveItemFromNPCInventory();

            //Reset list
            ResetInventoryUI();
        }

        void AddItemToPlayerInventory()
        {
            playerInventory.Add(ID, isEquip);
        }

        void RemoveItemFromNPCInventory()
        {
            bool removeFromList = false;

            foreach (BaseNPCItem bnpi in npcInventory.inventory)
            {
                if (bnpi.itemID == ID)
                {
                    if (bnpi.quantity <= 1)
                    {
                        removeFromList = true;
                    }
                }
            }

            if (!removeFromList) //still more items in list
            {
                foreach (BaseNPCItem bnpi in npcInventory.inventory)
                {
                    if (bnpi.itemID == ID)
                    {
                        Debug.Log("Reducing quantity of npcInventory - " + ID + " by 1");
                        bnpi.quantity -= 1;
                        break;
                    }
                }
            }
            else //took last item from list
            {
                int index = 0;

                for (int i = 0; i < npcInventory.inventory.Count; i++)
                {
                    if (npcInventory.inventory[i].itemID == ID)
                    {
                        index = i;
                        break;
                    }
                }

                npcInventory.inventory.RemoveAt(index);
            }
        }

        void ResetInventoryUI()
        {
            if (npcInventory.inventory.Count > 0) //clear and repopulate list
            {
                foreach (Transform child in npcInventoryList.transform)
                {
                    Destroy(child.gameObject);
                }

                foreach (BaseNPCItem bnpci in npcInventory.inventory)
                {
                    GameObject newItemSlot;
                    newItemSlot = Instantiate(npcItemPrefab);

                    if (bnpci.isEquip)
                    {
                        Equipment equip = EquipmentDB.instance.GetEquip(bnpci.itemID).equipment;
                        newItemSlot.name = bnpci.itemID + ") " + equip.name;
                        newItemSlot.transform.Find("BG/Icon").GetComponent<Image>().sprite = equip.icon;
                        newItemSlot.transform.Find("BG/Name").GetComponent<Text>().text = equip.name;
                    }
                    else
                    {
                        Item item = ItemDB.instance.GetItem(bnpci.itemID).item;
                        newItemSlot.name = bnpci.itemID + ") " + item.name;
                        newItemSlot.transform.Find("BG/Icon").GetComponent<Image>().sprite = item.icon;
                        newItemSlot.transform.Find("BG/Name").GetComponent<Text>().text = item.name;
                    }

                    newItemSlot.transform.Find("BG/Count").GetComponent<Text>().text = bnpci.quantity.ToString();

                    newItemSlot.GetComponent<NPCItemSlotInteraction>().npcInventory = npcInventory;
                    newItemSlot.GetComponent<NPCItemSlotInteraction>().npcItemPrefab = npcItemPrefab;
                    newItemSlot.GetComponent<NPCItemSlotInteraction>().isEquip = bnpci.isEquip;

                    newItemSlot.transform.SetParent(npcInventoryList.transform, false);
                }
            }
            else //hide window
            {
                HideWindow();
            }
        }

        void HideWindow()
        {
            HideCanvas();

            menu.CursorActive(false);

            //lock camera rotation
            player.GetComponent<SmartFPController.FirstPersonController>().enabled = true;

            //allow user to enter menu again
            menu.inNpcInventory = false;
        }

        public void OnCursorEnter()
        {
            visible = true;
            ShowItemDetails();
        }

        public void OnCursorExit()
        {
            visible = false;
            HideItemDetails();
        }

        void ShowItemDetails()
        {
            if (isEquip)
            {
                nameText.text = EquipmentDB.instance.GetEquip(ID).equipment.name;
                descText.text = EquipmentDB.instance.GetEquip(ID).equipment.description;
                weightText.text = EquipmentDB.instance.GetEquip(ID).equipment.weight.ToString();
                icon.sprite = EquipmentDB.instance.GetEquip(ID).equipment.icon;
            }
            else
            {
                nameText.text = ItemDB.instance.GetItem(ID).item.name;
                descText.text = ItemDB.instance.GetItem(ID).item.description;
                weightText.text = ItemDB.instance.GetItem(ID).item.weight.ToString();
                icon.sprite = ItemDB.instance.GetItem(ID).item.icon;
            }


            infoPanel.GetComponent<CanvasGroup>().alpha = 1.0f;
        }

        void HideItemDetails()
        {
            infoPanel.GetComponent<CanvasGroup>().alpha = 0.0f;

            nameText.text = "";
            descText.text = "";
            weightText.text = "";
            icon.sprite = null;
        }

        void HideCanvas()
        {
            npcInventoryCanvas.GetComponent<CanvasGroup>().alpha = 0;
            npcInventoryCanvas.GetComponent<CanvasGroup>().interactable = false;
            npcInventoryCanvas.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }


    }

}