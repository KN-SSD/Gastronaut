using UnityEngine;

[System.Serializable]
public class StartQuestAction : IDialogueAction
{
    public void Execute()
    {
        if (QuestManager.Instance != null)
            QuestManager.Instance.StartQuestes();
        else
            Debug.LogWarning("QuestManager nie znaleziony!");
    }
}
