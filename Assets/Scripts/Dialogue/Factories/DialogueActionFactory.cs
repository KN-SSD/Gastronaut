using UnityEngine;

public static class DialogueActionFactory
{
    public static void ExecuteAction(DialogueActionType actionType, string questName = "")
    {
        IDialogueAction action = null;
        
        switch (actionType)
        {
            case DialogueActionType.None:
                return;
                
            case DialogueActionType.StartQuest:
                action = new StartQuestAction();
                break;
                
            case DialogueActionType.StartAllQuests:
                action = new StartAllQuestsAction();
                break;
                
            case DialogueActionType.CompleteQuest:
                action = new CompleteQuestAction() { QuestNameForAction = questName };
                break;
                
            case DialogueActionType.LevelComplete:
                action = new LevelCompleteAction();
                break;
                
            default:
                Debug.LogWarning($"Nieznany typ akcji: {actionType}");
                return;
        }
        
        if (action != null)
        {
            action.Execute();
        }
    }
}
