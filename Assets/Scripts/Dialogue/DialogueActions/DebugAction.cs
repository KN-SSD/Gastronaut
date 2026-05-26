using UnityEngine;

/// <summary>
/// Testowa akcja do debugowania - wypisuje wiadomość.
/// </summary>
[System.Serializable]
public class DebugAction : IDialogueAction
{
    [SerializeField] private string message;

    public DebugAction(string msg = "Dialog zakończony!")
    {
        message = msg;
    }

    public void Execute()
    {
        Debug.Log($"[Dialog Action] {message}");
    }
}
