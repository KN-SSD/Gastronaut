using UnityEngine;

/// <summary>
/// Akcja dodająca postać do drużyny gracza.
/// </summary>
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
        // TODO: Implementuj logikę dodawania do drużyny
        // PartyManager.Instance.AddMember(characterName);
    }
}
