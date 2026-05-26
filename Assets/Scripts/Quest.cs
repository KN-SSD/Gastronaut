using UnityEngine;

/// <summary>
/// Stan questa
/// </summary>
public enum QuestState
{
    NotStarted,    // Quest jeszcze się nie rozpoczął
    InProgress,    // Quest w trakcie - brak wymaganych przedmiotów
    Ready,         // Quest gotów do oddania - gracz ma wszystkie przedmioty
    Completed      // Quest ukończony
}

/// <summary>
/// Klasa reprezentująca quest z informacjami o dawcy i odbiorcy
/// </summary>
public class Quest
{
    public string questName;
    public QuestState questState = QuestState.NotStarted;
    
    [SerializeField]
    public int currentProgress;
    public int requiredProgress;
    
    // Reference do NPC
    public string questGiverName;    // NPC który daje questę (np. Wanilla)
    public string questReceiverName; // NPC który odbiera questę (np. Companion)
    
    // Opis zadania
    public string questDescription;
    
    // Dla kompatybilności wstecznej
    public bool isQuestActive => questState == QuestState.InProgress || questState == QuestState.Ready;
    public bool isQuestCompleted => questState == QuestState.Completed;

    public Quest(string name, int requiredProgress, string giverName = "", string receiverName = "")
    {
        this.questName = name;
        this.requiredProgress = requiredProgress;
        this.currentProgress = 0;
        this.questGiverName = giverName;
        this.questReceiverName = receiverName;
        this.questDescription = $"Zbierz {requiredProgress} przedmiotów";
    }

    /// <summary>
    /// Zwraca true jeśli wszystkie przedmioty zostały zebrane
    /// </summary>
    public bool IsReady()
    {
        return currentProgress >= requiredProgress && questState == QuestState.InProgress;
    }

    /// <summary>
    /// Aktualizuje stan questa na podstawie postępu
    /// </summary>
    public void UpdateState()
    {
        if (questState == QuestState.NotStarted)
            return;

        if (questState == QuestState.Completed)
            return;

        if (currentProgress >= requiredProgress)
        {
            questState = QuestState.Ready;
            Debug.Log($"[Quest] {questName} jest gotów do oddania!");
        }
        else
        {
            questState = QuestState.InProgress;
        }
    }

    /// <summary>
    /// Oznacza questę jako ukończoną
    /// </summary>
    public void Complete()
    {
        questState = QuestState.Completed;
        Debug.Log($"[Quest] {questName} został ukończony!");
    }
}
