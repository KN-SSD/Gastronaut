using UnityEngine;

[System.Serializable]
public class AddToPartyAction : IDialogueAction
{
    [SerializeField] private string characterName;

    public AddToPartyAction(string name = "NPC")
    {
        characterName = name;
    }

    public void Execute()
    {
        Debug.Log($"{characterName} dołączył do drużyny!");
    }
}
