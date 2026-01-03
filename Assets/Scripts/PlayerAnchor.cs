using UnityEngine;

public class PlayerAnchor : MonoBehaviour
{
    [Header("Ustawienia")]
    [Tooltip("Warstwa, na której jest planeta (dla Raycastu)")]
    public LayerMask planetLayer;
    
    [Tooltip("Wysokość gracza nad powierzchnią")]
    public float hoverHeight = 1.0f;
    
    [Tooltip("Jak szybko gracz dopasowuje wysokość")]
    public float heightSmoothSpeed = 10f;

    void Update()
    {
        // 1. Zablokuj pozycję X i Z gracza w centrum świata
        // Gracz może zmieniać tylko Y (wysokość)
        Vector3 currentPos = transform.position;
        currentPos.x = 0;
        currentPos.z = 0;

        // 2. Raycast w dół, aby znaleźć powierzchnię planety
        // Dzięki temu gracz nie przeniknie przez planetę ani nie odleci
        RaycastHit hit;
        // Strzelamy promieniem z góry (np. 100 jednostek) w dół
        if (Physics.Raycast(new Vector3(0, 100, 0), Vector3.down, out hit, 200f, planetLayer))
        {
            // Docelowa pozycja Y to punkt trafienia + offset
            float targetY = hit.point.y + hoverHeight;
            
            // Płynne dopasowanie wysokości
            currentPos.y = Mathf.Lerp(currentPos.y, targetY, Time.deltaTime * heightSmoothSpeed);
        }

        // Aplikuj pozycję
        transform.position = currentPos;

        // 3. Zablokuj rotację gracza
        // Gracz zawsze stoi pionowo "do góry"
        transform.rotation = Quaternion.identity;
    }
}