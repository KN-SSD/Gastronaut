using UnityEngine;

public static class DialogueActionFactory
{
    public static void ExecuteAction(DialogueActionType actionType)
    {
        IDialogueAction action = null;
        
        switch (actionType)
        {
            case DialogueActionType.None:
                return;
                
            case DialogueActionType.StartQuest:
                action = new StartQuestAction();
                break;
                
            default:
                Debug.LogWarning($"Nieznany typ akcji: {actionType}");
                return;
        }
        
        if (action != null)
            action.Execute();
    }
}
