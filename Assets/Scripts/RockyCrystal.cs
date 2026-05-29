using UnityEngine;

public class RockyCrystal : MonoBehaviour
{
    [SerializeField] private GameObject rockPrefab;
    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Rock"))
        {
            Instantiate(rockPrefab, transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
            Destroy(gameObject);

        }
    }
}
