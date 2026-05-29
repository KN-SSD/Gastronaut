using UnityEngine;

public class MagnetItem : MonoBehaviour
{
    [SerializeField] private float flySpeed = 15f;
    [SerializeField] private float playerReached = 0.5f;

    private Transform playerTransform;
    private bool isFlying = false;

    [SerializeField] private string questName;

    [SerializeField] private GameObject collectVFXPrefab;
    [SerializeField] private float vfxDestroyDelay = 2f;


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
            {
                Transform backpackBone = playerTransform.Find("mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1");

                if (backpackBone == null)
                {
                    Transform[] allChildren = playerTransform.GetComponentsInChildren<Transform>();
                    foreach (Transform child in allChildren)
                    {
                        if (child.name == "mixamorig:RightShoulder")
                        {
                            backpackBone = child;
                            break;
                        }
                    }
                }

                if (collectVFXPrefab != null)
                {
                    GameObject spawnedVFX;

                    if (backpackBone != null)
                    {
                        spawnedVFX = Instantiate(collectVFXPrefab, backpackBone.position, backpackBone.rotation, backpackBone);
                        spawnedVFX.transform.localPosition = Vector3.zero;
                    }
                    else
                       spawnedVFX = Instantiate(collectVFXPrefab, playerTransform.position + Vector3.up * 1f, playerTransform.rotation);


                    Destroy(spawnedVFX, vfxDestroyDelay);
                }

                if (QuestManager.Instance.AddQuestProgress(questName))
                    Destroy(gameObject);
            }
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