using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DeepFry
{

    public class PowerButtonMenuInteraction : MonoBehaviour
    {
        [ReadOnly] public BasePower power;
        [ReadOnly] public int ID;

        Text descText;

        GameObject infoPanel;

        bool selected;

        BaseHero playerHero;
        PlayerInventory playerInventory;
        GameMenu menu;

        private void Start()
        {
            infoPanel = GameObject.Find("GameManager/Menus/PowersMenuCanvas/MenuPanel/PowersDescPanel");

            descText = infoPanel.transform.Find("Description").GetComponent<Text>();

            playerHero = GameObject.Find("GameManager/PlayerManager").GetComponent<PlayerManager>().playersCharacter;
            menu = GameObject.Find("GameManager/Menus").GetComponent<GameMenu>();
        }

        private void Update()
        {
            if (selected && Input.GetButtonDown("Fire1"))
            {
                //equip to primary
                playerHero.leftHandPower = power;
                //play any effects

                BoldText();
            }
            else if (selected && Input.GetButtonDown("Fire2"))
            {
                //equip to secondary
                playerHero.rightHandPower = power;
                //play any effects

                BoldText();
            }
        }

        public void Enter()
        {
            selected = true;
            ShowPowerDetails();
        }

        public void Exit()
        {
            selected = false;
            HidePowerDetails();
        }

        void ShowPowerDetails()
        {
            descText.text = power.description;
        }

        void HidePowerDetails()
        {
            descText.text = "";
        }

        void BoldText()
        {
            foreach (Transform child in GameObject.Find("GameManager/Menus/PowersMenuCanvas/MenuPanel/" +
                "PowersListPanel/PowersScroller/Viewport/Content/PowerList").transform)
            {
                if (playerHero.leftHandPower == AttackDB.instance.GetAttack(child.gameObject.GetComponent<PowerButtonMenuInteraction>().ID))
                {
                    child.Find("BG/Name").GetComponent<Text>().text = "(L) " + playerHero.leftHandPower.name;
                    child.Find("BG/Name").GetComponent<Text>().fontStyle = FontStyle.Bold;
                }
                else if (playerHero.rightHandPower == AttackDB.instance.GetAttack(child.gameObject.GetComponent<PowerButtonMenuInteraction>().ID))
                {
                    child.Find("BG/Name").GetComponent<Text>().text = "(R) " + playerHero.rightHandPower.name;
                    child.Find("BG/Name").GetComponent<Text>().fontStyle = FontStyle.Bold;
                }
                else
                {
                    child.Find("BG/Name").GetComponent<Text>().text =
                        AttackDB.instance.GetAttack(child.gameObject.GetComponent<PowerButtonMenuInteraction>().ID).name;
                    child.Find("BG/Name").GetComponent<Text>().fontStyle = FontStyle.Normal;
                }
            }
        }
    }

}