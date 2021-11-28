using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DeepFry
{
    public class StartUpThings : BaseScriptedEvent
    {

        void AutostartStuff()
        {
            InitiateStats();

            AssignAllIDs();

            SetHeroEquippedPowers();

            AddQuestToActive(0);
            AddQuestToActive(1);
            AddQuestToActive(2);

            AddItems();

            LastMinuteStuff();
        }

        void LastMinuteStuff()
        {

        }

        void AddItems()
        {
            AddItem(1);
            AddItem(2);
            AddItem(3);
            AddItem(4);
            AddItem(5);
            AddItem(6);
            AddItem(7);

            ChangeGold(150);

            AddEquip(0);
            AddEquip(1);
            AddEquip(1);
            AddEquip(2);
            AddEquip(3);
            AddEquip(4);
            AddEquip(5);
            AddEquip(6);
            AddEquip(7);
            AddEquip(7);
            AddEquip(7);
            AddEquip(8);
            AddEquip(9);
            AddEquip(10);
            AddEquip(11);
            AddEquip(12);
            AddEquip(13);
            AddEquip(14);
            AddEquip(14);
            AddEquip(14);
            AddEquip(14);
            AddEquip(15);
            AddEquip(16);
            AddEquip(17);
            AddEquip(18);
            AddEquip(19);
            AddEquip(20);
            AddEquip(21);
        }

        void SetHeroEquippedPowers()
        {
            GameObject.Find("PlayerManager").GetComponent<PlayerManager>().playersCharacter.leftHandPower = AttackDB.instance.GetAttack(0);
            GameObject.Find("PlayerManager").GetComponent<PlayerManager>().playersCharacter.rightHandPower = AttackDB.instance.GetAttack(1);
        }

        void InitiateStats()
        {
            Debug.Log("Initializing stats");
            GameObject.Find("PlayerManager").GetComponent<PlayerManager>().playersCharacter.InitializeStats();
        }

        void SetRandomGridSpawnPoints()
        {
            /*
            List<string> savedSpawnPoints = new List<string>();

            foreach (BaseHero hero in HeroDB.instance.heroes)
            {
                while (hero.spawnPoint == "")
                {
                    int randomColumn = Random.Range(1, 5);
                    int randomRow = Random.Range(1, 5);

                    string spawnPoint = randomColumn.ToString() + randomRow.ToString();

                    if (!savedSpawnPoints.Contains(spawnPoint))
                    {
                        savedSpawnPoints.Add(spawnPoint);
                        hero.spawnPoint = spawnPoint;
                        Debug.Log("Setting random spawn point for " + hero.name + " - " + spawnPoint);
                    }
                }
            }*/ //remove comments when not testing

            //------
        }

        private BaseHero GetHero(int ID)
        {
            /*foreach (BaseHero hero in HeroDB.instance.heroes)
            {
                if (hero.ID == ID)
                {
                    return hero;
                }
            }*/
            return null;
        }

        public void AssignAllIDs()
        {
            //AssignHeroIDs();
            AssignEnemyIDs();
            AssignItemIDs();
            AssignAttackIDs();
            AssignQuestIDs();
        }

        public void AssignHeroIDs()
        {/*
            foreach (BaseHero hero in GameObject.Find("GameManager/DBs/HeroDB").GetComponent<HeroDB>().heroes)
            {
                Debug.Log("Assigning ID " + GameObject.Find("GameManager/DBs/HeroDB").GetComponent<HeroDB>().heroes.IndexOf(hero) + " to hero " + hero.name);
                //hero.ID = GameObject.Find("GameManager/DBs/HeroDB").GetComponent<HeroDB>().heroes.IndexOf(hero);
            }*/
        }

        public void AssignEnemyIDs()
        {
            Debug.Log("-----EnemyDB-----");
            int count = 0;
            foreach (BaseEnemyDBEntry entry in GameObject.Find("GameManager/DBs/EnemyDB").GetComponent<EnemyDB>().enemies)
            {
                entry.ID = GameObject.Find("GameManager/DBs/EnemyDB").GetComponent<EnemyDB>().enemies.IndexOf(entry);
                Debug.Log(count + ") " + entry.enemy.name);
                count++;
            }
        }

        public void AssignItemIDs()
        {
            Debug.Log("-----ItemDB-----");
            foreach (ItemDBEntry entry in GameObject.Find("GameManager/DBs/ItemDB").GetComponent<ItemDB>().items)
            {
                entry.ID = GameObject.Find("GameManager/DBs/ItemDB").GetComponent<ItemDB>().items.IndexOf(entry);
                entry.SetName();

                Debug.Log(entry.Name);
            }

            Debug.Log("-----EquipmentDB-----");
            foreach (EquipmentDBEntry entry in GameObject.Find("GameManager/DBs/ItemDB").GetComponent<EquipmentDB>().equipment)
            {
                entry.ID = GameObject.Find("GameManager/DBs/ItemDB").GetComponent<EquipmentDB>().equipment.IndexOf(entry);
                entry.SetName();

                Debug.Log(entry.Name);
            }
        }

        public void AssignAttackIDs()
        {
            Debug.Log("-----AttackDB-----");
            foreach (AttackDBEntry entry in GameObject.Find("GameManager/DBs/PowerDB").GetComponent<AttackDB>().attacks)
            {
                entry.ID = GameObject.Find("GameManager/DBs/PowerDB").GetComponent<AttackDB>().attacks.IndexOf(entry);
                entry.SetName();

                Debug.Log(entry.Name);
            }
        }

        public void AssignQuestIDs()
        {
            Debug.Log("-----QuestDB-----");
            foreach (QuestDBEntry entry in GameObject.Find("GameManager/DBs/QuestDB").GetComponent<QuestDB>().quests)
            {
                entry.ID = GameObject.Find("GameManager/DBs/QuestDB").GetComponent<QuestDB>().quests.IndexOf(entry);
                entry.quest.ID = entry.ID;

                entry.SetName();

                Debug.Log(entry.Name);
            }
        }

        void LoadScenes()
        {

        }

    }
}
