using UnityEngine;
using UnityEngine.Events;

public class PlayerInventory : MonoBehaviour
{
    public int numberOfCollectables { get; private set; } = 0;

    [SerializeField] private UnityEvent<PlayerInventory> onCollectableGet;

    public void CollectedCollectables()
    {
        numberOfCollectables++;
        Debug.Log("Krysztly: " + numberOfCollectables);

        if (onCollectableGet!=null)
            onCollectableGet.Invoke(this);
       
    }
}
