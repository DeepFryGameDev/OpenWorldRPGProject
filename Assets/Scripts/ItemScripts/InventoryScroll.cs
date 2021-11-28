using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeepFry
{

    public class InventoryScroll : MonoBehaviour
    {
        bool visible;
        int itemCount;
        private GameObject inventoryList;
        private GameMenu menu;

        private void Start()
        {
            inventoryList = GameObject.Find("GameManager/Menus/InventoryMenuCanvas/MenuPanel/InventoryPanel/Scroll View/Viewport/InventoryList");
            menu = GameObject.Find("GameManager/Menus").GetComponent<GameMenu>();
        }

        private void Update()
        {
            if (visible)
            {
                if (Input.GetAxis("Mouse ScrollWheel") > 0)
                {
                    Vector3 newPos = new Vector3(inventoryList.transform.position.x,
                        inventoryList.transform.position.y - menu.scrollSpeed, inventoryList.transform.position.z);

                    inventoryList.transform.position = newPos;
                }
                else if (Input.GetAxis("Mouse ScrollWheel") < 0)
                {
                    Vector3 newPos = new Vector3(inventoryList.transform.position.x,
        inventoryList.transform.position.y + menu.scrollSpeed, inventoryList.transform.position.z);

                    inventoryList.transform.position = newPos;
                }
            }
        }

        public void Enter()
        {
            visible = true;
        }

        public void Exit()
        {
            visible = false;
        }
    }

}