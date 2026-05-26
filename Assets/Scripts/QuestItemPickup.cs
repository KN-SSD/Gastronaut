using UnityEngine;

public class QuestItemPickup : MonoBehaviour
{
    public string questName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(QuestManager.Instance.AddQuestProgress(questName))
            {
                Destroy(gameObject);
            }
        }
    }
}
