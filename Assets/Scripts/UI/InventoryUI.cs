using UnityEngine;
using TMPro;


public class InventoryUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI collectableTxt;

    public void updateCollectable(PlayerInventory playerInventory)
    {
        collectableTxt.text = playerInventory.numberOfCollectables.ToString();    
    }
}
