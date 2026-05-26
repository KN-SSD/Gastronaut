using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private GameObject dialoguePanel;

    private Queue<string> sentences;
    private Dialogue currentDialogue;
    private DialogueVariant currentVariant;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        if (dialogue == null)
        {
            Debug.LogError("Próba rozpoczęcia dialogu z null!");
            return;
        }

        currentDialogue = dialogue;
        
        currentVariant = dialogue.GetActiveVariant();
        
        if (currentVariant == null)
        {
            Debug.LogError("Brak dostępnego wariantu dialogu!");
            return;
        }

        if (currentVariant.sentences == null || currentVariant.sentences.Length == 0)
        {
            Debug.LogError($"Wariant dla '{dialogue.npcName}' ma puste sentences!");
            return;
        }

        Debug.Log($"[DialogueManager] Rozpoczynam dialog z '{dialogue.npcName}'. Zdań: {currentVariant.sentences.Length}");

        dialoguePanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        nameText.text = dialogue.npcName;
        Time.timeScale = 0;

        sentences.Clear();

        foreach (string sentence in currentVariant.sentences)
        {
            sentences.Enqueue(sentence);
        }

        Debug.Log($"[DialogueManager] Queue ma {sentences.Count} zdań");
        NextSentence();
    }

    public void NextSentence()
    {
        if (sentences.Count == 0)
        {
            Debug.Log("[DialogueManager] Koniec zdań, zamykam dialog");
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        Debug.Log($"[DialogueManager] Wyświetlam: {sentence} (pozostało: {sentences.Count})");
        dialogueText.text = sentence;
    }

    void EndDialogue()
    {
        if (currentVariant != null)
        {
            Debug.Log($"[DialogueManager] Wykonuję akcję: {currentVariant.actionType}");
            DialogueActionFactory.ExecuteAction(currentVariant.actionType, currentVariant.questName);
        }

        dialoguePanel.SetActive(false);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        currentDialogue = null;
        currentVariant = null;
    }
}
