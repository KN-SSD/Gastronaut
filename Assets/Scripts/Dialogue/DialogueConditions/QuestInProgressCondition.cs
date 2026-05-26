using UnityEngine;

[System.Serializable]
public class QuestInProgressCondition : IDialogueCondition
{
    public string QuestNameForCheck { get; set; } = "Red Quest";

    public bool IsMet()
    {
        if (QuestManager.Instance == null || QuestManager.Instance.quests.Count == 0)
            return false;

        var quest = QuestManager.Instance.GetQuestByName(QuestNameForCheck);
        bool inProgress = quest != null && quest.questState == QuestState.InProgress;
        
        Debug.Log($"[QuestInProgressCondition] {QuestNameForCheck}: {inProgress}");
        return inProgress;
    }
}
