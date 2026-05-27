using UnityEngine;
using System.Collections;

public class FlyingCharacter : MonoBehaviour
{
    [Header("Ustawienia Lotu")]
    [Tooltip("Prędkość poruszania się obiektu")]
    public float moveSpeed = 5f;
    [Tooltip("Czas oczekiwania (w sekundach) między jednym a drugim lotem")]
    public float spawnInterval = 2f;
    [Tooltip("Odległość obiektu od kamery")]
    public float zDepth = 10f;
    
    [Tooltip("Szansa na lot w poziomie (lewo-prawo). 0.8 oznacza 80% szans na lot z boku, a 20% na lot z góry/dołu.")]
    [Range(0f, 1f)]
    public float horizontalFlightChance = 0.8f;

    [Header("Ustawienia Rotacji")]
    [Tooltip("Prędkość ciągłego obrotu na osi Z podczas lotu")]
    public float rotationSpeedZ = 180f;
    [Tooltip("Szansa na to, że obiekt obróci się o 180 stopni na osi X przed wylotem (poza ekranem)")]
    [Range(0f, 1f)]
    public float flipXChance = 0.5f;

    private Camera mainCamera;
    private bool isFlying = false;
    private Vector3 targetPosition;

    void Start()
    {
        mainCamera = Camera.main;
        
        // Ukryj obiekt na start daleko poza ekranem
        transform.position = new Vector3(1000f, 1000f, 0f); 
        
        // Rozpocznij cykl lotów
        StartCoroutine(FlightRoutine());
    }

    IEnumerator FlightRoutine()
    {
        while (true)
        {
            // Czekaj określony interwał X poza ekranem
            yield return new WaitForSeconds(spawnInterval);
            
            // Przygotuj nową trasę (w tym obroty X)
            SetupNewFlight();
            isFlying = true;

            // Czekaj, aż obiekt skończy lecieć
            while (isFlying)
            {
                yield return null;
            }
        }
    }

    void SetupNewFlight()
    {
        // 1. Reset rotacji na starcie
        transform.rotation = Quaternion.identity;
        
        // 2. Ewentualny obrót o 180 na osi X w tajemnicy (poza ekranem)
        if (Random.value < flipXChance)
        {
            transform.Rotate(180f, 0, 0, Space.World);
        }

        // 3. Losowanie krawędzi z uwzględnieniem preferencji lotu poziomego
        int edge;
        if (Random.value < horizontalFlightChance)
        {
            // Lot poziomy (0 = z lewej, 1 = z prawej)
            edge = Random.value < 0.5f ? 0 : 1;
        }
        else
        {
            // Lot pionowy (2 = z góry, 3 = z dołu)
            edge = Random.value < 0.5f ? 2 : 3;
        }

        Vector3 startViewport = Vector3.zero;
        Vector3 targetViewport = Vector3.zero;
        float offset = 0.2f; 

        zDepth = Random.Range(4f, 17f); 
        switch (edge)
        {
            case 0: // Z Lewej na Prawą
                startViewport = new Vector3(-offset, Random.Range(0f, 1f), zDepth);
                targetViewport = new Vector3(1f + offset, Random.Range(0f, 1f), zDepth);
                break;
            case 1: // Z Prawej na Lewą
                startViewport = new Vector3(1f + offset, Random.Range(0f, 1f), zDepth);
                targetViewport = new Vector3(-offset, Random.Range(0f, 1f), zDepth);
                break;
            case 2: // Z Góry na Dół
                startViewport = new Vector3(Random.Range(0f, 1f), 1f + offset, zDepth);
                targetViewport = new Vector3(Random.Range(0f, 1f), -offset, zDepth);
                break;
            case 3: // Z Dołu do Góry
                startViewport = new Vector3(Random.Range(0f, 1f), -offset, zDepth);
                targetViewport = new Vector3(Random.Range(0f, 1f), 1f + offset, zDepth);
                break;
        }

        // Przelicz pozycje
        transform.position = mainCamera.ViewportToWorldPoint(startViewport);
        targetPosition = mainCamera.ViewportToWorldPoint(targetViewport);
    }

    void Update()
    {
        if (!isFlying) return;

        // Lot w stronę celu
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Ciągły obrót po osi Z w trakcie lotu
        transform.Rotate(0, 0, rotationSpeedZ * Time.deltaTime, Space.Self);

        // Sprawdzenie, czy obiekt doleciał
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            isFlying = false;
            // Ukryj obiekt
            transform.position = new Vector3(1000f, 1000f, 0f);
        }
    }
}