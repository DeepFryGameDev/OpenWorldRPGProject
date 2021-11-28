using CompassNavigatorPro;
using MapMinimap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeepFry
{

    public class QuestManager : MonoBehaviour
    {
        public static QuestManager instance;

        GameMenu menu;

        public Sprite questWaypoint;

        public List<BaseQuest> activeQuests;
        public List<BaseQuest> completedQuests;

        void Awake()
        {
            if (instance == null) //check if instance exists
            {
                instance = this; //if not set the instance to this
            }
            else if (instance != this) //if it exists but is not this instance
            {
                Destroy(gameObject); //destroy it
            }
            DontDestroyOnLoad(gameObject); //set this to be persistable across scenes

            menu = GameObject.Find("GameManager/Menus").GetComponent<GameMenu>();
        }

        public void ShowWaypoint()
        {
            if (menu.questSelected)
            {
                BaseQuest quest = GetActiveQuest(menu.questSelectedID);

                if (quest.type == BaseQuest.types.BOOLEAN)
                {
                    foreach (BaseQuestBoolRequirement bqbr in quest.boolReqs)
                    {
                        if (!bqbr.objectiveFulfilled)
                        {
                            bqbr.waypoint.GetComponent<MapIcon>().type = MapIconType.Important;
                            bqbr.waypoint.GetComponent<MapIcon>().icon = questWaypoint;
                            bqbr.waypoint.GetComponent<MapIcon>().desc = "(" + quest.name + ") " + bqbr.objectiveDescription;

                            bqbr.waypoint.GetComponent<CompassProPOI>().visibility = POI_VISIBILITY.AlwaysVisible;
                            bqbr.waypoint.GetComponent<CompassProPOI>().iconVisited = questWaypoint;
                            bqbr.waypoint.GetComponent<CompassProPOI>().iconNonVisited = questWaypoint;
                        }
                    }
                }
                else if (quest.type == BaseQuest.types.GATHER)
                {
                    foreach (BaseQuestGatherRequirement bqgr in quest.gatherReqs)
                    {
                        if (bqgr.inInventory < bqgr.quantity)
                        {
                            bqgr.waypoint.GetComponent<MapIcon>().type = MapIconType.Important;
                            bqgr.waypoint.GetComponent<MapIcon>().icon = questWaypoint;
                            bqgr.waypoint.GetComponent<MapIcon>().desc = "(" + quest.name + ") " + ItemDB.instance.GetItem(bqgr.itemID).item.name +
                                ": " + bqgr.inInventory + "/ " + bqgr.quantity;

                            bqgr.waypoint.GetComponent<CompassProPOI>().visibility = POI_VISIBILITY.AlwaysVisible;
                            bqgr.waypoint.GetComponent<CompassProPOI>().iconVisited = questWaypoint;
                            bqgr.waypoint.GetComponent<CompassProPOI>().iconNonVisited = questWaypoint;
                        }
                    }
                }
                else if (quest.type == BaseQuest.types.KILLTARGETS)
                {
                    foreach (BaseQuestKillRequirement bqkr in quest.killReqs)
                    {
                        if (bqkr.targetsKilled < bqkr.quantity)
                        {
                            bqkr.waypoint.GetComponent<MapIcon>().type = MapIconType.Important;
                            bqkr.waypoint.GetComponent<MapIcon>().icon = questWaypoint;
                            bqkr.waypoint.GetComponent<MapIcon>().desc = "(" + quest.name + ") " + EnemyDB.instance.GetEnemy(bqkr.enemyID).name +
                                ": " + bqkr.targetsKilled + "/ " + bqkr.quantity;

                            bqkr.waypoint.GetComponent<CompassProPOI>().visibility = POI_VISIBILITY.AlwaysVisible;
                            bqkr.waypoint.GetComponent<CompassProPOI>().iconVisited = questWaypoint;
                            bqkr.waypoint.GetComponent<CompassProPOI>().iconNonVisited = questWaypoint;
                        }
                    }
                }
            }
        }

        public void HideWaypoint()
        {
            if (menu.questSelected)
            {
                BaseQuest quest = GetActiveQuest(menu.questSelectedID);

                foreach (BaseQuestBoolRequirement bqbr in quest.boolReqs)
                {
                    bqbr.waypoint.GetComponent<MapIcon>().type = MapIconType.Default;
                    bqbr.waypoint.GetComponent<MapIcon>().icon = null;
                    bqbr.waypoint.GetComponent<MapIcon>().title = "";
                    bqbr.waypoint.GetComponent<MapIcon>().desc = "";

                    bqbr.waypoint.GetComponent<CompassProPOI>().visibility = POI_VISIBILITY.AlwaysHidden;
                    bqbr.waypoint.GetComponent<CompassProPOI>().iconVisited = null;
                    bqbr.waypoint.GetComponent<CompassProPOI>().iconNonVisited = null;
                }
                foreach (BaseQuestGatherRequirement bqgr in quest.gatherReqs)
                {
                    bqgr.waypoint.GetComponent<MapIcon>().type = MapIconType.Default;
                    bqgr.waypoint.GetComponent<MapIcon>().icon = null;
                    bqgr.waypoint.GetComponent<MapIcon>().title = "";
                    bqgr.waypoint.GetComponent<MapIcon>().desc = "";

                    bqgr.waypoint.GetComponent<CompassProPOI>().visibility = POI_VISIBILITY.AlwaysHidden;
                    bqgr.waypoint.GetComponent<CompassProPOI>().iconVisited = null;
                    bqgr.waypoint.GetComponent<CompassProPOI>().iconNonVisited = null;
                }
                foreach (BaseQuestKillRequirement bqkr in quest.killReqs)
                {
                    bqkr.waypoint.GetComponent<MapIcon>().type = MapIconType.Default;
                    bqkr.waypoint.GetComponent<MapIcon>().icon = null;
                    bqkr.waypoint.GetComponent<MapIcon>().title = "";
                    bqkr.waypoint.GetComponent<MapIcon>().desc = "";

                    bqkr.waypoint.GetComponent<CompassProPOI>().visibility = POI_VISIBILITY.AlwaysHidden;
                    bqkr.waypoint.GetComponent<CompassProPOI>().iconVisited = null;
                    bqkr.waypoint.GetComponent<CompassProPOI>().iconNonVisited = null;
                }
            }
        }

        public BaseQuest GetQuest(int ID)
        {
            foreach (QuestDBEntry qdbe in QuestDB.instance.quests)
            {
                if (qdbe.ID == ID)
                {
                    return qdbe.quest;
                }
            }
            return null;
        }

        public BaseQuest GetActiveQuest(int ID)
        {
            foreach (BaseQuest quest in activeQuests)
            {
                if (quest.ID == ID)
                {
                    return quest;
                }
            }
            return null;
        }

        public BaseQuest GetCompletedQuest(int ID)
        {
            foreach (BaseQuest quest in completedQuests)
            {
                if (quest.ID == ID)
                {
                    return quest;
                }
            }
            return null;
        }

        public int GetActiveSideChild()
        {
            GameObject questList = GameObject.Find("GameManager/Menus/JournalMenuCanvas/MenuPanel" +
                "/QuestListPanel/QuestScroller/Viewport/QuestList");

            int childCount = questList.transform.childCount;

            for (int i = 0; i <= childCount; i++)
            {
                if (questList.transform.GetChild(i).name == "[SideHeader]")
                {
                    return i;
                }
            }

            return 0;
        }
    }

}