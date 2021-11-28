using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeepFry
{
    [System.Serializable]
    public class QuestDB : MonoBehaviour
    {
        public List<QuestDBEntry> quests = new List<QuestDBEntry>();
        public List<QuestBool> bools = new List<QuestBool>();

        Inventory playersInventory; //update this later to point to the player's inventory

        #region Singleton
        public static QuestDB instance; //call instance to get the single active QuestDB for the game

        private void Awake()
        {
            if (instance != null)
            {
                //Debug.LogWarning("More than one instance of QuestDB found!");
                return;
            }

            instance = this;

            playersInventory = GameObject.Find("Player").GetComponent<Inventory>();
        }
        #endregion

        public QuestBool GetBool(int ID)
        {
            return bools[ID];
        }

        public void AddToActiveQuests(BaseQuest quest)
        {
            Debug.Log("Quest active! - " + quest.name);
            QuestManager.instance.activeQuests.Add(quest);
        }

        public void CompleteQuest(BaseQuest quest)
        {
            Debug.Log("Quest completed! - " + quest.name);

            if (quest.type == BaseQuest.types.GATHER)
            {
                for (int i = 0; i < quest.gatherReqs[0].quantity; i++)
                {
                    playersInventory.Remove(ItemDB.instance.GetItem(quest.gatherReqs[0].itemID).item);
                }
            }

            if (quest.type == BaseQuest.types.KILLTARGETS)
            {
                quest.killReqs[0].targetsKilled = 0;
            }

            AddGoldFromQuest(quest.rewardGold);

            AddExpFromQuest(quest.rewardExp);

            if (quest.rewardItems.Count > 0)
            {
                AddRewardItem(quest.rewardItems[0].item);
            }

            RemoveFromActiveQuests(quest);
            AddToCompletedQuests(quest);
        }

        void AddGoldFromQuest(int gold)
        {
            GameManager.instance.gold += gold;
        }

        void AddExpFromQuest(int EXP)
        {

        }

        void AddRewardItem(Item item)
        {
            playersInventory.Add(item);
        }

        void RemoveFromActiveQuests(BaseQuest quest)
        {
            QuestManager.instance.activeQuests.Remove(quest);
        }

        void AddToCompletedQuests(BaseQuest quest)
        {
            QuestManager.instance.completedQuests.Add(quest);
        }

        public void UpdateQuestObjectives()
        {
            foreach (BaseQuest quest in QuestManager.instance.activeQuests)
            {
                if (quest.type == BaseQuest.types.GATHER)
                {
                    foreach (Item item in playersInventory.items)
                    {
                        if (item == ItemDB.instance.GetItem(quest.gatherReqs[0].itemID).item)
                        {
                            quest.gatherReqs[0].inInventory++;
                        }
                    }

                    if (quest.gatherReqs[0].inInventory >= quest.gatherReqs[0].quantity)
                    {
                        quest.fulfilled = true;
                    }
                    else
                    {
                        quest.fulfilled = false;
                    }
                }
                if (quest.type == BaseQuest.types.KILLTARGETS)
                {
                    if (quest.killReqs[0].targetsKilled >= quest.killReqs[0].quantity)
                    {
                        quest.fulfilled = true;
                    }
                    else
                    {
                        quest.fulfilled = false;
                    }
                }
                if (quest.type == BaseQuest.types.BOOLEAN)
                {
                    bool boolFulfilled = true;

                    foreach (BaseQuestBoolRequirement bqr in quest.boolReqs)
                    {
                        if (!bqr.objectiveFulfilled)
                        {
                            boolFulfilled = false;
                        }
                    }

                    if (boolFulfilled)
                    {
                        quest.fulfilled = true;
                    }
                    else
                    {
                        quest.fulfilled = false;
                    }
                }
            }
        }

        public void SetBool(int ID, bool val)
        {
            bools[ID].complete = val;
        }
    }

}