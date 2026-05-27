using UnityEngine;

public class MagnetItem : MonoBehaviour
{
    [SerializeField] private float flySpeed = 15f;
    [SerializeField] private float playerReached = 0.5f;

    private Transform playerTransform;
    private bool isFlying = false;

    [SerializeField] private string questName;


    void Update()
    {
        FlyToPlayer();
    }

    private void FlyToPlayer()
    {
        if (isFlying && playerTransform != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, flySpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, playerTransform.position) < playerReached)
                if(QuestManager.Instance.AddQuestProgress(questName))
                    Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTransform = other.transform;
            isFlying = true;
        }
        if(other.CompareTag("Planet"))
            Destroy(gameObject);    
    }

    private void Collect(GameObject player)
    {
        PlayerInventory inventory = player.GetComponent<PlayerInventory>();

        if (inventory!=null)
            inventory.CollectedCollectables();
        
        Destroy(gameObject);
    }
}