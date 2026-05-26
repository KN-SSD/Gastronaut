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
            return;

        currentDialogue = dialogue;
        
        currentVariant = dialogue.GetActiveVariant();
        
        if (currentVariant == null)
            return;

        if (currentVariant.sentences == null || currentVariant.sentences.Length == 0)
            return;


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

        NextSentence();
    }

    public void NextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        dialogueText.text = sentence;
    }

    void EndDialogue()
    {
        if (currentVariant != null)
            DialogueActionFactory.ExecuteAction(currentVariant.actionType);

        dialoguePanel.SetActive(false);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        currentDialogue = null;
        currentVariant = null;
    }
}
