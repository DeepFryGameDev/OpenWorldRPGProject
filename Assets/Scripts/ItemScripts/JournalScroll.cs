using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeepFry
{

    public class JournalScroll : MonoBehaviour
    {
        bool visible;
        int itemCount;
        private GameObject questList;
        private GameMenu menu;

        private void Start()
        {
            questList = GameObject.Find("GameManager/Menus/JournalMenuCanvas/MenuPanel/QuestListPanel/QuestScroller/Viewport/QuestList");
            menu = GameObject.Find("GameManager/Menus").GetComponent<GameMenu>();
        }

        private void Update()
        {
            if (visible)
            {
                if (Input.GetAxis("Mouse ScrollWheel") > 0)
                {
                    Vector3 newPos = new Vector3(questList.transform.position.x,
                        questList.transform.position.y - menu.scrollSpeed, questList.transform.position.z);

                    questList.transform.position = newPos;
                }
                else if (Input.GetAxis("Mouse ScrollWheel") < 0)
                {
                    Vector3 newPos = new Vector3(questList.transform.position.x,
        questList.transform.position.y + menu.scrollSpeed, questList.transform.position.z);

                    questList.transform.position = newPos;
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