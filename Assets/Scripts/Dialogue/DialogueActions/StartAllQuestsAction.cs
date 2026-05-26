using UnityEngine;

/// <summary>
/// Akcja uruchamiająca wszystkie questy (nadal potrzebna kompatybilność)
/// </summary>
[System.Serializable]
public class StartAllQuestsAction : IDialogueAction
{
    public void Execute()
    {
        if (QuestManager.Instance != null)
        {
            QuestManager.Instance.StartAllQuests();
            Debug.Log("[StartAllQuestsAction] Wszystkie questy zostały uruchomione!");
        }
        else
        {
            Debug.LogWarning("QuestManager nie znaleziony!");
        }
    }
}
