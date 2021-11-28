using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeepFry
{

    [System.Serializable]
    public class PlayerInventory : MonoBehaviour
    {
        [ReadOnly] public int gold;
        public List<BasePlayerItem> inventory;

        public delegate void OnItemchanged();
        public OnItemchanged onItemChangedCallback;

        public void Add(int itemID, bool isEquip)
        {
            if (itemID == 0 && !isEquip) //gold
            {
                gold = GetGold() + 1;
            }
            else
            {
                BasePlayerItem bpi = new BasePlayerItem();
                bpi.itemID = itemID;
                bpi.isEquip = isEquip;

                inventory.Add(bpi);

                //check for any gather quests and verify if item is needed in any of them.  if so, update vars accordingly
                CheckQuests(itemID);
            }
        }

        public void Remove(int itemID, bool isEquip)
        {
            //inventory.Reverse();

            if (itemID == 0 && !isEquip)
            {
                GameManager.instance.gold -= 1;
            }
            else
            {
                inventory.RemoveAt(InventoryIndex(itemID, isEquip));
                //inventory.Reverse();
            }

            if (onItemChangedCallback != null)
            {
                onItemChangedCallback.Invoke();
            }

            CheckQuests(itemID);
        }

        int InventoryIndex(int itemID, bool isEquip)
        {
            if (inventory.Count == 0)
            {
                return 0;
            }

            for (int i = 0; i <= inventory.Count; i++)
            {
                BasePlayerItem bpi = inventory[i];

                if (bpi.itemID == itemID && bpi.isEquip == isEquip)
                {
                    return i;
                }
            }

            return 0;
        }

        void CheckQuests(int itemToCheckID)
        {
            foreach (BaseQuest quest in QuestManager.instance.activeQuests)
            {
                if (quest.gatherReqs.Count > 0) //has gather requirements
                {
                    foreach (BaseQuestGatherRequirement bqgr in quest.gatherReqs)
                    {
                        if (itemToCheckID == bqgr.itemID) //item in question is indeed needed by active quest
                        {
                            //sets the variable "quest_ID_finished" to true or false based on if player has >= number of required items
                            string varName = "quest_" + quest.ID + "_finished";
                            SetDialogueSystemVariable(varName, ItemsInInventory(itemToCheckID) >= bqgr.quantity);
                        }
                    }
                }
            }
        }

        public int GetGold()
        {
            int gold = 0;
            foreach (BasePlayerItem bpi in inventory)
            {
                if (bpi.itemID == 0 && !bpi.isEquip)
                    gold++;
            }
            return gold;
        }

        int ItemsInInventory(int itemToCheckID)
        {
            int count = 0;
            foreach (BasePlayerItem bpi in inventory)
            {
                if (bpi.itemID == itemToCheckID)
                {
                    count++;
                }
            }

            return count;
        }

        void SetDialogueSystemVariable(string variableName, bool value)
        {
            Debug.Log("Setting " + variableName + " to " + value);
            DialogueLua.SetVariable(variableName, value);
        }

    }

}