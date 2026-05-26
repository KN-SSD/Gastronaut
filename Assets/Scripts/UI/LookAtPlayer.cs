using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Transform cameraTransform;

    void Start()
    {
        GameObject cameraObj = GameObject.FindGameObjectWithTag("MainCamera");
        if (cameraObj != null)
            cameraTransform = cameraObj.transform;
        else
            Debug.LogError("Nie znaleziono kamery z tagiem 'MainCamera'!");
    }

    void Update()
    {
        if (cameraTransform != null && transform.parent != null)
        {
            Vector3 directionToCamera = cameraTransform.position - transform.position;

            Vector3 projectedDirection = Vector3.ProjectOnPlane(directionToCamera, transform.parent.up);

            projectedDirection = -projectedDirection;

            if (projectedDirection != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(projectedDirection, transform.parent.up);
        }
    }
}