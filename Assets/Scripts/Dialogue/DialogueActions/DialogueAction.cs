using UnityEngine;

/// <summary>
/// Interfejs dla modularnych akcji wykonywanych po zakończeniu dialogu.
/// Każda akcja powinna implementować tę interfejsem i definiować własną logikę wykonania.
/// </summary>
public interface IDialogueAction
{
    void Execute();
}
