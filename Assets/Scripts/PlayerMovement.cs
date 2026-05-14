using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Ustawienia")]
    [SerializeField] private Transform planet; 
    [SerializeField] private Transform cameraTransform; 
    [SerializeField] private Transform model; 

    [SerializeField] private float gravity = -15f; 
    [SerializeField] private float moveSpeed = 12f;
    [SerializeField] private float rotationSpeed = 15f; 
    [SerializeField] private float jumpForce = 10f;

    private Rigidbody rb;
    private Vector3 moveInput;
    private Animator playerAnimator;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        playerAnimator = GetComponentInChildren<Animator>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        PlayerMove();
    }
    

    void FixedUpdate()
    {
       PlanetGravity();
    }

    private void PlayerMove()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        moveInput = new Vector3(h, 0, v).normalized;

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.linearVelocity = Vector3.ProjectOnPlane(rb.linearVelocity, transform.up);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            playerAnimator.SetTrigger("Jump");
        }

        if (moveInput.sqrMagnitude > 0.01f)
        {
            Vector3 camFwd = Vector3.ProjectOnPlane(cameraTransform.forward, transform.up).normalized;
            Vector3 camRight = Vector3.ProjectOnPlane(cameraTransform.right, transform.up).normalized;
            Vector3 targetMoveDir = (camFwd * moveInput.z + camRight * moveInput.x).normalized;

            if (model != null && targetMoveDir != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(targetMoveDir, transform.up);
                model.rotation = Quaternion.Slerp(model.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }

        if (playerAnimator != null)
            playerAnimator.SetFloat("Speed", moveInput.magnitude);
    }

    private void PlanetGravity()
    {
         if (planet == null) return;

        Vector3 directionToCenter = (transform.position - planet.position).normalized;
        
        rb.AddForce(directionToCenter * gravity, ForceMode.Acceleration);

        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, directionToCenter) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 50f * Time.fixedDeltaTime);

        
        Vector3 camFwd = Vector3.ProjectOnPlane(cameraTransform.forward, transform.up).normalized;
        Vector3 camRight = Vector3.ProjectOnPlane(cameraTransform.right, transform.up).normalized;
        Vector3 desiredDir = (camFwd * moveInput.z + camRight * moveInput.x).normalized;

        Vector3 targetVelocity = desiredDir * moveSpeed;

        Vector3 verticalVelocity = Vector3.Project(rb.linearVelocity, transform.up);

        rb.linearVelocity = targetVelocity + verticalVelocity;
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -transform.up, 1.2f);
    }
}