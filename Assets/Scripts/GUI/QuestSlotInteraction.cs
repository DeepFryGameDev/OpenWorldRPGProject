using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DeepFry
{
    public class QuestSlotInteraction : MonoBehaviour
    {
        [ReadOnly] public BaseQuest thisQuest;
        [ReadOnly] public int questID;

        [ReadOnly] public bool activeQuest;


        private static GameMenu GAMEMENU;
        private static GameManager GAMEMANAGER;

        private void Awake()
        {
            GAMEMENU = GameObject.Find("GameManager/Menus").GetComponent<GameMenu>();

            GAMEMANAGER = GameObject.Find("GameManager").GetComponent<GameManager>();
        }

        public void OnEnter()
        {
            GAMEMENU.QUESTDESCTEXT.text = QuestManager.instance.GetActiveQuest(questID).description;
            GAMEMENU.QUESTLEVELTEXT.text = QuestManager.instance.GetActiveQuest(questID).level.ToString();
            GAMEMENU.QUESTOBJTEXT.text = SetThisObjectivesText();
            SetRewardsFields();
        }

        public void OnExit()
        {
            if (!GAMEMENU.questSelected)
            {
                //display nothing
                GAMEMENU.QUESTDESCTEXT.text = "";
                GAMEMENU.QUESTLEVELTEXT.text = "";
                GAMEMENU.QUESTOBJTEXT.text = "";
                GAMEMENU.REWARDEXPTEXT.text = "";
                GAMEMENU.REWARDGOLDTEXT.text = "";
                Color hideIcon = new Color(GAMEMENU.REWARDITEMICON.color.r, GAMEMENU.REWARDITEMICON.color.g, GAMEMENU.REWARDITEMICON.color.b, 0);
                GAMEMENU.REWARDITEMICON.color = hideIcon;
                GAMEMENU.REWARDITEMICON.sprite = null;
                GAMEMENU.REWARDITEMTEXT.text = "";

            }
            else
            {
                //display selected quest info
                GAMEMENU.QUESTDESCTEXT.text = QuestManager.instance.GetActiveQuest(GAMEMENU.questSelectedID).description;
                GAMEMENU.QUESTLEVELTEXT.text = QuestManager.instance.GetActiveQuest(GAMEMENU.questSelectedID).level.ToString();
                GAMEMENU.QUESTOBJTEXT.text = GAMEMENU.SetObjectivesText();
                SetRewardsFields();
            }
        }

        string SetThisObjectivesText()
        {
            string fullText = "";

            BaseQuest thisQuest;

            thisQuest = QuestManager.instance.GetActiveQuest(questID);

            if (thisQuest.type == BaseQuest.types.BOOLEAN)
            {
                foreach (BaseQuestBoolRequirement bqbr in thisQuest.boolReqs)
                {
                    fullText = fullText + bqbr.objectiveDescription + " - ";
                    if (bqbr.objectiveFulfilled)
                    {
                        fullText = fullText + "Completed";
                    }
                    else
                    {
                        fullText = fullText + "In Progress";
                    }
                    fullText = fullText + "\n";
                }
            }
            else if (thisQuest.type == BaseQuest.types.GATHER)
            {
                foreach (BaseQuestGatherRequirement bqgr in thisQuest.gatherReqs)
                {
                    fullText = fullText + ItemDB.instance.GetItem(bqgr.itemID).item.name + ": " + bqgr.inInventory + "/" + bqgr.quantity + "\n";
                }
            }
            else if (thisQuest.type == BaseQuest.types.KILLTARGETS)
            {
                foreach (BaseQuestKillRequirement bqkr in thisQuest.killReqs)
                {
                    fullText = fullText + EnemyDB.instance.GetEnemy(bqkr.enemyID).name + ": " + bqkr.targetsKilled + "/" + bqkr.quantity + "\n";
                }
            }

            if (fullText.Length == 0)
            {
                fullText = QuestManager.instance.GetActiveQuest(questID).completedText;
            }

            return fullText;
        }

        void SetRewardsFields()
        {
            BaseQuest quest = QuestManager.instance.GetActiveQuest(questID);

            GAMEMENU.REWARDEXPTEXT.text = quest.rewardExp.ToString();
            GAMEMENU.REWARDGOLDTEXT.text = quest.rewardGold.ToString();

            if (quest.rewardItems.Count > 0)
            {
                //later add code here to cycle the items. for now it just shows the first one.

                Color showIcon = new Color(GAMEMENU.REWARDITEMICON.color.r, GAMEMENU.REWARDITEMICON.color.g, GAMEMENU.REWARDITEMICON.color.b, 1);
                GAMEMENU.REWARDITEMICON.color = showIcon;

                GAMEMENU.REWARDITEMICON.sprite = quest.rewardItems[0].item.icon;
                GAMEMENU.REWARDITEMTEXT.text = quest.rewardItems[0].item.name;
            }
            else
            {
                Color hideIcon = new Color(GAMEMENU.REWARDITEMICON.color.r, GAMEMENU.REWARDITEMICON.color.g, GAMEMENU.REWARDITEMICON.color.b, 0);
                GAMEMENU.REWARDITEMICON.color = hideIcon;

                GAMEMENU.REWARDITEMICON.sprite = null;
                GAMEMENU.REWARDITEMTEXT.text = "";
            }
        }

        public void OnClick()
        {
            if ((GAMEMANAGER.questSelected && GAMEMANAGER.selectedQuest != questID) || !GAMEMANAGER.questSelected)
            {
                if (GAMEMANAGER.questSelected)
                {
                    QuestManager.instance.HideWaypoint();
                }

                GAMEMENU.questSelected = true;
                GAMEMENU.questButtonSelected = gameObject;
                GAMEMENU.questSelectedID = questID;
                GAMEMANAGER.questSelected = true;
                GAMEMANAGER.selectedQuest = questID;

                UnboldObjects();

                gameObject.transform.Find("NameText").GetComponent<Text>().fontStyle = FontStyle.Bold;
                gameObject.transform.Find("LevelText").GetComponent<Text>().fontStyle = FontStyle.Bold;

                QuestManager.instance.ShowWaypoint();

            }
            else if (GAMEMANAGER.questSelected && GAMEMANAGER.selectedQuest == questID)
            {
                QuestManager.instance.HideWaypoint();

                GAMEMENU.questSelected = false;
                GAMEMENU.questButtonSelected = null;
                GAMEMENU.questSelectedID = 0;
                GAMEMANAGER.questSelected = false;
                GAMEMANAGER.selectedQuest = 0;

                gameObject.transform.Find("NameText").GetComponent<Text>().fontStyle = FontStyle.Normal;
                gameObject.transform.Find("LevelText").GetComponent<Text>().fontStyle = FontStyle.Normal;
            }
        }

        void UnboldObjects()
        {
            foreach (Transform child in GameObject.Find("JournalMenuCanvas/MenuPanel/QuestListPanel/QuestScroller/Viewport/Content").transform)
            {
                if (!child.gameObject.name.StartsWith("["))
                {
                    child.transform.Find("NameText").GetComponent<Text>().fontStyle = FontStyle.Normal;
                    child.transform.Find("LevelText").GetComponent<Text>().fontStyle = FontStyle.Normal;
                }
            }
        }
    }
}
