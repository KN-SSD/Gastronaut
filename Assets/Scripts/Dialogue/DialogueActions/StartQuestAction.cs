using UnityEngine;

/// <summary>
/// Akcja startująca wszystkie dostępne questy.
/// </summary>
[System.Serializable]
public class StartQuestAction : IDialogueAction
{
    public void Execute()
    {
        if (QuestManager.Instance != null)
        {
            Debug.Log($"[StartQuestAction] Przed: Quest[0].isQuestActive = {(QuestManager.Instance.quests.Count > 0 ? QuestManager.Instance.quests[0].isQuestActive : "N/A")}");
            
            QuestManager.Instance.StartQuestes();
            
            Debug.Log($"[StartQuestAction] Po: Quest[0].isQuestActive = {(QuestManager.Instance.quests.Count > 0 ? QuestManager.Instance.quests[0].isQuestActive : "N/A")}");
            Debug.Log("Questy zostały uruchomione!");
        }
        else
        {
            Debug.LogWarning("QuestManager nie znaleziony!");
        }
    }
}
