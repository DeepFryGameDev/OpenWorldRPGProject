using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeepFry
{

    [System.Serializable]
    public class QuestGiver : MonoBehaviour
    {
        public List<int> questIDs = new List<int>();

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            //check if player has completed quests
            foreach (int questID in questIDs)
            {
                if (ReadyForPickup(questID))
                {
                    if (gameObject.GetComponent<MapMinimap.MapIcon>().icon != GameManager.instance.QuestReadyForPickupIcon)
                    {
                        gameObject.GetComponent<MapMinimap.MapIcon>().icon = GameManager.instance.QuestReadyForPickupIcon;
                        gameObject.GetComponent<CompassNavigatorPro.CompassProPOI>().iconNonVisited = GameManager.instance.QuestReadyForPickupIcon;
                        gameObject.GetComponent<CompassNavigatorPro.CompassProPOI>().iconVisited = GameManager.instance.QuestReadyForPickupIcon;
                    }
                }
                else
                {
                    gameObject.GetComponent<MapMinimap.MapIcon>().icon = null;
                    gameObject.GetComponent<CompassNavigatorPro.CompassProPOI>().iconNonVisited = null;
                    gameObject.GetComponent<CompassNavigatorPro.CompassProPOI>().iconVisited = null;
                    gameObject.GetComponent<CompassNavigatorPro.CompassProPOI>().visibility = CompassNavigatorPro.POI_VISIBILITY.AlwaysHidden;
                }
            }
        }

        bool ReadyForPickup(int ID)
        {
            //check if quest level is in level range of player
            int min = QuestDB.instance.quests[ID].quest.level - GameManager.instance.questPickupAvailableRange;
            int max = QuestDB.instance.quests[ID].quest.level + GameManager.instance.questPickupAvailableRange;

            int curLevel = GameObject.Find("PlayerManager").GetComponent<PlayerManager>().playersCharacter.currentLevel;

            if (curLevel < min || curLevel > max)
            {
                return false;
            }

            //check if already active
            if (IsQuestActive(ID))
            {
                return false;
            }

            //check if already completed
            if (IsQuestCompleted(ID))
            {
                return false;
            }

            //check if there are any pre-req quests not completed
            foreach (int prereqQuestID in QuestDB.instance.quests[ID].quest.dependentQuests)
            {
                foreach (BaseQuest activeQuest in QuestManager.instance.activeQuests)
                {
                    if (activeQuest.ID == prereqQuestID)
                    {
                        //is actively on the pre-req quest
                        return false;
                    }
                }
            }

            return CompletedDependents(ID);
        }

        bool CompletedDependents(int ID)
        {
            bool check = false;

            //if ID is nowhere in completed quests, return false

            foreach (int prereqQuestID in QuestDB.instance.quests[ID].quest.dependentQuests)
            {
                foreach (BaseQuest completedQuest in QuestManager.instance.completedQuests)
                {
                    check = false;

                    if (completedQuest.ID == prereqQuestID)
                    {
                        check = true;
                        break;
                    }
                }

                if (!check)
                {
                    Debug.Log("finish the quest: " + prereqQuestID);
                    return false;
                }
            }

            return true;
        }

        bool IsQuestActive(int ID)
        {
            foreach (BaseQuest quest in QuestManager.instance.activeQuests)
            {
                if (quest.ID == ID)
                {
                    return true;
                }
            }

            return false;
        }

        bool IsQuestCompleted(int ID)
        {
            foreach (BaseQuest quest in QuestManager.instance.completedQuests)
            {
                if (quest.ID == ID)
                {
                    return true;
                }
            }

            return false;
        }
    }

}