using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeepFry
{

    public class PowersScroll : MonoBehaviour
    {
        bool visible;
        int itemCount;
        private GameObject powerList;
        private GameMenu menu;

        private void Start()
        {
            powerList = GameObject.Find("GameManager/Menus/PowersMenuCanvas/MenuPanel/PowersListPanel/PowersScroller/Viewport/PowerList");
            menu = GameObject.Find("GameManager/Menus").GetComponent<GameMenu>();
        }

        private void Update()
        {
            if (visible)
            {
                if (Input.GetAxis("Mouse ScrollWheel") > 0)
                {
                    Vector3 newPos = new Vector3(powerList.transform.position.x,
                        powerList.transform.position.y - menu.scrollSpeed, powerList.transform.position.z);

                    powerList.transform.position = newPos;
                }
                else if (Input.GetAxis("Mouse ScrollWheel") < 0)
                {
                    Vector3 newPos = new Vector3(powerList.transform.position.x,
        powerList.transform.position.y + menu.scrollSpeed, powerList.transform.position.z);

                    powerList.transform.position = newPos;
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