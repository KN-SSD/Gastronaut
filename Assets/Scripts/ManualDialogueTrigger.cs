using UnityEngine;

public class ManualDialogueTrigger : MonoBehaviour
{
     private NPCDialogue NPCDialogue;
    void Start()
    {
        NPCDialogue = GetComponent<NPCDialogue>();
        NPCDialogue.TriggerDialogue();
    }

    
}
