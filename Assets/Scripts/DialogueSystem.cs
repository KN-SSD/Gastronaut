using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    [SerializeField] private Dialogue dialogue;
    private GameObject NPCInteractCanvas;

    private void Start()
    {
        NPCInteractCanvas = GameObject.Find("NPCInteractCanvas");
        NPCInteractCanvas.SetActive(false);
    }

     void Update()
    {
        if(NPCInteractCanvas.activeSelf && Input.GetKeyDown(KeyCode.E))
            TriggerDialogue();
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

    private void TriggerDialogue()
    {
        DialogueManager.Instance.StartDialogue(dialogue);
    }
}