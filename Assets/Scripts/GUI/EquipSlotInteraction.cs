using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DeepFry
{

    public class EquipSlotInteraction : MonoBehaviour
    {
        bool visible;

        Text nameText;
        GameObject descObj, descParent;
        Image icon;
        Text weightText;

        GameObject infoPanel;

        GameObject objToModify;

        BaseHero playerHero;
        PlayerInventory playerInventory;
        GameMenu menu;

        [ReadOnly] public Equipment equip;

        [ReadOnly] public int ID;

        GameObject equipDescPrefab;

        GameObject playerObj;

        // Start is called before the first frame update
        void Start()
        {
            infoPanel = GameObject.Find("GameManager/Menus/InventoryMenuCanvas/MenuPanel/InfoPanel");

            nameText = infoPanel.transform.Find("Name").GetComponent<Text>();
            descParent = infoPanel.transform.Find("EquipDesc").gameObject;
            descObj = descParent.transform.Find("Description").gameObject;
            icon = infoPanel.transform.Find("Icon").GetComponent<Image>();
            weightText = infoPanel.transform.Find("WeightVal").GetComponent<Text>();

            playerHero = GameObject.Find("GameManager/PlayerManager").GetComponent<PlayerManager>().playersCharacter;
            playerInventory = GameObject.Find("GameManager/PlayerManager").GetComponent<PlayerInventory>();
            menu = GameObject.Find("GameManager/Menus").GetComponent<GameMenu>();

            equipDescPrefab = Resources.Load<GameObject>("Prefabs/UI/EquipDescPrefab");
            playerObj = GameObject.Find("Player");
        }

        private void Update()
        {
            if (visible && Input.GetButtonDown("Confirm"))
            {
                Equip();
            }
            else if (visible && Input.GetButtonDown("Secondary"))
            {
                DropItem();
            }
        }

        void Equip()
        {
            Debug.Log("equipping: " + equip.name);

            playerHero.Equip(equip);

            ChangeToEquipped();

            menu.DrawItems(menu.selectedItemTypeIndex);

            menu.DestroyEquipDescs();
        }

        void ChangeToEquipped()
        {
            //set icon of equipped slot to icon of equipped
            SetIcon();

            //set script component vals to match equipped item
            objToModify.GetComponent<ActiveEquipSlotInteraction>().hasEquip = true;
            objToModify.GetComponent<ActiveEquipSlotInteraction>().equipID = ID;
            objToModify.GetComponent<ActiveEquipSlotInteraction>().equip = equip;
        }

        void SetIcon()
        {
            switch (equip.equipmentSlot)
            {
                case EquipmentSlot.CHEST:
                    objToModify = GameObject.Find("GameManager/Menus/InventoryMenuCanvas/MenuPanel/EquipPanel/" +
        "EquipColPanelL/EquipColL/ChestEquipSlot");
                    break;
                case EquipmentSlot.FEET:
                    objToModify = GameObject.Find("GameManager/Menus/InventoryMenuCanvas/MenuPanel/EquipPanel/" +
    "EquipColPanelL/EquipColL/BootsEquipSlot");
                    break;
                case EquipmentSlot.HANDS:
                    objToModify = GameObject.Find("GameManager/Menus/InventoryMenuCanvas/MenuPanel/EquipPanel/" +
        "EquipColPanelR/EquipColR/HandEquipSlot");
                    break;
                case EquipmentSlot.HEAD:
                    objToModify = GameObject.Find("GameManager/Menus/InventoryMenuCanvas/MenuPanel/EquipPanel/" +
    "EquipColPanelL/EquipColL/HelmEquipSlot");
                    break;
                case EquipmentSlot.LEFTARM:
                    objToModify = GameObject.Find("GameManager/Menus/InventoryMenuCanvas/MenuPanel/EquipPanel/" +
    "EquipRowPanel/EquipRow/LeftHandEquipSlot");
                    break;
                case EquipmentSlot.NECK:
                    objToModify = GameObject.Find("GameManager/Menus/InventoryMenuCanvas/MenuPanel/EquipPanel/" +
        "EquipColPanelR/EquipColR/NeckEquipSlot");
                    break;
                case EquipmentSlot.RELIC:
                    objToModify = GameObject.Find("GameManager/Menus/InventoryMenuCanvas/MenuPanel/EquipPanel/" +
    "EquipRowPanel/EquipRow/RelicEquipSlot");
                    break;
                case EquipmentSlot.RIGHTARM:
                    objToModify = GameObject.Find("GameManager/Menus/InventoryMenuCanvas/MenuPanel/EquipPanel/" +
    "EquipRowPanel/EquipRow/RightHandEquipSlot");
                    break;
                case EquipmentSlot.RING:
                    objToModify = GameObject.Find("GameManager/Menus/InventoryMenuCanvas/MenuPanel/EquipPanel/" +
        "EquipColPanelR/EquipColR/RingEquipSlot");
                    break;
                case EquipmentSlot.WRIST:
                    objToModify = GameObject.Find("GameManager/Menus/InventoryMenuCanvas/MenuPanel/EquipPanel/" +
        "EquipColPanelR/EquipColR/WristEquipSlot");
                    break;
                case EquipmentSlot.SHOULDERS:
                    objToModify = GameObject.Find("GameManager/Menus/InventoryMenuCanvas/MenuPanel/EquipPanel/" +
        "EquipColPanelL/EquipColL/ShoulderEquipSlot");
                    break;
            }

            objToModify.transform.Find("Button/IconBG/Icon").GetComponent<Image>().sprite = equip.icon;
        }

        public void Enter()
        {
            visible = true;
            ShowEquipDetails();
        }

        public void Exit()
        {
            visible = false;
            HideEquipDetails();
        }

        void ShowEquipDetails()
        {
            nameText.text = equip.name;

            weightText.text = equip.weight.ToString();
            icon.sprite = equip.icon;

            //set desc panel info
            SetDescInfo();

            descParent.GetComponent<CanvasGroup>().alpha = 1.0f;
            infoPanel.GetComponent<CanvasGroup>().alpha = 1.0f;
        }

        /// <summary>
        /// Sets given equip to appropriate slot on hero's equipment slot, and removes it from the inventory
        /// </summary>
        /// <param name="newEquip">Equip to be set to equipment slot</param>
        void PreviewEquip(Equipment newEquip, BaseHero hero)
        {
            //Debug.Log((int)newEquip.equipmentSlot);
            hero.equipment[(int)newEquip.equipmentSlot] = newEquip;

            PreviewUpdateStats(hero);
        }

        void PreviewUpdateStats(BaseHero hero)
        {
            #region preperation

            hero.preEquipmentSTR = hero.baseSTR;
            hero.preEquipmentSTA = hero.baseSTA;
            hero.preEquipmentAGI = hero.baseAGI;
            hero.preEquipmentCHA = hero.baseCHA;
            hero.preEquipmentINT = hero.baseINT;
            hero.preEquipmentEND = hero.baseEND;
            hero.preEquipmentCON = hero.baseCON;
            hero.preEquipmentLUK = hero.baseLUK;
            hero.preEquipmentWIS = hero.baseWIS;
            hero.preEquipmentPER = hero.basePER;

            hero.preEquipmentMGT = hero.baseMGT;
            hero.preEquipmentPOW = hero.basePOW;
            hero.preEquipmentDEF = hero.baseDEF;
            hero.preEquipmentPDEF = hero.basePDEF;

            int tempStrength = 0, tempStamina = 0, tempAgility = 0, tempCharisma = 0, tempIntelligence = 0, tempEndurance = 0,
                tempConstitution = 0, tempLuck = 0, tempWisdom = 0, tempPerception = 0;
            int tempMGT = 0, tempPOW = 0, tempDEF = 0, tempPDEF = 0;

            foreach (Equipment equipment in hero.equipment)
            {
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

            hero.fromEquipmentSTR = tempStrength;
            hero.fromEquipmentSTA = tempStamina;
            hero.fromEquipmentAGI = tempAgility;
            hero.fromEquipmentCHA = tempCharisma;
            hero.fromEquipmentINT = tempIntelligence;
            hero.fromEquipmentEND = tempEndurance;
            hero.fromEquipmentCON = tempConstitution;
            hero.fromEquipmentLUK = tempLuck;
            hero.fromEquipmentWIS = tempWisdom;
            hero.fromEquipmentPER = tempPerception;

            hero.fromEquipmentMGT = tempMGT;
            hero.fromEquipmentPOW = tempPOW;
            hero.fromEquipmentDEF = tempDEF;
            hero.fromEquipmentPDEF = tempPDEF;
            #endregion

            #region post equipment stats
            hero.postEquipmentSTR = hero.baseSTR + hero.fromEquipmentSTR;
            hero.postEquipmentSTA = hero.baseSTA + hero.fromEquipmentSTA;
            hero.postEquipmentAGI = hero.baseAGI + hero.fromEquipmentAGI;
            hero.postEquipmentCHA = hero.baseCHA + hero.fromEquipmentCHA;
            hero.postEquipmentINT = hero.baseINT + hero.fromEquipmentINT;
            hero.postEquipmentEND = hero.baseEND + hero.fromEquipmentEND;
            hero.postEquipmentCON = hero.baseCON + hero.fromEquipmentCON;
            hero.postEquipmentLUK = hero.baseLUK + hero.fromEquipmentLUK;
            hero.postEquipmentWIS = hero.baseWIS + hero.fromEquipmentWIS;
            hero.postEquipmentPER = hero.basePER + hero.fromEquipmentPER;

            hero.postEquipmentMGT = hero.baseMGT + hero.fromEquipmentMGT;
            hero.postEquipmentPOW = hero.basePOW + hero.fromEquipmentPOW;
            hero.postEquipmentDEF = hero.baseDEF + hero.fromEquipmentDEF;
            hero.postEquipmentPDEF = hero.basePDEF + hero.fromEquipmentPDEF;
            #endregion

            #region talents
            hero.finalSTR = hero.postEquipmentSTR;
            hero.finalSTA = hero.postEquipmentSTA;
            hero.finalAGI = hero.postEquipmentAGI;
            hero.finalCHA = hero.postEquipmentCHA;
            hero.finalINT = hero.postEquipmentINT;
            hero.finalEND = hero.postEquipmentEND;
            hero.finalCON = hero.postEquipmentAGI;
            hero.finalLUK = hero.postEquipmentLUK;
            hero.finalWIS = hero.postEquipmentWIS;
            hero.finalPER = hero.postEquipmentPER;

            hero.finalMGT = hero.postEquipmentMGT;
            hero.finalPOW = hero.postEquipmentPOW;
            hero.finalDEF = hero.postEquipmentDEF;
            hero.finalPDEF = hero.postEquipmentPDEF;

            TalentEffects effect = new TalentEffects();

            //insert talents or w/e here
            #endregion
        }

        public BaseHero NewHeroForEquip(BaseHero hero)
        {
            BaseHero copy = new BaseHero();

            copy.equipment[0] = NewEquipment(hero.equipment[0]);
            copy.equipment[1] = NewEquipment(hero.equipment[1]);
            copy.equipment[2] = NewEquipment(hero.equipment[2]);
            copy.equipment[3] = NewEquipment(hero.equipment[3]);
            copy.equipment[4] = NewEquipment(hero.equipment[4]);
            copy.equipment[5] = NewEquipment(hero.equipment[5]);
            copy.equipment[6] = NewEquipment(hero.equipment[6]);
            copy.equipment[7] = NewEquipment(hero.equipment[7]);
            copy.equipment[8] = NewEquipment(hero.equipment[8]);
            copy.equipment[9] = NewEquipment(hero.equipment[9]);
            copy.equipment[10] = NewEquipment(hero.equipment[10]);

            copy.baseMGT = hero.baseMGT;
            copy.basePOW = hero.basePOW;
            copy.baseDEF = hero.baseDEF;
            copy.basePDEF = hero.basePDEF;

            copy.baseSTR = hero.baseSTR;
            copy.baseSTA = hero.baseSTA;
            copy.baseAGI = hero.baseAGI;
            copy.baseCHA = hero.baseCHA;
            copy.baseINT = hero.baseINT;
            copy.baseEND = hero.baseEND;
            copy.baseCON = hero.baseCON;
            copy.baseLUK = hero.baseLUK;
            copy.baseWIS = hero.baseWIS;
            copy.basePER = hero.basePER;

            copy.preEquipmentSTR = hero.preEquipmentSTR;
            copy.preEquipmentSTA = hero.preEquipmentSTA;
            copy.preEquipmentAGI = hero.preEquipmentAGI;
            copy.preEquipmentCHA = hero.preEquipmentCHA;
            copy.preEquipmentINT = hero.preEquipmentINT;
            copy.preEquipmentEND = hero.preEquipmentEND;
            copy.preEquipmentCON = hero.preEquipmentCON;
            copy.preEquipmentLUK = hero.preEquipmentLUK;
            copy.preEquipmentWIS = hero.preEquipmentWIS;
            copy.preEquipmentPER = hero.preEquipmentPER;

            copy.preEquipmentMGT = hero.preEquipmentMGT;
            copy.preEquipmentPOW = hero.preEquipmentPOW;
            copy.preEquipmentDEF = hero.preEquipmentDEF;
            copy.preEquipmentPDEF = hero.preEquipmentPDEF;

            copy.fromEquipmentSTR = hero.fromEquipmentSTR;
            copy.fromEquipmentSTA = hero.fromEquipmentSTA;
            copy.fromEquipmentAGI = hero.fromEquipmentAGI;
            copy.fromEquipmentCHA = hero.fromEquipmentCHA;
            copy.fromEquipmentINT = hero.fromEquipmentINT;
            copy.fromEquipmentEND = hero.fromEquipmentEND;
            copy.fromEquipmentCON = hero.fromEquipmentCON;
            copy.fromEquipmentLUK = hero.fromEquipmentLUK;
            copy.fromEquipmentWIS = hero.fromEquipmentWIS;
            copy.fromEquipmentPER = hero.fromEquipmentPER;

            copy.fromEquipmentMGT = hero.fromEquipmentMGT;
            copy.fromEquipmentPOW = hero.fromEquipmentPOW;
            copy.fromEquipmentDEF = hero.fromEquipmentDEF;
            copy.fromEquipmentPDEF = hero.fromEquipmentPDEF;

            copy.postEquipmentSTR = hero.postEquipmentSTR;
            copy.postEquipmentSTA = hero.postEquipmentSTA;
            copy.postEquipmentAGI = hero.postEquipmentAGI;
            copy.postEquipmentCHA = hero.postEquipmentCHA;
            copy.postEquipmentINT = hero.postEquipmentINT;
            copy.postEquipmentEND = hero.postEquipmentEND;
            copy.postEquipmentCON = hero.postEquipmentCON;
            copy.postEquipmentLUK = hero.postEquipmentLUK;
            copy.postEquipmentWIS = hero.postEquipmentWIS;
            copy.postEquipmentPER = hero.postEquipmentPER;

            copy.postEquipmentMGT = hero.postEquipmentMGT;
            copy.postEquipmentPOW = hero.postEquipmentPOW;
            copy.postEquipmentDEF = hero.postEquipmentDEF;
            copy.postEquipmentPDEF = hero.postEquipmentPDEF;

            copy.finalSTR = hero.finalSTR;
            copy.finalSTA = hero.finalSTA;
            copy.finalAGI = hero.finalAGI;
            copy.finalCHA = hero.finalCHA;
            copy.finalINT = hero.finalINT;
            copy.finalEND = hero.finalEND;
            copy.finalCON = hero.finalCON;
            copy.finalLUK = hero.finalLUK;
            copy.finalWIS = hero.finalWIS;
            copy.finalPER = hero.finalPER;

            copy.finalMGT = hero.finalMGT;
            copy.finalPOW = hero.finalPOW;
            copy.finalDEF = hero.finalDEF;
            copy.finalPDEF = hero.finalPDEF;

            return copy;
        }

        public Equipment NewEquipment(Equipment equip)
        {
            Equipment copy = new Equipment();

            copy.name = name;

            copy.equipmentSlot = equip.equipmentSlot;

            copy.Might = equip.Might;
            copy.Defense = equip.Defense;
            copy.Power = equip.Power;
            copy.PowerDefense = equip.PowerDefense;

            copy.Strength = equip.Strength;
            copy.Stamina = equip.Stamina;
            copy.Constitution = equip.Constitution;
            copy.Endurance = equip.Endurance;
            copy.Agility = equip.Agility;
            copy.Charisma = equip.Charisma;
            copy.Intelligence = equip.Intelligence;
            copy.Wisdom = equip.Wisdom;
            copy.Luck = equip.Luck;
            copy.Perception = equip.Perception;

            return copy;
        }

        void SetDescInfo()
        {
            //equipping
            BaseHero toHero = new BaseHero();
            toHero = NewHeroForEquip(playerHero);

            Equipment newEquip = new Equipment();
            newEquip = NewEquipment(equip);

            PreviewEquip(newEquip, toHero);

            int count = 0;

            #region check stats and instantiate

            //check each stat difference
            if (newEquip.Might != 0)
            {
                InstantiateDescObject(Resources.Load<Sprite>("Sprites/statIcons/mgtIcon"), newEquip.Might, playerHero.finalMGT, toHero.finalMGT);
                count++;
            }

            if (newEquip.Defense != 0)
            {
                InstantiateDescObject(Resources.Load<Sprite>("Sprites/statIcons/defIcon"), newEquip.Defense, playerHero.finalDEF, toHero.finalDEF);
                count++;
            }

            if (newEquip.Power != 0)
            {
                InstantiateDescObject(Resources.Load<Sprite>("Sprites/statIcons/powIcon"), newEquip.Power, playerHero.finalPOW, toHero.finalPOW);
                count++;
            }

            if (newEquip.PowerDefense != 0)
            {
                InstantiateDescObject(Resources.Load<Sprite>("Sprites/statIcons/pdefIcon"), newEquip.PowerDefense, playerHero.finalPDEF, toHero.finalPDEF);
                count++;
            }

            //-------------------------

            if (newEquip.Strength != 0)
            {
                InstantiateDescObject(Resources.Load<Sprite>("Sprites/statIcons/strIcon"), newEquip.Strength, playerHero.finalSTR, toHero.finalSTR);
                count++;
            }

            if (newEquip.Stamina != 0)
            {
                InstantiateDescObject(Resources.Load<Sprite>("Sprites/statIcons/staIcon"), newEquip.Stamina, playerHero.finalSTA, toHero.finalSTA);
                count++;
            }

            if (newEquip.Constitution != 0 && count < 6)
            {
                InstantiateDescObject(Resources.Load<Sprite>("Sprites/statIcons/conIcon"), newEquip.Constitution, playerHero.finalCON, toHero.finalCON);
                count++;
            }

            if (newEquip.Endurance != 0 && count < 6)
            {
                InstantiateDescObject(Resources.Load<Sprite>("Sprites/statIcons/endIcon"), newEquip.Endurance, playerHero.finalEND, toHero.finalEND);
                count++;
            }

            if (newEquip.Agility != 0 && count < 6)
            {
                InstantiateDescObject(Resources.Load<Sprite>("Sprites/statIcons/agiIcon"), newEquip.Agility, playerHero.finalAGI, toHero.finalAGI);
                count++;
            }

            if (newEquip.Charisma != 0 && count < 6)
            {
                InstantiateDescObject(Resources.Load<Sprite>("Sprites/statIcons/chaIcon"), newEquip.Charisma, playerHero.finalCHA, toHero.finalCHA);
                count++;
            }

            if (newEquip.Intelligence != 0 && count < 6)
            {
                InstantiateDescObject(Resources.Load<Sprite>("Sprites/statIcons/intIcon"), newEquip.Intelligence, playerHero.finalINT, toHero.finalINT);
                count++;
            }

            if (newEquip.Wisdom != 0 && count < 6)
            {
                InstantiateDescObject(Resources.Load<Sprite>("Sprites/statIcons/wisIcon"), newEquip.Wisdom, playerHero.finalWIS, toHero.finalWIS);
                count++;
            }

            if (newEquip.Luck != 0 && count < 6)
            {
                InstantiateDescObject(Resources.Load<Sprite>("Sprites/statIcons/lukIcon"), newEquip.Luck, playerHero.finalLUK, toHero.finalLUK);
                count++;
            }

            if (newEquip.Perception != 0 && count < 6)
            {
                InstantiateDescObject(Resources.Load<Sprite>("Sprites/statIcons/perIcon"), newEquip.Perception, playerHero.finalPER, toHero.finalPER);
            }

            #endregion
        }

        void InstantiateDescObject(Sprite statIcon, int val, int fromVal, int toVal)
        {
            //instantiate a desc object
            GameObject newDescObject = Instantiate(Resources.Load<GameObject>("Prefabs/UI/EquipDescPrefab"));

            //set the values
            newDescObject.transform.Find("StatIcon").GetComponent<Image>().sprite = statIcon;
            newDescObject.transform.Find("ValText").GetComponent<Text>().text = val.ToString();
            newDescObject.transform.Find("FromText").GetComponent<Text>().text = fromVal.ToString();
            newDescObject.transform.Find("ToText").GetComponent<Text>().text = toVal.ToString();

            if (fromVal > toVal)
            {
                //show down arrow
                newDescObject.transform.Find("ArrowIcon").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/UI/downArrow");
                newDescObject.transform.Find("ArrowIcon").GetComponent<Image>().color = Color.red;
            }
            else if (fromVal < toVal)
            {
                //show up arrow
                newDescObject.transform.Find("ArrowIcon").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/UI/upArrow");
                newDescObject.transform.Find("ArrowIcon").GetComponent<Image>().color = Color.green;
            }
            else
            {
                //no change
                newDescObject.transform.Find("ArrowIcon").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/UI/rightArrow");
            }

            //set parent to anchor to layout group
            newDescObject.transform.SetParent(descObj.transform, false);
        }

        void HideEquipDetails()
        {
            descParent.GetComponent<CanvasGroup>().alpha = 0.0f;
            infoPanel.GetComponent<CanvasGroup>().alpha = 0.0f;

            menu.DestroyEquipDescs();

            nameText.text = "";

            weightText.text = "";
            icon.sprite = null;
        }

        void DropItem()
        {
            //prepare item to for instantiate
            GameObject obj = equip.fieldPrefab;
            Vector3 newPos = new Vector3(playerObj.transform.position.x, playerObj.transform.position.y, playerObj.transform.position.z) + playerObj.transform.forward;
            Quaternion newRot = new Quaternion(0, 0, 0, 0);

            //instantiate it
            Instantiate(obj, newPos, newRot, GameObject.Find("[DROPPEDITEMS]").transform);

            //remove from inventory
            playerInventory.Remove(ID, true);

            //redraw ui
            menu.DrawItems(menu.selectedItemTypeIndex);
        }
    }

}