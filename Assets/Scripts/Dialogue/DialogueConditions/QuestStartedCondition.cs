using UnityEngine;

[System.Serializable]
public class QuestStartedCondition : IDialogueCondition
{
    public bool IsMet()
    {
        if (QuestManager.Instance == null)
            return false;

        if (QuestManager.Instance.quests == null || QuestManager.Instance.quests.Count == 0)
            return false;

        bool questActive = QuestManager.Instance.quests[0].isQuestActive;
        return questActive;
    }
}
