using UnityEngine;

[System.Serializable]
public class QuestCompletedCondition : IDialogueCondition
{
    public string QuestNameForCheck { get; set; } = "Red Quest";

    public bool IsMet()
    {
        if (QuestManager.Instance == null)
            return false;

        var quest = QuestManager.Instance.GetQuestByName(QuestNameForCheck);
        bool completed = quest != null && quest.questState == QuestState.Completed;
        
        Debug.Log($"[QuestCompletedCondition] {QuestNameForCheck}: {completed}");
        return completed;
    }
}
