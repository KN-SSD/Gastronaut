using UnityEngine;

[System.Serializable]
public class QuestReadyCondition : IDialogueCondition
{
    public string QuestNameForCheck { get; set; } = "Red Quest";

    public bool IsMet()
    {
        if (QuestManager.Instance == null)
            return false;

        var quest = QuestManager.Instance.GetQuestByName(QuestNameForCheck);
        bool ready = quest != null && quest.questState == QuestState.Ready;
        
        Debug.Log($"[QuestReadyCondition] {QuestNameForCheck}: {ready}");
        return ready;
    }
}
