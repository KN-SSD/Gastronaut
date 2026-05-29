using UnityEngine;

public class RotatePlanetInMenu : MonoBehaviour
{
    [Tooltip("The axis to rotate around. For example, set Y to 1 for a standard spin.")]
    public Vector3 rotationAxis = Vector3.up; 

    [Tooltip("Rotation speed in degrees per second.")]
    public float rotationSpeed = 20f;

    private void Update()
    {
        transform.Rotate(rotationAxis.normalized * (rotationSpeed * Time.deltaTime));
    }
}