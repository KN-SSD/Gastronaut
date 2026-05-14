using UnityEngine;

public class GravityAttractor : MonoBehaviour
{
    public static GravityAttractor Instance; 
    
    [SerializeField] private float gravityForce = -9.81f;

    private void Awake()
    {
        if (Instance == null) 
            Instance = this;
    }

    public void Attract(Rigidbody body)
    {
        Vector3 gravityDirection = (body.position - transform.position).normalized;
        body.AddForce(gravityDirection * gravityForce * body.mass);
    }
}