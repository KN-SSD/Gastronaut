using UnityEngine;

[System.Serializable]
public class StartQuestAction : IDialogueAction
{
    public void Execute()
    {
        if (QuestManager.Instance != null)
            QuestManager.Instance.StartAllQuests();
        else
            Debug.LogWarning("QuestManager nie znaleziony!");
    }
}
