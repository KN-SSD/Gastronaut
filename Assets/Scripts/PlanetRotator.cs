using UnityEngine;
using UnityEngine.InputSystem;

public class PlanetRotator : MonoBehaviour
{
    [Header("Ustawienia Ruchu")]
    public float rotationSpeed = 50f;
    public Transform cameraTransform;

    [Header("Ustawienia Kolizji")]
    public Transform playerTransform; // Przeciągnij tu obiekt Gracza
    public LayerMask obstacleLayer;   // Zaznacz tutaj warstwę "Obstacle"
    public float checkDistance = 1.0f; // Jak blisko ściany zatrzymać ruch (promień gracza + margines)

    [Header("Korekta kierunków")]
    public bool invertHorizontal = true;
    public bool invertVertical = true;

    void Update()
    {
        // --- 1. Odczyt Inputu ---
        float h = 0;
        float v = 0;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) h = -1;
            else if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) h = 1;

            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) v = -1;
            else if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) v = 1;
        }

        if (h == 0 && v == 0) return;

        // --- 2. Obliczanie wektora ruchu ---
        // (Tu jeszcze NIE obracamy, tylko liczymy gdzie CHCEMY iść)
        
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camForward.y = 0;
        camRight.y = 0;

        if (camForward.sqrMagnitude < 0.01f)
        {
            camForward = cameraTransform.up;
            camForward.y = 0;
        }

        camForward.Normalize();
        camRight.Normalize();

        // Wektor kierunku, w którym gracz "chce" iść
        // (Bez odwracania inputu - raycast musi iść w stronę ściany)
        Vector3 inputDir = (camForward * v + camRight * h).normalized;

        // --- 3. SPRAWDZANIE KOLIZJI (Nowość) ---
        // Strzelamy promieniem z klatki piersiowej gracza (player + 0.5f w górę)
        // w kierunku ruchu.
        Vector3 rayOrigin = playerTransform.position + Vector3.up * 0.5f;
        
        // Rysujemy linię w edytorze, żebyś widział jak to działa (tylko w Scene View)
        Debug.DrawRay(rayOrigin, inputDir * checkDistance, Color.red);

        if (Physics.Raycast(rayOrigin, inputDir, checkDistance, obstacleLayer))
        {
            // Trafiliśmy w ścianę! Przerywamy funkcję, nie obracamy planety.
            return; 
        }

        // --- 4. Aplikacja Ruchu (jeśli brak kolizji) ---
        
        // Tutaj dopiero odwracamy wartości dla obrotu planety
        if (invertHorizontal) h *= -1;
        if (invertVertical) v *= -1;

        // Przeliczamy moveDir dla obrotu (może być odwrócony)
        Vector3 rotMoveDir = (camForward * v + camRight * h).normalized;

        Vector3 rotationAxis = Vector3.Cross(Vector3.up, rotMoveDir);
        transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime, Space.World);
    }
}