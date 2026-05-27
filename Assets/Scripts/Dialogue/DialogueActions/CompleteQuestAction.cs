using UnityEngine;

[System.Serializable]
public class CompleteQuestAction : IDialogueAction
{
    public string QuestNameForAction { get; set; } = "Red Quest";

    public void Execute()
    {
        if (QuestManager.Instance != null)
        {
            QuestManager.Instance.CompleteQuest(QuestNameForAction);
            Debug.Log($"[CompleteQuestAction] Quest '{QuestNameForAction}' został ukończony!");
        }
        else
        {
            Debug.LogWarning("QuestManager nie znaleziony!");
        }
    }
}
