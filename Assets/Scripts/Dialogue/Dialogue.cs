using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class DialogueVariant
{
    [TextArea(3, 10)]
    public string[] sentences = new string[] { };
    
    [Tooltip("Typ warunku, który musi być spełniony, aby wyświetlić ten wariant")]
    public DialogueConditionType conditionType = DialogueConditionType.AlwaysTrue;
    
    [Tooltip("Typ akcji do wykonania po zakończeniu dialogu")]
    public DialogueActionType actionType = DialogueActionType.None;
}

[System.Serializable]
public class Dialogue
{
    public string npcName;
    
    [SerializeField]
    private List<DialogueVariant> variants = new List<DialogueVariant>();
    
    public List<DialogueVariant> GetVariants()
    {
        if (variants == null)
            variants = new List<DialogueVariant>();

        return variants;
    }
    
    public DialogueVariant GetActiveVariant()
    {
        if (variants == null || variants.Count == 0)
            return null;

        for (int i = variants.Count - 1; i >= 0; i--)
        {
            var variant = variants[i];
            
            if (variant == null)
                continue;
                
            if (DialogueConditionFactory.CheckCondition(variant.conditionType))
                return variant;
        }
        
        return null;
    }
}

public enum DialogueConditionType
{
    AlwaysTrue,      
    QuestStarted,    
}

public enum DialogueActionType
{
    None,            
    StartQuest,      
}