using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using Unity.Cinemachine;
using UnityEngine.SceneManagement;

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
    private string questReceiverName = "";

    private int nextSceneIndex;
    private bool shouldChangeSceneAfterThisDialogue = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        sentences = new Queue<string>();
        camObject = GameObject.FindGameObjectWithTag("CinemamachineCamera");
        playerObject = GameObject.FindGameObjectWithTag("Player");
    }

    public void StartDialogue(Dialogue dialogue, Animator npcAnimator, string questReceiverName = "")
    {
        this.npcAnimator = npcAnimator;
        this.questReceiverName = questReceiverName;

        shouldChangeSceneAfterThisDialogue = false;

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
            string processedSentence = ProcessSentence(sentence);
            sentences.Enqueue(processedSentence);
        }

        DisableCinemachineAxes();

        Debug.Log($"[DialogueManager] Queue ma {sentences.Count} zdań");

        if (npcAnimator != null)
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

        if (npcAnimator != null)
            npcAnimator.SetBool("Talk", false);

        currentDialogue = null;
        currentVariant = null;

        if (SceneManager.GetActiveScene().name == "Epilog")
        {
            LoadSceneAndWait(0);
        }
        else    
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            bool triggerSceneChange = false;
            if (currentSceneIndex > 0 && currentSceneIndex < 3)
                triggerSceneChange = true;
            else if (shouldChangeSceneAfterThisDialogue)
                triggerSceneChange = true;

            if (triggerSceneChange)
            {
                int targetSceneIndex = currentSceneIndex + 1;

                if (targetSceneIndex < SceneManager.sceneCountInBuildSettings)
                    LoadSceneAndWait(targetSceneIndex);
                else
                    Debug.LogWarning("[DialogueManager] Brak kolejnych scen w Build Settings");
            }
        }
    }

    public void LoadSceneAndWait(int sceneIndex)
    {
        nextSceneIndex = sceneIndex;

        float waitTime = 1f;

        CloseBlackscreenScript closeScript = FindFirstObjectByType<CloseBlackscreenScript>();
        if (closeScript != null)
        {
            waitTime = closeScript.GetFadeTime();
            closeScript.StartClosingScreen();
        }
        else
        {
            Debug.LogWarning("[DialogueManager] Nie znaleziono CloseBlackscreenScript na scenie!");
        }

        Invoke("ActivateSceneAfterDelay", waitTime);
    }

    private void ActivateSceneAfterDelay()
    {
        SceneManager.LoadScene(nextSceneIndex);
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

    private string ProcessSentence(string sentence)
    {
        if (string.IsNullOrEmpty(questReceiverName))
            return sentence;

        return sentence.Replace("{receiverName}", questReceiverName);
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

    public void MarkCurrentDialogueAsSceneChanger()
    {
        shouldChangeSceneAfterThisDialogue = true;
    }
}