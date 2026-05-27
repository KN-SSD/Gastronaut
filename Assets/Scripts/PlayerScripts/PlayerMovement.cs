using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Ustawienia")]
    [SerializeField] private Transform planet;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform model;

    [SerializeField] private float gravity = 30f;
    [SerializeField] private float moveSpeed = 12f;
    [SerializeField] private float rotationSpeed = 15f;
    [SerializeField] private float jumpForce = 12f;
    
    [Header("Detekcja Ziemi")]
    [SerializeField] private LayerMask groundMask;

    private Rigidbody rb;
    private Vector3 moveInput;
    private Animator playerAnimator;

    private bool isGrounded;
    private Vector3 groundNormal;
    private float lastJumpTime;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        playerAnimator = GetComponentInChildren<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        moveInput = new Vector3(h, 0, v).normalized;

        if (Input.GetButtonDown("Jump") && isGrounded && Time.time - lastJumpTime > 0.2f)
        {
            Vector3 currentVel = rb.linearVelocity;
            float upSpeed = Vector3.Dot(currentVel, transform.up);
            rb.linearVelocity = currentVel - (transform.up * upSpeed);

            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            if (playerAnimator != null) playerAnimator.SetTrigger("Jump");
            lastJumpTime = Time.time;
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

        if (playerAnimator != null) playerAnimator.SetFloat("Speed", moveInput.magnitude);

        if(Input.GetKeyDown(KeyCode.G))
            playerAnimator.SetTrigger("Wave");
        if(Input.GetKeyDown(KeyCode.Y))
            playerAnimator.SetTrigger("Dance");
    }

    void FixedUpdate()
    {
        ApplyPlanetGravity();
        CheckGrounded();
        ApplyMovement();
    }

    private void ApplyPlanetGravity()
    {
        if (planet == null) return;
        Vector3 directionToCenter = (transform.position - planet.position).normalized;
        rb.AddForce(directionToCenter * gravity, ForceMode.Acceleration);

        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, directionToCenter) * transform.rotation;
        rb.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, 50f * Time.fixedDeltaTime));
    }

    private void CheckGrounded()
    {
        if (Time.time - lastJumpTime < 0.2f)
        {
            isGrounded = false;
            groundNormal = transform.up;
            return;
        }

        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, 1.3f, groundMask))
        {
            isGrounded = true;
            groundNormal = hit.normal;
        }
        else
        {
            isGrounded = false;
            groundNormal = transform.up;
        }
    }

    private void ApplyMovement()
    {
        Vector3 camFwd = Vector3.ProjectOnPlane(cameraTransform.forward, transform.up).normalized;
        Vector3 camRight = Vector3.ProjectOnPlane(cameraTransform.right, transform.up).normalized;
        Vector3 desiredDir = (camFwd * moveInput.z + camRight * moveInput.x).normalized;

        if (isGrounded)
        {
            Vector3 slopeMoveDir = Vector3.ProjectOnPlane(desiredDir, groundNormal).normalized;

            if (moveInput.sqrMagnitude > 0.01f)
                rb.linearVelocity = slopeMoveDir * moveSpeed;
            else
                rb.linearVelocity = Vector3.zero;
        }
        else
        {
            float currentVerticalSpeed = Vector3.Dot(rb.linearVelocity, transform.up);
            Vector3 verticalVelocity = transform.up * currentVerticalSpeed;

            Vector3 horizontalVelocity = desiredDir * moveSpeed;

            rb.linearVelocity = horizontalVelocity + verticalVelocity;
        }
    }

    public void StopMovementAndRotation()
    {
        moveSpeed = 0;
        rotationSpeed = 0;
    }

    public void StartMovementAndRotation()
    {
        moveSpeed = 8;
        rotationSpeed = 10;
    }
}