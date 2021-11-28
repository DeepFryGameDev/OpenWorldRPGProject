using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeepFry
{

    public class NPCInventoryList : MonoBehaviour
    {
        GameMenu menu;

        private GameObject player, npcInventoryCanvas;

        // Start is called before the first frame update
        void Start()
        {
            menu = GameObject.Find("GameManager/Menus").GetComponent<GameMenu>();
            player = GameObject.Find("Player");
            npcInventoryCanvas = GameObject.Find("GameManager/NPCInventory/NPCInventoryCanvas");
        }

        // Update is called once per frame
        void Update()
        {
            if (menu.inNpcInventory && Input.GetButtonDown("Cancel"))
            {
                npcInventoryCanvas.GetComponent<CanvasGroup>().alpha = 0;
                npcInventoryCanvas.GetComponent<CanvasGroup>().interactable = false;
                npcInventoryCanvas.GetComponent<CanvasGroup>().blocksRaycasts = false;

                menu.CursorActive(false);

                //lock camera rotation
                player.GetComponent<SmartFPController.FirstPersonController>().enabled = true;

                //allow user to enter menu again
                menu.inNpcInventory = false;
            }
        }
    }

}