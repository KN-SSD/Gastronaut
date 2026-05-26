using UnityEngine;

/// <summary>
/// Warunek sprawdzający, czy WSZYSTKIE questy zostały ukończone.
/// </summary>
[System.Serializable]
public class AllQuestsCompletedCondition : IDialogueCondition
{
    public bool IsMet()
    {
        if (QuestManager.Instance == null || QuestManager.Instance.quests.Count == 0)
            return false;

        bool allCompleted = QuestManager.Instance.AreAllQuestsCompleted();
        Debug.Log($"[AllQuestsCompletedCondition] Wszystkie questy ukończone: {allCompleted}");
        return allCompleted;
    }
}
