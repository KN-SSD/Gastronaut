using UnityEngine;
using Unity.Cinemachine;
using System.Collections; 

public class WobblyThrow : MonoBehaviour
{
    [System.Serializable]
    public struct CameraProfile
    {
        public float FieldOfView;
        public Vector3 TargetOffset;
        public Vector2 ScreenPosition;
    }

    [SerializeField] private Rigidbody playerRigidbody;
    [SerializeField] private Transform rotatingModel;

    [Header("UI i Skrypty")]
    [SerializeField] private GameObject crosshairUI;
    [SerializeField] private PlayerMovement playerMovementScript; 
    [SerializeField] private float rotationSpeed = 25f;

    [Header("Mechanika Kamienia")]
    [SerializeField] private GameObject rock;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float throwForce = 40f; 
    [SerializeField] private float throwCooldown = 1.5f;
    [SerializeField] private float rockLifetime = 5f;
    [SerializeField] private LayerMask aimColliderMask = ~0; 

    [Header("Ustawienia Kamery")]
    [SerializeField] private CinemachineCamera virtualCamera;
    [SerializeField] private CameraProfile normalCameraState;
    [SerializeField] private CameraProfile aimingCameraState; 
    [SerializeField] private float transitionSpeed = 15f; 

    private CinemachineOrbitalFollow orbitalFollow;
    private CinemachineRotationComposer rotationComposer; 
    private bool isAiming = false;
    private bool isMovementLocked = false; 
    
    private float currentFOV;
    private Vector3 currentOffset;
    private Vector2 currentScreenPos;
    private float nextThrowTime = 0f;

    private Animator playerAnimator;

    void Start()
    {
         playerAnimator = GetComponentInChildren<Animator>();
        if (crosshairUI != null) crosshairUI.SetActive(false);
        
        if (virtualCamera != null)
        {
            orbitalFollow = virtualCamera.GetComponent<CinemachineOrbitalFollow>();
            rotationComposer = virtualCamera.GetComponent<CinemachineRotationComposer>();
            
            if (orbitalFollow != null && rotationComposer != null) 
            {
                currentFOV = normalCameraState.FieldOfView;
                currentOffset = normalCameraState.TargetOffset;
                currentScreenPos = normalCameraState.ScreenPosition;

                var lens = virtualCamera.Lens;
                lens.FieldOfView = currentFOV;
                virtualCamera.Lens = lens;

                orbitalFollow.TargetOffset = currentOffset;
                rotationComposer.TargetOffset = currentOffset;
                
                var comp = rotationComposer.Composition;
                comp.ScreenPosition = currentScreenPos;
                rotationComposer.Composition = comp;
            }
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) SetAiming(true);
        else if (Input.GetMouseButtonUp(1)) SetAiming(false);

        if (isAiming && Input.GetMouseButtonDown(0)) 
        {
            if (Time.time >= nextThrowTime)
            {
                StartCoroutine(ThrowRockStraight());
                nextThrowTime = Time.time + throwCooldown;
            }
        }

        HandleCameraTransition();
    }

    void LateUpdate()
    {
        if (isAiming) 
            LockModelRotationToCamera();
    }

    void SetAiming(bool state)
    {
        isAiming = state;
        if (crosshairUI != null) 
            crosshairUI.SetActive(state);

    }

    void LockModelRotationToCamera()
    {
        if (rotatingModel == null || playerRigidbody == null || Camera.main == null) return;

        Vector3 camForward = Camera.main.transform.forward;
        Vector3 playerUp = playerRigidbody.transform.up; 
        
        Vector3 projectedForward = Vector3.ProjectOnPlane(camForward, playerUp).normalized;

        if (projectedForward != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(projectedForward, playerUp);
            rotatingModel.rotation = Quaternion.Slerp(rotatingModel.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    void HandleCameraTransition()
    {
        if (virtualCamera == null || orbitalFollow == null || rotationComposer == null) return;

        CameraProfile targetProfile = isAiming ? aimingCameraState : normalCameraState;

        currentFOV = Mathf.Lerp(currentFOV, targetProfile.FieldOfView, Time.deltaTime * transitionSpeed);
        currentOffset = Vector3.Lerp(currentOffset, targetProfile.TargetOffset, Time.deltaTime * transitionSpeed);
        currentScreenPos = Vector2.Lerp(currentScreenPos, targetProfile.ScreenPosition, Time.deltaTime * transitionSpeed);

        var lens = virtualCamera.Lens;
        lens.FieldOfView = currentFOV;
        virtualCamera.Lens = lens;

        orbitalFollow.TargetOffset = currentOffset; 
        rotationComposer.TargetOffset = currentOffset;

        var comp = rotationComposer.Composition;
        comp.ScreenPosition = currentScreenPos;
        rotationComposer.Composition = comp;
    }

    private IEnumerator ThrowRockStraight()
    {
        if (rock == null || spawnPoint == null) 
            yield return null;

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        Vector3 targetPoint;

        if (Physics.Raycast(ray, out hit, 200f, aimColliderMask)) 
            targetPoint = hit.point;
        else 
            targetPoint = ray.GetPoint(100f); 

        Vector3 throwDirection = (targetPoint - spawnPoint.position).normalized;
        playerAnimator.SetTrigger("Throw");

        yield return new WaitForSeconds(0.5f);
        GameObject currentRock = Instantiate(rock, spawnPoint.position, Quaternion.identity);
        Rigidbody rb = currentRock.GetComponent<Rigidbody>();


        if (rb != null) 
        {
            rb.useGravity = false;
            rb.linearVelocity = throwDirection * throwForce; 
        }

        StartCoroutine(LockMovementRoutine());

        Destroy(currentRock, rockLifetime);
    }

    private IEnumerator LockMovementRoutine()
    {
        isMovementLocked = true;

        if (playerMovementScript != null) 
            playerMovementScript.StopMovementAndRotation();

        // Czekamy równą 1 sekundę
        yield return new WaitForSeconds(1.5f); 

        if (playerMovementScript != null) 
            playerMovementScript.StartMovementAndRotation(); 

        isMovementLocked = false;
    }
}