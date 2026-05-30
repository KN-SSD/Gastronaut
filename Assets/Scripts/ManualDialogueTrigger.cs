using UnityEngine;

public class ManualDialogueTrigger : MonoBehaviour
{
    [SerializeField] private GameObject dialogueWindow;
     private NPCDialogue NPCDialogue;
     [SerializeField] private bool epilog;
    void Start()
    {

        NPCDialogue = GetComponent<NPCDialogue>();
        if(epilog)
                Invoke("StartSceneDialogueInEpilog", 5f);
        else    
            Invoke("StartSceneDialogue", 0.01f);

    }

    private void StartSceneDialogue()
    {
        NPCDialogue.TriggerDialogue();
    }
    
    private void StartSceneDialogueInEpilog()
    {
        dialogueWindow.SetActive(true);
        NPCDialogue.TriggerDialogue();
    }

}
