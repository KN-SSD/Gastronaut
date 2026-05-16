using UnityEngine;

public class PickUpAndHoldItem : MonoBehaviour
{
    [SerializeField] private Transform holderTransform;
    
    private bool isHoldingItem = false;
    private GameObject itemInRange = null; 
    private Transform heldItem = null;    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PickableCube") && !isHoldingItem)
            itemInRange = other.gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PickableCube") && other.gameObject == itemInRange)
            itemInRange = null; 
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isHoldingItem)
                DropItem();

            else if (itemInRange != null)
                PickUpItem(itemInRange);
        }
    }

    private void PickUpItem(GameObject item)
    {
        isHoldingItem = true;
        heldItem = item.transform;

        heldItem.SetParent(holderTransform);
        heldItem.localPosition = Vector3.zero;
        heldItem.localRotation = Quaternion.identity; 

        Rigidbody rb = heldItem.GetComponent<Rigidbody>();
        if (rb != null) 
        {
            rb.isKinematic = true; 
            rb.useGravity = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    private void DropItem()
    {
        isHoldingItem = false;
        
        Rigidbody rb = heldItem.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false; 
            rb.useGravity = true;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        heldItem.SetParent(null);
        heldItem = null; 
    }
}