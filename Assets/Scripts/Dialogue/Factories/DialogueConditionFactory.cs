using UnityEngine;

public static class DialogueConditionFactory
{
    public static bool CheckCondition(DialogueConditionType conditionType, string questName = "")
    {
        switch (conditionType)
        {
            case DialogueConditionType.AlwaysTrue:
                return new AlwaysTrueCondition().IsMet();
            case DialogueConditionType.QuestStarted:
                return new QuestStartedCondition().IsMet();
            case DialogueConditionType.QuestInProgress:
                return new QuestInProgressCondition() { QuestNameForCheck = questName }.IsMet();
            case DialogueConditionType.QuestReady:
                return new QuestReadyCondition() { QuestNameForCheck = questName }.IsMet();
            case DialogueConditionType.QuestCompleted:
                return new QuestCompletedCondition() { QuestNameForCheck = questName }.IsMet();
            case DialogueConditionType.AllQuestsCompleted:
                return new AllQuestsCompletedCondition().IsMet();
            default:
                return true;
        }
    }
}
