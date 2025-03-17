using UnityEngine;

public class RisingGround : MonoBehaviour
{
    public float riseSpeed = 0.5f; // Velocidad de subida del suelo
    public float fallSpeedMultiplier = 2f; // Cuánto más rápido baja al aumentar la puntuación
    public float minY = -5f; // Altura mínima del suelo (límite de bajada)

    private float initialY; // Para guardar la posición inicial del suelo
    private GameManager gameManager; // Referencia al GameManager
    private int lastScore; // Puntuación anterior

    void Start()
    {
        initialY = transform.position.y; // Guardamos la posición inicial del suelo
        gameManager = FindObjectOfType<GameManager>();

        if (gameManager == null)
        {
            Debug.LogError("❌ No se encontró el GameManager en la escena.");
        }
    }

    void Update()
    {
        // Hacer que el suelo suba poco a poco con el tiempo
        transform.position += Vector3.up * riseSpeed * Time.deltaTime;

        // Si la puntuación ha aumentado, bajar el suelo
        if (gameManager != null && gameManager.CurrentScore > lastScore)
        {
            float fallAmount = (gameManager.CurrentScore - lastScore) * fallSpeedMultiplier;
            transform.position -= Vector3.up * fallAmount;
            lastScore = gameManager.CurrentScore; // Actualizar la puntuación anterior
        }

        // Limitar la bajada del suelo
        if (transform.position.y < minY)
        {
            transform.position = new Vector3(transform.position.x, minY, transform.position.z);
        }
    }
}
