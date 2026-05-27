using UnityEngine;
using UnityEngine.SceneManagement;

public class NPCDialogue : MonoBehaviour
{
    [SerializeField] private Dialogue dialogue;
    private GameObject NPCInteractCanvas;
    [SerializeField] private Animator animator;

    private void Start()
    {
        Transform child = transform.Find("NPCInteractCanvas");
        animator = transform.GetComponentInChildren<Animator>();

        if (child != null)
        {
            NPCInteractCanvas = child.gameObject;
            NPCInteractCanvas.SetActive(false);
        }
        if (animator == null)
            Debug.LogWarning("Nie można znaleźć komponentu Animator w dzieciach obiektu NPC!");
    }

    void Update()
    {
        if(SceneManager.GetActiveScene().name != "Prolog")
        {
            if (NPCInteractCanvas.activeSelf && Input.GetKeyDown(KeyCode.E))
                TriggerDialogue();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            NPCInteractCanvas.SetActive(true);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            NPCInteractCanvas.SetActive(false);
    }

    public void TriggerDialogue()
    {
        if (dialogue == null)
            return;

        if (dialogue.GetVariants().Count == 0)
            return;

        DialogueManager.Instance.StartDialogue(dialogue, animator);
    }
}