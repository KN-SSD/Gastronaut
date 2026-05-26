using UnityEngine;

public static class DialogueConditionFactory
{
    public static bool CheckCondition(DialogueConditionType conditionType)
    {
        switch (conditionType)
        {
            case DialogueConditionType.AlwaysTrue:
                return new AlwaysTrueCondition().IsMet();
            case DialogueConditionType.QuestStarted:
                return new QuestStartedCondition().IsMet();
            default:
                return true;
        }
    }
}
