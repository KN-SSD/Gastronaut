using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityBody : MonoBehaviour
{
    private Rigidbody rb;
    
    [SerializeField] private float maxSpeed = 2f; 

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; 
    }

    private void FixedUpdate()
    {
        if (GravityAttractor.Instance != null && !rb.isKinematic)
            GravityAttractor.Instance.Attract(rb);

        if (!rb.isKinematic)
        {
            if (rb.linearVelocity.magnitude > maxSpeed)
                rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, maxSpeed);
        }
    }
}