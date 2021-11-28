using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseQuest
{
    [ReadOnly] public int ID;

    public string name;

    public GameObject questGiver;

    public List<int> dependentQuests = new List<int>();

    public enum types
    {
        GATHER,
        KILLTARGETS,
        BOOLEAN
    }
    public types type;

    public bool mainQuest;

    public int level;

    public string description;

    public string completedText;

    public List<BaseQuestGatherRequirement> gatherReqs = new List<BaseQuestGatherRequirement>();
    public List<BaseQuestKillRequirement> killReqs = new List<BaseQuestKillRequirement>();
    public List<BaseQuestBoolRequirement> boolReqs = new List<BaseQuestBoolRequirement>();

    public int rewardGold;
    public int rewardExp;

    public List<BaseItemReward> rewardItems = new List<BaseItemReward>();

    [ReadOnly] public bool fulfilled;

    public BaseQuest NewQuest()
    {
        BaseQuest copy = new BaseQuest();

        copy.ID = this.ID;

        copy.fulfilled = this.fulfilled;

        copy.name = this.name;

        copy.questGiver = this.questGiver;

        copy.dependentQuests = this.dependentQuests;

        copy.type = this.type;

        copy.mainQuest = this.mainQuest;

        copy.level = this.level;

        copy.description = this.description;

        copy.completedText = this.completedText;

        copy.gatherReqs = this.gatherReqs;
        copy.killReqs = this.killReqs;
        copy.boolReqs = this.boolReqs;

        copy.rewardGold = this.rewardGold;
        copy.rewardExp = this.rewardExp;

        copy.rewardItems = this.rewardItems;


        return copy;
    }
}
