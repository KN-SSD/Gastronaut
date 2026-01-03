using UnityEngine;

public class Portal : MonoBehaviour
{
    GameObject teleportPanel;

    void Start()
    {
        teleportPanel = GameObject.Find("TeleportPanel");
        teleportPanel.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            teleportPanel.SetActive(true);
        }
    }
}
