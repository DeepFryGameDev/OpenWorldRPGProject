using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace DeepFry
{

    [System.Serializable]
    public class MenuScript : MonoBehaviour
    {
        #region Singleton
        public static MenuScript instance; //call instance to get the single active AttackDB for the game

        private void Awake()
        {
            if (instance != null)
            {
                //Debug.LogWarning("More than one instance of ItemDB found!");
                return;
            }

            instance = this;
        }
        #endregion

        [MenuItem("Dev Tools/Camera/Toggle Debug Camera")] //displays dialogue canvas in Unity editor
        public static void ToggleDebugCamera()
        {
            if (GameObject.Find("DebugCamera").GetComponent<Camera>().enabled)
            {
                GameObject.Find("DebugCamera").GetComponent<Camera>().enabled = false;
                GameObject.Find("MenuCamera").GetComponent<Camera>().enabled = true;
            }
            else
            {
                GameObject.Find("DebugCamera").GetComponent<Camera>().enabled = true;
                GameObject.Find("MenuCamera").GetComponent<Camera>().enabled = false;
            }
        }

        [MenuItem("Dev Tools/Dialogue Canvas/Display")] //displays dialogue canvas in Unity editor
        public static void DisplayDialogueCanvas()
        {
            GameObject.Find("DialogueCanvas").GetComponent<CanvasGroup>().alpha = 1;
        }

        [MenuItem("Dev Tools/Dialogue Canvas/Hide")] //hides dialogue canvas in Unity editor
        public static void HideDialogueCanvas()
        {
            GameObject.Find("DialogueCanvas").GetComponent<CanvasGroup>().alpha = 0;
        }

        //for Menu
        [MenuItem("Dev Tools/Menu Canvas/Display Main Menu")] //displays menu canvas in Unity editor
        public static void DisplayMenuCanvas()
        {
            GameObject.Find("MainMenuCanvas").GetComponent<CanvasGroup>().alpha = 1;
            GameObject.Find("InventoryMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
            GameObject.Find("PowersMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
            GameObject.Find("MapMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        }

        [MenuItem("Dev Tools/Menu Canvas/Display Inventory Menu")] //displays item menu canvas in Unity editor
        public static void DisplayInventoryMenuCanvas()
        {
            GameObject.Find("MainMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
            GameObject.Find("InventoryMenuCanvas").GetComponent<CanvasGroup>().alpha = 1;
            GameObject.Find("PowersMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
            GameObject.Find("JournalMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
            GameObject.Find("MapMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        }

        [MenuItem("Dev Tools/Menu Canvas/Display Powers Menu")] //hides menu canvases in Unity editor
        public static void DisplayPowersMenuCanvas()
        {
            GameObject.Find("MainMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
            GameObject.Find("InventoryMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
            GameObject.Find("PowersMenuCanvas").GetComponent<CanvasGroup>().alpha = 1;
            GameObject.Find("JournalMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
            GameObject.Find("MapMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        }

        [MenuItem("Dev Tools/Menu Canvas/Display Journal Menu")] //hides menu canvases in Unity editor
        public static void DisplayJournalMenuCanvas()
        {
            GameObject.Find("MainMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
            GameObject.Find("InventoryMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
            GameObject.Find("PowersMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
            GameObject.Find("JournalMenuCanvas").GetComponent<CanvasGroup>().alpha = 1;
            GameObject.Find("MapMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        }

        [MenuItem("Dev Tools/Menu Canvas/Display Map Menu")] //hides menu canvases in Unity editor
        public static void DisplayMapMenuCanvas()
        {
            GameObject.Find("MainMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
            GameObject.Find("InventoryMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
            GameObject.Find("PowersMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
            GameObject.Find("JournalMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
            GameObject.Find("MapMenuCanvas").GetComponent<CanvasGroup>().alpha = 1;
        }

        [MenuItem("Dev Tools/Menu Canvas/Hide Menus")] //hides menu canvases in Unity editor
        public static void HideMenuCanvas()
        {
            GameObject.Find("MainMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
            GameObject.Find("InventoryMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
            GameObject.Find("PowersMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
            GameObject.Find("JournalMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
            GameObject.Find("MapMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        }

        [MenuItem("Dev Tools/Assign IDs and Save GameManager")]
        public static void AssignAllIDs()
        {
            AssignEnemyIDs();
            AssignItemIDs();
            AssignAttackIDs();
            AssignQuestIDs();

            PrefabUtility.ApplyPrefabInstance(GameObject.Find("GameManager"), InteractionMode.UserAction);
        }

        [MenuItem("Dev Tools/Assign IDs/Enemy IDs")]
        public static void AssignEnemyIDs()
        {
            Debug.Log("-----Writing EnemyDB-----");

            string enemyPath = Application.dataPath + "/../DBs/" + "EnemyDB.csv";

            StreamWriter enemyWriter = new StreamWriter(enemyPath);

            enemyWriter.WriteLine("ID, Name");

            int count = 0;

            foreach (BaseEnemyDBEntry entry in GameObject.Find("GameManager/DBs/EnemyDB").GetComponent<EnemyDB>().enemies)
            {
                entry.ID = GameObject.Find("GameManager/DBs/EnemyDB").GetComponent<EnemyDB>().enemies.IndexOf(entry);

                enemyWriter.WriteLine(entry.ID + "," + entry.enemy.name);

                Debug.Log(count + ") " + entry.enemy.name);
                count++;
            }

            enemyWriter.Flush();

            enemyWriter.Close();

        }

        [MenuItem("Dev Tools/Assign IDs/Item IDs")]
        public static void AssignItemIDs()
        {
            Debug.Log("-----Writing ItemDB-----");

            string itemPath = Application.dataPath + "/../DBs/" + "ItemDB.csv";

            StreamWriter itemWriter = new StreamWriter(itemPath);

            itemWriter.WriteLine("ID, Name");

            foreach (ItemDBEntry entry in GameObject.Find("GameManager/DBs/ItemDB").GetComponent<ItemDB>().items)
            {
                entry.ID = GameObject.Find("GameManager/DBs/ItemDB").GetComponent<ItemDB>().items.IndexOf(entry);
                entry.SetName();

                itemWriter.WriteLine(entry.ID + "," + entry.item.name);

                Debug.Log(entry.Name);
            }

            itemWriter.Flush();

            itemWriter.Close();


            Debug.Log("-----Writing EquipmentDB-----");

            string equipPath = Application.dataPath + "/../DBs/" + "EquipDB.csv";

            StreamWriter equipWriter = new StreamWriter(equipPath);

            equipWriter.WriteLine("ID, Name, Type, Slot");

            foreach (EquipmentDBEntry entry in GameObject.Find("GameManager/DBs/ItemDB").GetComponent<EquipmentDB>().equipment)
            {
                entry.ID = GameObject.Find("GameManager/DBs/ItemDB").GetComponent<EquipmentDB>().equipment.IndexOf(entry);
                entry.SetName();

                equipWriter.WriteLine(entry.ID + "," + entry.equipment.name + "," + entry.equipment.type.ToString() + "," + entry.equipment.equipmentSlot.ToString());

                Debug.Log(entry.Name);
            }

            equipWriter.Flush();

            equipWriter.Close();
        }

        [MenuItem("Dev Tools/Assign IDs/Attack IDs")]
        public static void AssignAttackIDs()
        {
            Debug.Log("-----Writing AttackDB-----");

            string attackPath = Application.dataPath + "/../DBs/" + "AttackDB.csv";

            StreamWriter attackWriter = new StreamWriter(attackPath);

            attackWriter.WriteLine("ID, Name, Class");

            foreach (AttackDBEntry entry in GameObject.Find("GameManager/DBs/PowerDB").GetComponent<AttackDB>().attacks)
            {
                entry.ID = GameObject.Find("GameManager/DBs/PowerDB").GetComponent<AttackDB>().attacks.IndexOf(entry);
                entry.SetName();

                attackWriter.WriteLine(entry.ID + "," + entry.power.name + "," + entry.power.powerClass.ToString());

                Debug.Log(entry.Name);
            }

            attackWriter.Flush();

            attackWriter.Close();
        }

        [MenuItem("Dev Tools/Assign IDs/Quest IDs")]
        public static void AssignQuestIDs()
        {
            Debug.Log("-----Writing QuestDB-----");

            string questPath = Application.dataPath + "/../DBs/" + "QuestDB.csv";

            StreamWriter questWriter = new StreamWriter(questPath);

            questWriter.WriteLine("ID, Name, Type");

            foreach (QuestDBEntry entry in GameObject.Find("GameManager/DBs/QuestDB").GetComponent<QuestDB>().quests)
            {
                entry.ID = GameObject.Find("GameManager/DBs/QuestDB").GetComponent<QuestDB>().quests.IndexOf(entry);
                entry.quest.ID = entry.ID;
                entry.SetName();

                questWriter.WriteLine(entry.ID + "," + entry.quest.name + "," + entry.quest.type.ToString());

                Debug.Log(entry.Name);
            }

            questWriter.Flush();

            questWriter.Close();
        }
    }

}