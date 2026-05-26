using UnityEngine;

/// <summary>
/// Szablon dla nowych akcji dialogowych.
/// Skopiuj tę klasę i zmień logikę Execute() na swoją.
/// </summary>
[System.Serializable]
public class CustomDialogueAction : IDialogueAction
{
    // Dodaj pola do konfiguracji akcji w inspectorze (jeśli potrzebne)
    [SerializeField] private string description = "Niestandardowa akcja";

    public void Execute()
    {
        Debug.Log($"Wykonuję akcję: {description}");
        
        // TUTAJ WPISZ SWOJĄ LOGIKĘ
        // Przykład:
        // Animator animator = GetComponent<Animator>();
        // animator.SetTrigger("SomeAnimation");
    }
}
