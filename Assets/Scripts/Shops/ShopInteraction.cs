using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DeepFry
{

    public class ShopInteraction : MonoBehaviour
    {
        [ReadOnly] public bool open;
        [ReadOnly] public bool inQuantitySelect;
        [ReadOnly] public GameObject merchantObject;
        [ReadOnly] public bool buyMode;
        [ReadOnly] public int selectedItemTypeIndex;

        GameMenu menu;

        GameObject shopListObj;

        GameObject player;
        PlayerInventory pi;
        ShopInventory mi;

        PixelCrushers.DialogueSystem.Selector selector;

        GameObject quantityCanvas, itemTypePanel;
        InputField quantityIF;
        Text costText, newGoldText, goldText, merchantGoldText;

        [HideInInspector] public int vendorsQuantityOfItem;
        [HideInInspector] public int tempID;
        [HideInInspector] public bool tempIsEquip;
        [HideInInspector] public int tempCost;
        [HideInInspector] public int merchantGold;

        public int quantityThreshold;
        public int prefabLayer = 13;

        public float quantityOffsetX;
        public float quantityOffsetY;
        public int inactiveButtonFontSize, activeButtonFontSize;

        private void Start()
        {
            menu = GameObject.Find("Menus").GetComponent<GameMenu>();
            shopListObj = GameObject.Find("ShopCanvas/ShopBG/InventoryPanel/Scroll View/Viewport/InventoryList");

            player = GameObject.Find("Player");
            pi = GameObject.Find("PlayerManager").GetComponent<PlayerInventory>();

            selector = player.GetComponent<PixelCrushers.DialogueSystem.Selector>();
            quantityCanvas = GameObject.Find("ShopCanvas/QuantityCanvas");
            quantityIF = quantityCanvas.transform.Find("QuantityPanel/InputField").GetComponent<InputField>();
            costText = quantityCanvas.transform.Find("QuantityPanel/TotalCostText").GetComponent<Text>();
            newGoldText = quantityCanvas.transform.Find("QuantityPanel/NewGoldText").GetComponent<Text>();
            goldText = GameObject.Find("ShopCanvas/ShopBG/GoldPanel/GoldText").GetComponent<Text>();
            merchantGoldText = GameObject.Find("ShopCanvas/ShopBG/GoldPanel/NPCGoldText").GetComponent<Text>();

            itemTypePanel = GameObject.Find("ShopCanvas/ShopBG/ItemTypePanel");
        }

        // Update is called once per frame
        void Update()
        {
            if (open && Input.GetButtonDown("Cancel") && !inQuantitySelect)
            {
                CloseShopUI();
            }

            if (open && Input.GetButtonDown("Cancel") && inQuantitySelect)
            {
                CloseQuantitySelect();
            }

            if (open && inQuantitySelect && Input.GetButtonDown("Confirm") && quantityIF.text.Length > 0)
            {
                ProcessMultiple();
            }

            if (open && inQuantitySelect && Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                ScrollUp();

            }
            else if (open && inQuantitySelect && Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                ScrollDown();
            }

            if (open && inQuantitySelect && quantityIF.text.Length > 0)
            {
                if (buyMode)
                {
                    quantityIF.text = MaxQuantityToBuy().ToString();
                    costText.text = (MaxQuantityToBuy() * tempCost).ToString();
                }
                else
                {
                    quantityIF.text = MaxQuantityToSell().ToString();
                    costText.text = (MaxQuantityToSell() * tempCost).ToString();
                }

                if (buyMode)
                {
                    newGoldText.text = (pi.gold - int.Parse(costText.text)).ToString();
                }
                else
                {
                    newGoldText.text = (merchantGold - int.Parse(costText.text)).ToString();
                }
            }
            else if (open && inQuantitySelect && quantityIF.text.Length == 0)
            {
                costText.text = "00";
                if (buyMode)
                {
                    newGoldText.text = pi.gold.ToString();
                }
                else
                {
                    newGoldText.text = merchantGold.ToString();
                }
            }

            if (int.Parse(newGoldText.text) < 0)
            {
                newGoldText.text = "00";
            }
        }

        public void AnimateText(int index, GameObject textObject)
        {
            switch (index)
            {
                case 0: //turns dark red for a moment, then returns
                    textObject.GetComponent<Animator>().SetTrigger("CantAction");
                    break;
            }
        }

        void ScrollDown()
        {
            Debug.Log("Down: " + quantityIF.text.Length);
            if (quantityIF.text.Length > 0 && (int.Parse(quantityIF.text) >= 2))
            {
                //Debug.Log("Down: " + quantityIF.text);
                quantityIF.text = (int.Parse(quantityIF.text) - 1).ToString();
            }
        }

        void ScrollUp()
        {
            Debug.Log("Up: " + quantityIF.text.Length);
            if (quantityIF.text.Length > 0)
            {
                //Debug.Log("Up: " + quantityIF.text);
                quantityIF.text = (int.Parse(quantityIF.text) + 1).ToString();
            }
        }

        void HighlightButton(int index)
        {
            //unhighlight all buttons
            foreach (Transform child in itemTypePanel.transform)
            {
                ColorBlock dimColor = child.GetComponent<Button>().colors;

                dimColor.normalColor = new Color(dimColor.disabledColor.r, dimColor.disabledColor.g, dimColor.disabledColor.b, .5f);
                child.GetComponent<Button>().colors = dimColor;

            }

            //highlight just the 1
            ColorBlock highlightColor = itemTypePanel.transform.GetChild(index).GetComponent<Button>().colors;

            highlightColor.normalColor = new Color(highlightColor.disabledColor.r, highlightColor.disabledColor.g, highlightColor.disabledColor.b, 1f);

            itemTypePanel.transform.GetChild(index).GetComponent<Button>().colors = highlightColor;
        }

        public void DrawItems(int index)
        {
            //set new index
            selectedItemTypeIndex = index;

            //highlight button
            HighlightButton(index);

            //erase old list
            //clears list
            foreach (Transform child in shopListObj.transform)
            {
                Destroy(child.gameObject);
            }

            //generate new list
            switch (index)
            {
                case 0:
                    DrawFavorites();
                    break;
                case 1:
                    DrawAllItems();
                    break;
                case 2:
                    DrawWeapons();
                    break;
                case 3:
                    DrawArmor();
                    break;
                case 4:
                    DrawPotions();
                    break;
                case 5:
                    DrawFood();
                    break;
                case 6:
                    DrawScrolls();
                    break;
                case 7:
                    DrawBooks();
                    break;
                case 8:
                    DrawIngredients();
                    break;
            }

        }

        void DrawAllItems()
        {
            if (buyMode) //build list from vendor items
            {
                foreach (BaseShopItem bsi in merchantObject.GetComponent<ShopInventory>().itemsForSale)
                {
                    GameObject itemSlot = Instantiate(Resources.Load<GameObject>("Prefabs/UI/ShopItemSlot"));

                    itemSlot.name = bsi.itemID + ") " + ItemDB.instance.GetItem(bsi.itemID).item.name;
                    itemSlot.transform.Find("Icon").GetComponent<Image>().sprite = ItemDB.instance.GetItem(bsi.itemID).item.icon;
                    itemSlot.transform.Find("Name").GetComponent<Text>().text = ItemDB.instance.GetItem(bsi.itemID).item.name +
                        " (" + bsi.count + ")";
                    itemSlot.transform.Find("Cost").GetComponent<Text>().text = bsi.cost.ToString();

                    itemSlot.GetComponent<ItemShopSlotInteraction>().ID = bsi.itemID;
                    itemSlot.GetComponent<ItemShopSlotInteraction>().item = ItemDB.instance.GetItem(bsi.itemID).item;
                    itemSlot.GetComponent<ItemShopSlotInteraction>().count = bsi.count;
                    itemSlot.GetComponent<ItemShopSlotInteraction>().cost = bsi.cost;

                    itemSlot.GetComponent<ItemShopSlotInteraction>().buyMode = true;

                    itemSlot.transform.SetParent(shopListObj.transform, false);
                }

                foreach (BaseShopEquipment bse in merchantObject.GetComponent<ShopInventory>().equipmentForSale)
                {
                    GameObject itemSlot = Instantiate(Resources.Load<GameObject>("Prefabs/UI/ShopEquipSlot"));

                    itemSlot.name = bse.itemID + ") " + EquipmentDB.instance.GetEquip(bse.itemID).equipment.name;
                    itemSlot.transform.Find("Icon").GetComponent<Image>().sprite = EquipmentDB.instance.GetEquip(bse.itemID).equipment.icon;
                    itemSlot.transform.Find("Name").GetComponent<Text>().text = EquipmentDB.instance.GetEquip(bse.itemID).equipment.name +
                        " (" + bse.count + ")";
                    itemSlot.transform.Find("Cost").GetComponent<Text>().text = bse.cost.ToString();

                    itemSlot.GetComponent<EquipShopSlotInteraction>().ID = bse.itemID;
                    itemSlot.GetComponent<EquipShopSlotInteraction>().equip = EquipmentDB.instance.GetEquip(bse.itemID).equipment;
                    itemSlot.GetComponent<EquipShopSlotInteraction>().count = bse.count;
                    itemSlot.GetComponent<EquipShopSlotInteraction>().cost = bse.cost;

                    itemSlot.GetComponent<EquipShopSlotInteraction>().buyMode = true;

                    itemSlot.transform.SetParent(shopListObj.transform, false);
                }
            }
            else //build items from player inventory
            {
                //building new list
                List<Item> itemsAccountedFor = new List<Item>();

                //for each item in player inventory
                for (int i = 0; i <= pi.inventory.Count - 1; i++)
                {
                    if (!pi.inventory[i].isEquip && pi.inventory[i].itemID != 0) //if item and not gold
                    {
                        Item item = ItemDB.instance.GetItem(pi.inventory[i].itemID).item;
                        if (!itemsAccountedFor.Contains(item) && item.type != Item.Types.QUEST)
                        {
                            int count = 0;
                            foreach (BasePlayerItem bpi in pi.inventory)
                            {
                                if (bpi.itemID == pi.inventory[i].itemID && !bpi.isEquip)
                                {
                                    count++;
                                }
                            }

                            GameObject itemSlot = Instantiate(Resources.Load<GameObject>("Prefabs/UI/ShopItemSlot"));

                            itemSlot.name = pi.inventory[i].itemID + ") " + item.name;
                            itemSlot.transform.Find("Icon").GetComponent<Image>().sprite = item.icon;
                            itemSlot.transform.Find("Name").GetComponent<Text>().text = item.name + " (" + count + ")";
                            itemSlot.transform.Find("Cost").GetComponent<Text>().text = item.sellValue.ToString();

                            itemSlot.GetComponent<ItemShopSlotInteraction>().ID = pi.inventory[i].itemID;
                            itemSlot.GetComponent<ItemShopSlotInteraction>().item = item;
                            itemSlot.GetComponent<ItemShopSlotInteraction>().count = count;
                            itemSlot.GetComponent<ItemShopSlotInteraction>().cost = item.sellValue;

                            itemSlot.transform.SetParent(shopListObj.transform, false);
                        }

                        itemsAccountedFor.Add(item);
                    }
                    else //if equip
                    {
                        Equipment equip = EquipmentDB.instance.GetEquip(pi.inventory[i].itemID).equipment;

                        GameObject itemSlot = Instantiate(Resources.Load<GameObject>("Prefabs/UI/ShopEquipSlot"));

                        int count = 0;
                        foreach (BasePlayerItem bpi in pi.inventory)
                        {
                            if (bpi.itemID == pi.inventory[i].itemID && bpi.isEquip)
                            {
                                count++;
                            }
                        }

                        itemSlot.name = pi.inventory[i].itemID + ") " + equip.name;
                        itemSlot.transform.Find("Icon").GetComponent<Image>().sprite = equip.icon;
                        itemSlot.transform.Find("Name").GetComponent<Text>().text = equip.name + " (" + count + ")";
                        itemSlot.transform.Find("Cost").GetComponent<Text>().text = equip.sellValue.ToString();

                        itemSlot.GetComponent<EquipShopSlotInteraction>().ID = pi.inventory[i].itemID;
                        itemSlot.GetComponent<EquipShopSlotInteraction>().equip = equip;
                        itemSlot.GetComponent<EquipShopSlotInteraction>().count = count;
                        itemSlot.GetComponent<EquipShopSlotInteraction>().cost = equip.sellValue;

                        itemSlot.transform.SetParent(shopListObj.transform, false);

                        itemsAccountedFor.Add(equip);
                    }
                }
            }
        }

        void DrawFavorites()
        {
            if (buyMode)
            {
                foreach (BaseShopItem bsi in merchantObject.GetComponent<ShopInventory>().itemsForSale)
                {
                    if (ItemDB.instance.GetItem(bsi.itemID).item.isFavorite)
                    {
                        GameObject itemSlot = Instantiate(Resources.Load<GameObject>("Prefabs/UI/ShopItemSlot"));

                        itemSlot.name = bsi.itemID + ") " + ItemDB.instance.GetItem(bsi.itemID).item.name;
                        itemSlot.transform.Find("Icon").GetComponent<Image>().sprite = ItemDB.instance.GetItem(bsi.itemID).item.icon;
                        itemSlot.transform.Find("Name").GetComponent<Text>().text = ItemDB.instance.GetItem(bsi.itemID).item.name +
                            " (" + bsi.count + ")";
                        itemSlot.transform.Find("Cost").GetComponent<Text>().text = bsi.cost.ToString();

                        itemSlot.GetComponent<ItemShopSlotInteraction>().ID = bsi.itemID;
                        itemSlot.GetComponent<ItemShopSlotInteraction>().item = ItemDB.instance.GetItem(bsi.itemID).item;
                        itemSlot.GetComponent<ItemShopSlotInteraction>().count = bsi.count;
                        itemSlot.GetComponent<ItemShopSlotInteraction>().cost = bsi.cost;

                        itemSlot.GetComponent<ItemShopSlotInteraction>().buyMode = true;

                        itemSlot.transform.SetParent(shopListObj.transform, false);
                    }
                }

                foreach (BaseShopEquipment bse in merchantObject.GetComponent<ShopInventory>().equipmentForSale)
                {
                    if (EquipmentDB.instance.GetEquip(bse.itemID).equipment.isFavorite)
                    {
                        GameObject itemSlot = Instantiate(Resources.Load<GameObject>("Prefabs/UI/ShopEquipSlot"));

                        itemSlot.name = bse.itemID + ") " + EquipmentDB.instance.GetEquip(bse.itemID).equipment.name;
                        itemSlot.transform.Find("Icon").GetComponent<Image>().sprite = EquipmentDB.instance.GetEquip(bse.itemID).equipment.icon;
                        itemSlot.transform.Find("Name").GetComponent<Text>().text = EquipmentDB.instance.GetEquip(bse.itemID).equipment.name +
                            " (" + bse.count + ")";
                        itemSlot.transform.Find("Cost").GetComponent<Text>().text = bse.cost.ToString();

                        itemSlot.GetComponent<EquipShopSlotInteraction>().ID = bse.itemID;
                        itemSlot.GetComponent<EquipShopSlotInteraction>().equip = EquipmentDB.instance.GetEquip(bse.itemID).equipment;
                        itemSlot.GetComponent<EquipShopSlotInteraction>().count = bse.count;
                        itemSlot.GetComponent<EquipShopSlotInteraction>().cost = bse.cost;

                        itemSlot.GetComponent<EquipShopSlotInteraction>().buyMode = true;

                        itemSlot.transform.SetParent(shopListObj.transform, false);
                    }
                }
            }
            else
            {
                //building new list
                List<Item> itemsAccountedFor = new List<Item>();

                //for each item in player inventory
                for (int i = 0; i <= pi.inventory.Count - 1; i++)
                {
                    if (!pi.inventory[i].isEquip && pi.inventory[i].itemID != 0 && ItemDB.instance.GetItem(pi.inventory[i].itemID).item.isFavorite) //if item and not gold
                    {
                        Item item = ItemDB.instance.GetItem(pi.inventory[i].itemID).item;
                        if (!itemsAccountedFor.Contains(item))
                        {
                            int count = 0;
                            foreach (BasePlayerItem bpi in pi.inventory)
                            {
                                if (bpi.itemID == pi.inventory[i].itemID && !bpi.isEquip)
                                {
                                    count++;
                                }
                            }

                            GameObject itemSlot = Instantiate(Resources.Load<GameObject>("Prefabs/UI/ShopItemSlot"));

                            itemSlot.name = pi.inventory[i].itemID + ") " + item.name;
                            itemSlot.transform.Find("Icon").GetComponent<Image>().sprite = item.icon;
                            itemSlot.transform.Find("Name").GetComponent<Text>().text = item.name + " (" + count + ")";
                            itemSlot.transform.Find("Cost").GetComponent<Text>().text = item.sellValue.ToString();

                            itemSlot.GetComponent<ItemShopSlotInteraction>().ID = pi.inventory[i].itemID;
                            itemSlot.GetComponent<ItemShopSlotInteraction>().item = item;
                            itemSlot.GetComponent<ItemShopSlotInteraction>().count = count;
                            itemSlot.GetComponent<ItemShopSlotInteraction>().cost = item.sellValue;

                            itemSlot.transform.SetParent(shopListObj.transform, false);
                        }

                        itemsAccountedFor.Add(item);
                    }
                    else if (!pi.inventory[i].isEquip && pi.inventory[i].itemID != 0 && EquipmentDB.instance.GetEquip(pi.inventory[i].itemID).equipment.isFavorite) //if equip
                    {
                        Equipment equip = EquipmentDB.instance.GetEquip(pi.inventory[i].itemID).equipment;

                        int count = 0;
                        foreach (BasePlayerItem bpi in pi.inventory)
                        {
                            if (bpi.itemID == pi.inventory[i].itemID && bpi.isEquip)
                            {
                                count++;
                            }
                        }

                        GameObject itemSlot = Instantiate(Resources.Load<GameObject>("Prefabs/UI/ShopEquipSlot"));

                        itemSlot.name = pi.inventory[i].itemID + ") " + equip.name;
                        itemSlot.transform.Find("Icon").GetComponent<Image>().sprite = equip.icon;
                        itemSlot.transform.Find("Name").GetComponent<Text>().text = equip.name + " (" + count + ")";
                        itemSlot.transform.Find("Cost").GetComponent<Text>().text = equip.sellValue.ToString();

                        itemSlot.GetComponent<EquipShopSlotInteraction>().ID = pi.inventory[i].itemID;
                        itemSlot.GetComponent<EquipShopSlotInteraction>().equip = equip;
                        itemSlot.GetComponent<EquipShopSlotInteraction>().count = count;
                        itemSlot.GetComponent<EquipShopSlotInteraction>().cost = equip.sellValue;

                        itemSlot.transform.SetParent(shopListObj.transform, false);

                        itemsAccountedFor.Add(equip);
                    }
                }
            }
        }

        void DrawWeapons()
        {
            if (buyMode)
            {
                foreach (BaseShopEquipment bse in merchantObject.GetComponent<ShopInventory>().equipmentForSale)
                {
                    if (EquipmentDB.instance.GetEquip(bse.itemID).equipment.type == Item.Types.WEAPON) //if weapon
                    {
                        GameObject itemSlot = Instantiate(Resources.Load<GameObject>("Prefabs/UI/ShopEquipSlot"));

                        itemSlot.name = bse.itemID + ") " + EquipmentDB.instance.GetEquip(bse.itemID).equipment.name;
                        itemSlot.transform.Find("Icon").GetComponent<Image>().sprite = EquipmentDB.instance.GetEquip(bse.itemID).equipment.icon;
                        itemSlot.transform.Find("Name").GetComponent<Text>().text = EquipmentDB.instance.GetEquip(bse.itemID).equipment.name +
                            " (" + bse.count + ")";
                        itemSlot.transform.Find("Cost").GetComponent<Text>().text = bse.cost.ToString();

                        itemSlot.GetComponent<EquipShopSlotInteraction>().ID = bse.itemID;
                        itemSlot.GetComponent<EquipShopSlotInteraction>().equip = EquipmentDB.instance.GetEquip(bse.itemID).equipment;
                        itemSlot.GetComponent<EquipShopSlotInteraction>().count = bse.count;
                        itemSlot.GetComponent<EquipShopSlotInteraction>().cost = bse.cost;

                        itemSlot.GetComponent<EquipShopSlotInteraction>().buyMode = true;

                        itemSlot.transform.SetParent(shopListObj.transform, false);
                    }
                }
            }
            else
            {
                //building new list
                List<Item> itemsAccountedFor = new List<Item>();

                //for each item in player inventory
                for (int i = 0; i <= pi.inventory.Count - 1; i++)
                {
                    //if equip
                    if (pi.inventory[i].isEquip &&
                        EquipmentDB.instance.GetEquip(pi.inventory[i].itemID).equipment.type == Item.Types.WEAPON) //if equip and weapon
                    {
                        Equipment equip = EquipmentDB.instance.GetEquip(pi.inventory[i].itemID).equipment;

                        GameObject itemSlot = Instantiate(Resources.Load<GameObject>("Prefabs/UI/ShopEquipSlot"));

                        int count = 0;
                        foreach (BasePlayerItem bpi in pi.inventory)
                        {
                            if (bpi.itemID == pi.inventory[i].itemID && bpi.isEquip)
                            {
                                count++;
                            }
                        }

                        itemSlot.name = pi.inventory[i].itemID + ") " + equip.name;
                        itemSlot.transform.Find("Icon").GetComponent<Image>().sprite = equip.icon;
                        itemSlot.transform.Find("Name").GetComponent<Text>().text = equip.name + " (" + count + ")";
                        itemSlot.transform.Find("Cost").GetComponent<Text>().text = equip.sellValue.ToString();

                        itemSlot.GetComponent<EquipShopSlotInteraction>().ID = pi.inventory[i].itemID;
                        itemSlot.GetComponent<EquipShopSlotInteraction>().equip = equip;
                        itemSlot.GetComponent<EquipShopSlotInteraction>().count = count;
                        itemSlot.GetComponent<EquipShopSlotInteraction>().cost = equip.sellValue;

                        itemsAccountedFor.Add(equip);

                        itemSlot.transform.SetParent(shopListObj.transform, false);
                    }
                }
            }

        }

        void DrawArmor()
        {
            if (buyMode)
            {
                foreach (BaseShopEquipment bse in merchantObject.GetComponent<ShopInventory>().equipmentForSale)
                {
                    if (EquipmentDB.instance.GetEquip(bse.itemID).equipment.type == Item.Types.ARMOR ||
                        EquipmentDB.instance.GetEquip(bse.itemID).equipment.type == Item.Types.CLOTHING) //if armor
                    {
                        GameObject itemSlot = Instantiate(Resources.Load<GameObject>("Prefabs/UI/ShopEquipSlot"));

                        itemSlot.name = bse.itemID + ") " + EquipmentDB.instance.GetEquip(bse.itemID).equipment.name;
                        itemSlot.transform.Find("Icon").GetComponent<Image>().sprite = EquipmentDB.instance.GetEquip(bse.itemID).equipment.icon;
                        itemSlot.transform.Find("Name").GetComponent<Text>().text = EquipmentDB.instance.GetEquip(bse.itemID).equipment.name +
                            " (" + bse.count + ")";
                        itemSlot.transform.Find("Cost").GetComponent<Text>().text = bse.cost.ToString();

                        itemSlot.GetComponent<EquipShopSlotInteraction>().ID = bse.itemID;
                        itemSlot.GetComponent<EquipShopSlotInteraction>().equip = EquipmentDB.instance.GetEquip(bse.itemID).equipment;
                        itemSlot.GetComponent<EquipShopSlotInteraction>().count = bse.count;
                        itemSlot.GetComponent<EquipShopSlotInteraction>().cost = bse.cost;

                        itemSlot.GetComponent<EquipShopSlotInteraction>().buyMode = true;

                        itemSlot.transform.SetParent(shopListObj.transform, false);
                    }
                }
            }
            else
            {
                //building new list
                List<Item> itemsAccountedFor = new List<Item>();

                //for each item in player inventory
                for (int i = 0; i <= pi.inventory.Count - 1; i++)
                {
                    //if equip
                    if (pi.inventory[i].isEquip &&
                        (EquipmentDB.instance.GetEquip(pi.inventory[i].itemID).equipment.type == Item.Types.ARMOR ||
                        EquipmentDB.instance.GetEquip(pi.inventory[i].itemID).equipment.type == Item.Types.CLOTHING)) //if equip and armor
                    {
                        Equipment equip = EquipmentDB.instance.GetEquip(pi.inventory[i].itemID).equipment;

                        int count = 0;
                        foreach (BasePlayerItem bpi in pi.inventory)
                        {
                            if (bpi.itemID == pi.inventory[i].itemID && bpi.isEquip)
                            {
                                count++;
                            }
                        }

                        GameObject itemSlot = Instantiate(Resources.Load<GameObject>("Prefabs/UI/ShopEquipSlot"));

                        itemSlot.name = pi.inventory[i].itemID + ") " + equip.name;
                        itemSlot.transform.Find("Icon").GetComponent<Image>().sprite = equip.icon;
                        itemSlot.transform.Find("Name").GetComponent<Text>().text = equip.name + " (" + count + ")";
                        itemSlot.transform.Find("Cost").GetComponent<Text>().text = equip.sellValue.ToString();

                        itemSlot.GetComponent<EquipShopSlotInteraction>().ID = pi.inventory[i].itemID;
                        itemSlot.GetComponent<EquipShopSlotInteraction>().equip = equip;
                        itemSlot.GetComponent<EquipShopSlotInteraction>().count = count;
                        itemSlot.GetComponent<EquipShopSlotInteraction>().cost = equip.sellValue;

                        itemsAccountedFor.Add(equip);

                        itemSlot.transform.SetParent(shopListObj.transform, false);

                    }
                }
            }
        }

        void DrawPotions()
        {
            if (buyMode)
            {
                foreach (BaseShopItem bsi in merchantObject.GetComponent<ShopInventory>().itemsForSale)
                {
                    if (ItemDB.instance.GetItem(bsi.itemID).item.type == Item.Types.POTION)
                    {
                        GameObject itemSlot = Instantiate(Resources.Load<GameObject>("Prefabs/UI/ShopItemSlot"));

                        itemSlot.name = bsi.itemID + ") " + ItemDB.instance.GetItem(bsi.itemID).item.name;
                        itemSlot.transform.Find("Icon").GetComponent<Image>().sprite = ItemDB.instance.GetItem(bsi.itemID).item.icon;
                        itemSlot.transform.Find("Name").GetComponent<Text>().text = ItemDB.instance.GetItem(bsi.itemID).item.name +
                            " (" + bsi.count + ")";
                        itemSlot.transform.Find("Cost").GetComponent<Text>().text = bsi.cost.ToString();

                        itemSlot.GetComponent<ItemShopSlotInteraction>().ID = bsi.itemID;
                        itemSlot.GetComponent<ItemShopSlotInteraction>().item = ItemDB.instance.GetItem(bsi.itemID).item;
                        itemSlot.GetComponent<ItemShopSlotInteraction>().count = bsi.count;
                        itemSlot.GetComponent<ItemShopSlotInteraction>().cost = bsi.cost;

                        itemSlot.GetComponent<ItemShopSlotInteraction>().buyMode = true;

                        itemSlot.transform.SetParent(shopListObj.transform, false);
                    }
                }
            }
            else
            {
                //building new list
                List<Item> itemsAccountedFor = new List<Item>();

                //for each item in player inventory
                for (int i = 0; i <= pi.inventory.Count - 1; i++)
                {
                    if (!pi.inventory[i].isEquip && pi.inventory[i].itemID != 0 &&
                        ItemDB.instance.GetItem(pi.inventory[i].itemID).item.type == Item.Types.POTION) //if item and not gold
                    {
                        Item item = ItemDB.instance.GetItem(pi.inventory[i].itemID).item;
                        if (!itemsAccountedFor.Contains(item))
                        {
                            int count = 0;
                            foreach (BasePlayerItem bpi in pi.inventory)
                            {
                                if (bpi.itemID == pi.inventory[i].itemID && !bpi.isEquip)
                                {
                                    count++;
                                }
                            }

                            GameObject itemSlot = Instantiate(Resources.Load<GameObject>("Prefabs/UI/ShopItemSlot"));

                            itemSlot.name = pi.inventory[i].itemID + ") " + item.name;
                            itemSlot.transform.Find("Icon").GetComponent<Image>().sprite = item.icon;
                            itemSlot.transform.Find("Name").GetComponent<Text>().text = item.name + " (" + count + ")";
                            itemSlot.transform.Find("Cost").GetComponent<Text>().text = item.sellValue.ToString();

                            itemSlot.GetComponent<ItemShopSlotInteraction>().ID = pi.inventory[i].itemID;
                            itemSlot.GetComponent<ItemShopSlotInteraction>().item = item;
                            itemSlot.GetComponent<ItemShopSlotInteraction>().count = count;
                            itemSlot.GetComponent<ItemShopSlotInteraction>().cost = item.sellValue;

                            itemSlot.transform.SetParent(shopListObj.transform, false);
                        }

                        itemsAccountedFor.Add(item);
                    }
                }
            }
        }

        void DrawFood()
        {
            if (buyMode)
            {
                foreach (BaseShopItem bsi in merchantObject.GetComponent<ShopInventory>().itemsForSale)
                {
                    if (ItemDB.instance.GetItem(bsi.itemID).item.type == Item.Types.FOOD ||
                        ItemDB.instance.GetItem(bsi.itemID).item.type == Item.Types.BEVERAGE)
                    {
                        GameObject itemSlot = Instantiate(Resources.Load<GameObject>("Prefabs/UI/ShopItemSlot"));

                        itemSlot.name = bsi.itemID + ") " + ItemDB.instance.GetItem(bsi.itemID).item.name;
                        itemSlot.transform.Find("Icon").GetComponent<Image>().sprite = ItemDB.instance.GetItem(bsi.itemID).item.icon;
                        itemSlot.transform.Find("Name").GetComponent<Text>().text = ItemDB.instance.GetItem(bsi.itemID).item.name +
                            " (" + bsi.count + ")";
                        itemSlot.transform.Find("Cost").GetComponent<Text>().text = bsi.cost.ToString();

                        itemSlot.GetComponent<ItemShopSlotInteraction>().ID = bsi.itemID;
                        itemSlot.GetComponent<ItemShopSlotInteraction>().item = ItemDB.instance.GetItem(bsi.itemID).item;
                        itemSlot.GetComponent<ItemShopSlotInteraction>().count = bsi.count;
                        itemSlot.GetComponent<ItemShopSlotInteraction>().cost = bsi.cost;

                        itemSlot.GetComponent<ItemShopSlotInteraction>().buyMode = true;

                        itemSlot.transform.SetParent(shopListObj.transform, false);
                    }
                }
            }
            else
            {
                //building new list
                List<Item> itemsAccountedFor = new List<Item>();

                //for each item in player inventory
                for (int i = 0; i <= pi.inventory.Count - 1; i++)
                {
                    if (!pi.inventory[i].isEquip && pi.inventory[i].itemID != 0 &&
                        (ItemDB.instance.GetItem(pi.inventory[i].itemID).item.type == Item.Types.FOOD ||
                        ItemDB.instance.GetItem(pi.inventory[i].itemID).item.type == Item.Types.BEVERAGE)) //if item and not gold
                    {
                        Item item = ItemDB.instance.GetItem(pi.inventory[i].itemID).item;
                        if (!itemsAccountedFor.Contains(item))
                        {
                            int count = 0;
                            foreach (BasePlayerItem bpi in pi.inventory)
                            {
                                if (bpi.itemID == pi.inventory[i].itemID && !bpi.isEquip)
                                {
                                    count++;
                                }
                            }

                            GameObject itemSlot = Instantiate(Resources.Load<GameObject>("Prefabs/UI/ShopItemSlot"));

                            itemSlot.name = pi.inventory[i].itemID + ") " + item.name;
                            itemSlot.transform.Find("Icon").GetComponent<Image>().sprite = item.icon;
                            itemSlot.transform.Find("Name").GetComponent<Text>().text = item.name + " (" + count + ")";
                            itemSlot.transform.Find("Cost").GetComponent<Text>().text = item.sellValue.ToString();

                            itemSlot.GetComponent<ItemShopSlotInteraction>().ID = pi.inventory[i].itemID;
                            itemSlot.GetComponent<ItemShopSlotInteraction>().item = item;
                            itemSlot.GetComponent<ItemShopSlotInteraction>().count = count;
                            itemSlot.GetComponent<ItemShopSlotInteraction>().cost = item.sellValue;

                            itemSlot.transform.SetParent(shopListObj.transform, false);
                        }

                        itemsAccountedFor.Add(item);
                    }
                }
            }
        }

        void DrawScrolls()
        {
            if (buyMode)
            {
                foreach (BaseShopItem bsi in merchantObject.GetComponent<ShopInventory>().itemsForSale)
                {
                    if (ItemDB.instance.GetItem(bsi.itemID).item.type == Item.Types.SCROLL)
                    {
                        GameObject itemSlot = Instantiate(Resources.Load<GameObject>("Prefabs/UI/ShopItemSlot"));

                        itemSlot.name = bsi.itemID + ") " + ItemDB.instance.GetItem(bsi.itemID).item.name;
                        itemSlot.transform.Find("Icon").GetComponent<Image>().sprite = ItemDB.instance.GetItem(bsi.itemID).item.icon;
                        itemSlot.transform.Find("Name").GetComponent<Text>().text = ItemDB.instance.GetItem(bsi.itemID).item.name +
                            " (" + bsi.count + ")";
                        itemSlot.transform.Find("Cost").GetComponent<Text>().text = bsi.cost.ToString();

                        itemSlot.GetComponent<ItemShopSlotInteraction>().ID = bsi.itemID;
                        itemSlot.GetComponent<ItemShopSlotInteraction>().item = ItemDB.instance.GetItem(bsi.itemID).item;
                        itemSlot.GetComponent<ItemShopSlotInteraction>().count = bsi.count;
                        itemSlot.GetComponent<ItemShopSlotInteraction>().cost = bsi.cost;

                        itemSlot.GetComponent<ItemShopSlotInteraction>().buyMode = true;

                        itemSlot.transform.SetParent(shopListObj.transform, false);
                    }
                }
            }
            else
            {
                //building new list
                List<Item> itemsAccountedFor = new List<Item>();

                //for each item in player inventory
                for (int i = 0; i <= pi.inventory.Count - 1; i++)
                {
                    if (!pi.inventory[i].isEquip && pi.inventory[i].itemID != 0 &&
                        ItemDB.instance.GetItem(pi.inventory[i].itemID).item.type == Item.Types.SCROLL) //if item and not gold
                    {
                        Item item = ItemDB.instance.GetItem(pi.inventory[i].itemID).item;
                        if (!itemsAccountedFor.Contains(item))
                        {
                            int count = 0;
                            foreach (BasePlayerItem bpi in pi.inventory)
                            {
                                if (bpi.itemID == pi.inventory[i].itemID && !bpi.isEquip)
                                {
                                    count++;
                                }
                            }

                            GameObject itemSlot = Instantiate(Resources.Load<GameObject>("Prefabs/UI/ShopItemSlot"));

                            itemSlot.name = pi.inventory[i].itemID + ") " + item.name;
                            itemSlot.transform.Find("Icon").GetComponent<Image>().sprite = item.icon;
                            itemSlot.transform.Find("Name").GetComponent<Text>().text = item.name + " (" + count + ")";
                            itemSlot.transform.Find("Cost").GetComponent<Text>().text = item.sellValue.ToString();

                            itemSlot.GetComponent<ItemShopSlotInteraction>().ID = pi.inventory[i].itemID;
                            itemSlot.GetComponent<ItemShopSlotInteraction>().item = item;
                            itemSlot.GetComponent<ItemShopSlotInteraction>().count = count;
                            itemSlot.GetComponent<ItemShopSlotInteraction>().cost = item.sellValue;

                            itemSlot.transform.SetParent(shopListObj.transform, false);
                        }

                        itemsAccountedFor.Add(item);
                    }
                }
            }
        }

        void DrawBooks()
        {
            if (buyMode)
            {
                foreach (BaseShopItem bsi in merchantObject.GetComponent<ShopInventory>().itemsForSale)
                {
                    if (ItemDB.instance.GetItem(bsi.itemID).item.type == Item.Types.BOOK)
                    {
                        GameObject itemSlot = Instantiate(Resources.Load<GameObject>("Prefabs/UI/ShopItemSlot"));

                        itemSlot.name = bsi.itemID + ") " + ItemDB.instance.GetItem(bsi.itemID).item.name;
                        itemSlot.transform.Find("Icon").GetComponent<Image>().sprite = ItemDB.instance.GetItem(bsi.itemID).item.icon;
                        itemSlot.transform.Find("Name").GetComponent<Text>().text = ItemDB.instance.GetItem(bsi.itemID).item.name +
                            " (" + bsi.count + ")";
                        itemSlot.transform.Find("Cost").GetComponent<Text>().text = bsi.cost.ToString();

                        itemSlot.GetComponent<ItemShopSlotInteraction>().ID = bsi.itemID;
                        itemSlot.GetComponent<ItemShopSlotInteraction>().item = ItemDB.instance.GetItem(bsi.itemID).item;
                        itemSlot.GetComponent<ItemShopSlotInteraction>().count = bsi.count;
                        itemSlot.GetComponent<ItemShopSlotInteraction>().cost = bsi.cost;

                        itemSlot.GetComponent<ItemShopSlotInteraction>().buyMode = true;

                        itemSlot.transform.SetParent(shopListObj.transform, false);
                    }
                }
            }
            else
            {
                //building new list
                List<Item> itemsAccountedFor = new List<Item>();

                //for each item in player inventory
                for (int i = 0; i <= pi.inventory.Count - 1; i++)
                {
                    if (!pi.inventory[i].isEquip && pi.inventory[i].itemID != 0 &&
                        ItemDB.instance.GetItem(pi.inventory[i].itemID).item.type == Item.Types.BOOK) //if item and not gold
                    {
                        Item item = ItemDB.instance.GetItem(pi.inventory[i].itemID).item;
                        if (!itemsAccountedFor.Contains(item))
                        {
                            int count = 0;
                            foreach (BasePlayerItem bpi in pi.inventory)
                            {
                                if (bpi.itemID == pi.inventory[i].itemID && !bpi.isEquip)
                                {
                                    count++;
                                }
                            }

                            GameObject itemSlot = Instantiate(Resources.Load<GameObject>("Prefabs/UI/ShopItemSlot"));

                            itemSlot.name = pi.inventory[i].itemID + ") " + item.name;
                            itemSlot.transform.Find("Icon").GetComponent<Image>().sprite = item.icon;
                            itemSlot.transform.Find("Name").GetComponent<Text>().text = item.name + " (" + count + ")";
                            itemSlot.transform.Find("Cost").GetComponent<Text>().text = item.sellValue.ToString();

                            itemSlot.GetComponent<ItemShopSlotInteraction>().ID = pi.inventory[i].itemID;
                            itemSlot.GetComponent<ItemShopSlotInteraction>().item = item;
                            itemSlot.GetComponent<ItemShopSlotInteraction>().count = count;
                            itemSlot.GetComponent<ItemShopSlotInteraction>().cost = item.sellValue;

                            itemSlot.transform.SetParent(shopListObj.transform, false);
                        }

                        itemsAccountedFor.Add(item);
                    }
                }
            }
        }

        void DrawIngredients()
        {
            if (buyMode)
            {
                foreach (BaseShopItem bsi in merchantObject.GetComponent<ShopInventory>().itemsForSale)
                {
                    if (ItemDB.instance.GetItem(bsi.itemID).item.type == Item.Types.INGREDIENT)
                    {
                        GameObject itemSlot = Instantiate(Resources.Load<GameObject>("Prefabs/UI/ShopItemSlot"));

                        itemSlot.name = bsi.itemID + ") " + ItemDB.instance.GetItem(bsi.itemID).item.name;
                        itemSlot.transform.Find("Icon").GetComponent<Image>().sprite = ItemDB.instance.GetItem(bsi.itemID).item.icon;
                        itemSlot.transform.Find("Name").GetComponent<Text>().text = ItemDB.instance.GetItem(bsi.itemID).item.name +
                            " (" + bsi.count + ")";
                        itemSlot.transform.Find("Cost").GetComponent<Text>().text = bsi.cost.ToString();

                        itemSlot.GetComponent<ItemShopSlotInteraction>().ID = bsi.itemID;
                        itemSlot.GetComponent<ItemShopSlotInteraction>().item = ItemDB.instance.GetItem(bsi.itemID).item;
                        itemSlot.GetComponent<ItemShopSlotInteraction>().count = bsi.count;
                        itemSlot.GetComponent<ItemShopSlotInteraction>().cost = bsi.cost;

                        itemSlot.GetComponent<ItemShopSlotInteraction>().buyMode = true;

                        itemSlot.transform.SetParent(shopListObj.transform, false);
                    }
                }
            }
            else
            {
                //building new list
                List<Item> itemsAccountedFor = new List<Item>();

                //for each item in player inventory
                for (int i = 0; i <= pi.inventory.Count - 1; i++)
                {
                    if (!pi.inventory[i].isEquip && pi.inventory[i].itemID != 0 &&
                        ItemDB.instance.GetItem(pi.inventory[i].itemID).item.type == Item.Types.INGREDIENT) //if item and not gold
                    {
                        Item item = ItemDB.instance.GetItem(pi.inventory[i].itemID).item;
                        if (!itemsAccountedFor.Contains(item))
                        {
                            int count = 0;
                            foreach (BasePlayerItem bpi in pi.inventory)
                            {
                                if (bpi.itemID == pi.inventory[i].itemID && !bpi.isEquip)
                                {
                                    count++;
                                }
                            }

                            GameObject itemSlot = Instantiate(Resources.Load<GameObject>("Prefabs/UI/ShopItemSlot"));

                            itemSlot.name = pi.inventory[i].itemID + ") " + item.name;
                            itemSlot.transform.Find("Icon").GetComponent<Image>().sprite = item.icon;
                            itemSlot.transform.Find("Name").GetComponent<Text>().text = item.name + " (" + count + ")";
                            itemSlot.transform.Find("Cost").GetComponent<Text>().text = item.sellValue.ToString();

                            itemSlot.GetComponent<ItemShopSlotInteraction>().ID = pi.inventory[i].itemID;
                            itemSlot.GetComponent<ItemShopSlotInteraction>().item = item;
                            itemSlot.GetComponent<ItemShopSlotInteraction>().count = count;
                            itemSlot.GetComponent<ItemShopSlotInteraction>().cost = item.sellValue;

                            itemSlot.transform.SetParent(shopListObj.transform, false);
                        }

                        itemsAccountedFor.Add(item);
                    }
                }
            }
        }

        int MaxQuantityToBuy()
        {
            int quantityInput = int.Parse(quantityIF.text); //quantity input by player
            int itemCost = tempCost; //cost for 1 of the item

            int totalCost = quantityInput * itemCost;

            if (quantityInput < 0)
            {
                quantityInput = 1;
            }

            if (quantityInput > vendorsQuantityOfItem && pi.gold >= totalCost) //can afford max quantity
            {
                return vendorsQuantityOfItem;
            }

            if (quantityInput > 0 && pi.gold < totalCost) //cannot afford
            {
                //find how many can afford and return that
                return GetMaxAffordable(itemCost, vendorsQuantityOfItem);
            }

            //if input is less/eq than vendor allows, AND has enough money for that many, return that quantity
            if (quantityInput <= vendorsQuantityOfItem && pi.gold >= totalCost)
            {
                return quantityInput;
            }

            return 0;
        }

        //THIS NEEDS TO BE UPDATED NEXT----
        int MaxQuantityToSell()
        {
            int quantityInput = int.Parse(quantityIF.text); //quantity input by player

            if (quantityInput <= 0)
            {
                quantityInput = 1;
            }

            //quantityInput should be > player's quantity of item
            if (quantityInput > GetCountInPlayerInventory(tempID, tempIsEquip))
            {
                return GetCountInPlayerInventory(tempID, tempIsEquip);
            }

            //if input is less/eq than player's inventory allows, return that quantity
            if (quantityInput <= GetCountInPlayerInventory(tempID, tempIsEquip))
            {
                return quantityInput;
            }

            return 1;
        }

        int GetCountInPlayerInventory(int ID, bool isEquip)
        {
            int count = 0;

            foreach (BasePlayerItem bpi in pi.inventory)
            {
                if (bpi.itemID == ID && bpi.isEquip == isEquip)
                {
                    count++;
                }
            }

            return count;
        }

        int GetMaxAffordable(int cost, int maxAllowed)
        {
            int tempVolume = 0;

            for (int i = 1; i <= maxAllowed; i++)
            {
                int tempCost = cost * i; //cost of each item

                if (buyMode)
                {
                    if (tempCost > pi.gold) //can't afford that quantity
                    {
                        break;
                    }
                    else
                    {
                        tempVolume++;
                    }
                }
                else
                {
                    if (tempCost > merchantGold) //can't afford that quantity
                    {
                        break;
                    }
                    else
                    {
                        tempVolume++;
                    }
                }
            }

            return tempVolume;
        }

        public void ProcessMultiple()
        {
            int itemID = tempID;
            int count = int.Parse(quantityIF.text); //number of items to buy
            int cost = int.Parse(costText.text); //total cost of purchase

            if (buyMode)
            {
                //subtract count from NPC's inventory
                RemoveFromShopInventory(itemID, count);

                //add count to player's inventory
                AddToPlayerInventory(itemID, count);

                //subtract gold from player
                pi.gold -= cost;

                //add gold to merchant
                merchantGold += cost;
            }
            else
            {
                //subtract count from player's inventory
                RemoveFromPlayerInventory(itemID, count);

                //add count to merchant's inventory
                AddToShopInventory(itemID, count);

                if (merchantGold >= cost)
                {
                    //subtract gold from merchant
                    merchantGold -= cost;

                    //add gold to player
                    pi.gold += cost;
                }
                else //merchant does not have enough gold to cover it
                {
                    pi.gold += merchantGold;

                    merchantGold = 0;
                }
            }

            //rebuild lists
            RebuildInventory();

            //play SE
            AudioManager.instance.PlaySE(AudioManager.instance.purchaseSE);

            //hide quantity window
            CloseQuantitySelect();
        }

        public void ProcessBuy()
        {
            int itemID = tempID;
            int cost = tempCost; //total cost of purchase

            //make sure money wont go into negatives.  if so, player error SE. otherwise, continue
            if (pi.gold - cost < 0)
            {
                AudioManager.instance.PlaySE(AudioManager.instance.cantActionSE);
                AnimateText(0, GameObject.Find("GoldPanel/GoldText"));
                Debug.Log("Not enough money");
            }
            else
            {
                Debug.Log("pi.gold: " + pi.gold + ", cost: " + cost);

                //subtract count from NPC's inventory
                RemoveFromShopInventory(itemID, 1);

                //add count to player's inventory
                AddToPlayerInventory(itemID, 1);

                //subtract gold from player
                pi.gold -= cost;

                //add gold to merchant
                merchantGold += cost;

                //rebuild lists
                RebuildInventory();

                //play SE
                AudioManager.instance.PlaySE(AudioManager.instance.purchaseSE);
            }

            buyMode = true;
        }

        public void ProcessSell()
        {
            int itemID = tempID;
            int cost = tempCost; //total cost of purchase

            //subtract count from Player's inventory
            RemoveFromPlayerInventory(itemID, 1);

            //add count to NPC's inventory
            AddToShopInventory(itemID, 1);

            //if merchant has enough money to cover it
            if (merchantGold >= cost)
            {
                //subtract gold from merchant
                merchantGold -= cost;

                //add gold to player
                pi.gold += cost;
            }
            else //merchant does not have enough gold to cover it
            {
                Debug.Log("pi.gold: " + pi.gold + ", merchantGold: " + merchantGold);
                pi.gold += merchantGold;

                merchantGold = 0;
            }

            //rebuild lists
            RebuildInventory();

            //play SE
            AudioManager.instance.PlaySE(AudioManager.instance.purchaseSE);

            buyMode = false;
        }

        void RebuildInventory()
        {
            List<BaseShopItem> tempBSIList = new List<BaseShopItem>();
            List<BaseShopEquipment> tempBSEList = new List<BaseShopEquipment>();

            foreach (BaseShopItem bsi in mi.itemsForSale)
            {
                if (bsi.count > 0)
                {
                    tempBSIList.Add(bsi);
                }
            }

            foreach (BaseShopEquipment bse in mi.equipmentForSale)
            {
                if (bse.count > 0)
                {
                    tempBSEList.Add(bse);
                }
            }

            mi.itemsForSale = tempBSIList;
            mi.equipmentForSale = tempBSEList;

            goldText.text = pi.gold.ToString();
            merchantGoldText.text = merchantGold.ToString();
            merchantObject.GetComponent<NPCInventory>().gold = merchantGold;

            DrawItems(selectedItemTypeIndex);

            HideDetails();
        }

        void HideDetails()
        {
            quantityCanvas.GetComponent<CanvasGroup>().alpha = 0;
            quantityCanvas.GetComponent<CanvasGroup>().interactable = false;
            quantityCanvas.GetComponent<CanvasGroup>().blocksRaycasts = false;

            //clear stuff
            GameObject infoPanel = GameObject.Find("GameManager/ShopCanvas/ShopBG/InfoPanel");

            infoPanel.transform.Find("EquipDesc").gameObject.GetComponent<CanvasGroup>().alpha = 0.0f;
            infoPanel.GetComponent<CanvasGroup>().alpha = 0.0f;

            infoPanel.transform.Find("Name").GetComponent<Text>().text = "";

            infoPanel.transform.Find("WeightVal").GetComponent<Text>().text = "";
            infoPanel.transform.Find("Icon").GetComponent<Image>().sprite = null;

            infoPanel.transform.Find("ItemDesc/Description").GetComponent<Text>().text = "";

            foreach (Transform child in infoPanel.transform.Find("EquipDesc/Description").gameObject.transform)
            {
                Destroy(child.gameObject);
            }

            foreach (Transform child in GameObject.Find("PrefabView/ItemPrefab/PrefabView").transform)
            {
                Destroy(child.gameObject);
            }
        }

        void AddToPlayerInventory(int ID, int count)
        {
            if (!tempIsEquip)
            {
                for (int i = 0; i <= count; i++)
                {
                    pi.Add(ID, false);
                }
            }
            else
            {
                for (int i = 0; i <= count; i++)
                {
                    pi.Add(ID, true);
                }
            }
        }

        void RemoveFromShopInventory(int ID, int count)
        {
            if (!tempIsEquip)
            {
                foreach (BaseShopItem bsi in mi.itemsForSale)
                {
                    if (bsi.itemID == ID)
                    {
                        bsi.count -= count;

                        break;
                    }
                }
            }
            else
            {
                foreach (BaseShopEquipment bse in mi.equipmentForSale)
                {
                    if (bse.itemID == ID)
                    {
                        bse.count -= count;

                        break;
                    }
                }
            }
        }

        void AddToShopInventory(int ID, int count)
        {
            if (!tempIsEquip)
            {
                bool found = false;
                foreach (BaseShopItem bsi in mi.itemsForSale)
                {
                    if (bsi.itemID == ID)
                    {
                        bsi.count += count;
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    BaseShopItem newBSI = new BaseShopItem
                    {
                        itemID = ID,
                        count = count,
                        cost = ItemDB.instance.GetItem(ID).item.sellValue * 2
                    };

                    mi.itemsForSale.Add(newBSI);
                }
            }
            else
            {
                bool found = false;
                foreach (BaseShopEquipment bse in mi.equipmentForSale)
                {
                    if (bse.itemID == ID)
                    {
                        bse.count += count;
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    BaseShopEquipment newBSE = new BaseShopEquipment
                    {
                        itemID = ID,
                        count = count,
                        cost = EquipmentDB.instance.GetEquip(ID).equipment.sellValue * 2
                    };

                    mi.equipmentForSale.Add(newBSE);
                }
            }
        }

        void RemoveFromPlayerInventory(int ID, int count)
        {
            for (int i = 0; i <= count - 1; i++)
            {
                if (!tempIsEquip)
                {
                    pi.Remove(ID, false);
                }
                else
                {
                    pi.Remove(ID, true);
                }
            }
        }

        public void OpenQuantityPanel()
        {
            //make sure money wont go into negatives.  if so, player error SE. otherwise, continue
            if (pi.gold - tempCost < 0)
            {
                AudioManager.instance.PlaySE(AudioManager.instance.cantActionSE);
                AnimateText(0, GameObject.Find("GoldPanel/GoldText"));
                inQuantitySelect = false;
            }
            else
            {
                Vector3 screenPoint = Input.mousePosition;
                Vector3 quantPos = new Vector3(0, 0, 0);
                quantPos.x = screenPoint.x + quantityOffsetX;
                quantPos.y = screenPoint.y + quantityOffsetY;

                quantityCanvas.transform.Find("QuantityPanel").transform.position = quantPos;

                InputField inf = quantityCanvas.transform.Find("QuantityPanel/InputField").GetComponent<InputField>();
                inf.text = "01";

                quantityCanvas.GetComponent<CanvasGroup>().alpha = 1;
                quantityCanvas.GetComponent<CanvasGroup>().interactable = true;
                quantityCanvas.GetComponent<CanvasGroup>().blocksRaycasts = true;
            }
        }

        void ToggleButtons(bool buy)
        {
            Button buttonB = GameObject.Find("ShopCanvas/ShopBG/ShopModePanel/BuyButton").GetComponent<Button>();
            Button buttonS = GameObject.Find("ShopCanvas/ShopBG/ShopModePanel/SellButton").GetComponent<Button>();

            buttonB.gameObject.transform.Find("Text").GetComponent<Text>().fontStyle = FontStyle.Normal;
            buttonS.gameObject.transform.Find("Text").GetComponent<Text>().fontStyle = FontStyle.Normal;

            buttonB.gameObject.transform.Find("Text").GetComponent<Text>().fontSize = inactiveButtonFontSize;
            buttonS.gameObject.transform.Find("Text").GetComponent<Text>().fontSize = inactiveButtonFontSize;

            if (buy)
            {
                buttonB.gameObject.transform.Find("Text").GetComponent<Text>().fontStyle = FontStyle.Bold;
                buttonB.gameObject.transform.Find("Text").GetComponent<Text>().fontSize = activeButtonFontSize;

                quantityCanvas.transform.Find("QuantityPanel/ConfirmButton/ConfirmText").GetComponent<Text>().text = "BUY";
            }
            else
            {
                buttonS.gameObject.transform.Find("Text").GetComponent<Text>().fontStyle = FontStyle.Bold;
                buttonS.gameObject.transform.Find("Text").GetComponent<Text>().fontSize = activeButtonFontSize;

                quantityCanvas.transform.Find("QuantityPanel/ConfirmButton/ConfirmText").GetComponent<Text>().text = "SELL";
            }
        }

        int GetCountInInventory(int ID, bool isEquip)
        {
            int count = 0;
            foreach (BasePlayerItem bpi in pi.inventory)
            {
                if (bpi.itemID == ID && bpi.isEquip == isEquip)
                {
                    count++;
                }
            }
            return count;
        }

        public void OpenShopUI(GameObject vendorObj)
        {
            PixelCrushers.DialogueSystem.DialogueManager.Pause();

            open = true;

            buyMode = true;

            merchantObject = vendorObj;
            merchantGold = merchantObject.GetComponent<NPCInventory>().gold;

            menu.ShowCanvas(menu.paramCanvas, false);
            menu.ShowCanvas(menu.compassCanvas, false);
            menu.ShowCanvas(menu.crosshairCG, false);

            menu.CursorActive(true);

            player.GetComponent<PixelCrushers.DialogueSystem.Selector>().enabled = false;

            DrawItems(1);

            goldText.text = pi.gold.ToString();
            merchantGoldText.text = merchantGold.ToString();
            mi = merchantObject.GetComponent<ShopInventory>();

            gameObject.GetComponent<CanvasGroup>().alpha = 1;
            gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
            gameObject.GetComponent<CanvasGroup>().interactable = true;
        }

        void ContinueDialogueManager()
        {
            PixelCrushers.DialogueSystem.DialogueManager.Unpause();

            //play talking animation
            if (merchantObject.GetComponent<Animator>() != null)
            {
                merchantObject.GetComponent<Animator>().SetTrigger("TalkShort");
            }
        }

        void CloseShopUI()
        {
            ContinueDialogueManager();

            open = false;

            foreach (Transform child in shopListObj.transform)
            {
                Destroy(child.gameObject);
            }

            gameObject.GetComponent<CanvasGroup>().alpha = 0;
            gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
            gameObject.GetComponent<CanvasGroup>().interactable = false;

            /*menu.ShowCanvas(menu.paramCanvas, true);
            menu.ShowCanvas(menu.compassCanvas, true);
            menu.ShowCanvas(menu.crosshairCG, true);

            menu.CursorActive(false);

            selector.enabled = true;*/
        }

        void CloseQuantitySelect()
        {
            inQuantitySelect = false;

            HideDetails();
        }

        public void SetShopMode(bool buy)
        {
            ToggleButtons(buy);

            buyMode = buy;

            DrawItems(selectedItemTypeIndex);
        }
    }

}