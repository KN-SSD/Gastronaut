using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// Akcja kończąca poziom lub pokazująca specjalny komunikat
/// </summary>
[System.Serializable]
public class LevelCompleteAction : IDialogueAction
{
    [SerializeField] private string message = "Dziękuję za pomoc! Poziom ukończony!";

    public void Execute()
    {
        Debug.Log($"[LevelCompleteAction] {message}");
        // TODO: Implementuj logikę końca poziomu
        // LoadNextLevel, ShowCompletionScreen, itp.
        SceneManager.LoadScene("Menu");
    }
}
