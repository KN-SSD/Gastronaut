using UnityEngine;

[System.Serializable]
public class AlwaysTrueCondition : IDialogueCondition
{
    public bool IsMet()
    {
        return true;
    }
}
