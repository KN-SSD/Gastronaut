using UnityEngine;

[System.Serializable]
public class CustomDialogueAction : IDialogueAction
{
    [SerializeField] private string description = "Niestandardowa akcja";

    public void Execute()
    {
    }
}
