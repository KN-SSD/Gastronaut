using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using Unity.Cinemachine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private GameObject dialoguePanel;

    private GameObject camObject = null;
    GameObject playerObject = null;
    private Queue<string> sentences;
    private Dialogue currentDialogue;
    private DialogueVariant currentVariant;
    private Animator npcAnimator;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        sentences = new Queue<string>();
        camObject = GameObject.FindGameObjectWithTag("CinemamachineCamera");
        playerObject = GameObject.FindGameObjectWithTag("Player");
    }

    public void StartDialogue(Dialogue dialogue, Animator npcAnimator)
    {
        this.npcAnimator = npcAnimator;

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

        sentences.Clear();

        foreach (string sentence in currentVariant.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisableCinemachineAxes();

        Debug.Log($"[DialogueManager] Queue ma {sentences.Count} zdań");
        npcAnimator.SetBool("Talk", true);
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

        EnableCinemachineAxes();
        npcAnimator.SetBool("Talk", false);

        currentDialogue = null;
        currentVariant = null;
    }

    private void DisableCinemachineAxes()
    {
        if (playerObject != null)
        {
            PlayerMovement playerMovement = playerObject.GetComponent<PlayerMovement>();

            if (playerMovement != null)
                playerMovement.StopMovementAndRotation();
            else
                Debug.LogWarning("Nie można znaleźć komponentu PlayerMovement na obiekcie gracza!");
        }

        if (camObject != null)
        {
            var inputController = camObject.GetComponent<CinemachineInputAxisController>();

            if (inputController != null)
            {
                foreach (var axis in inputController.Controllers)
                {
                    if (axis.Name == "Look Orbit X" || axis.Name == "Look Orbit Y")
                        axis.Enabled = false;
                }
            }
        }
    }

    private void EnableCinemachineAxes()
    {
        if (playerObject != null)
        {
            PlayerMovement playerMovement = playerObject.GetComponent<PlayerMovement>();

            if (playerMovement != null)
                playerMovement.StartMovementAndRotation();
        }

        if (camObject != null)
        {
            var inputController = camObject.GetComponent<CinemachineInputAxisController>();

            if (inputController != null)
            {
                foreach (var axis in inputController.Controllers)
                {
                    if (axis.Name == "Look Orbit X" || axis.Name == "Look Orbit Y")
                        axis.Enabled = true;
                }
            }
        }
    }
}