using UnityEngine;

public class RisingGround : MonoBehaviour
{
    public float riseSpeed = 0.1f; // Velocidad de subida del suelo con el tiempo
    public float fallSpeed = 1f; // Velocidad de bajada cuando sube la puntuación
    public float fallAmountPerPoint = 0.2f; // Cuánto baja por cada punto de puntuación
    public float maxFallDistance = 1f; // Distancia máxima que puede bajar en total
    public float minY = -4f; // **Límite mínimo de bajada** (ajústalo a lo que quieras)

    private float targetY; // La posición a la que el suelo debe moverse
    private GameManager gameManager; // Referencia al GameManager
    private int lastScore; // Puntuación anterior

    void Start()
    {
        targetY = transform.position.y; // Inicializar la posición objetivo
        gameManager = FindObjectOfType<GameManager>();

        if (gameManager == null)
        {
            Debug.LogError("❌ No se encontró el GameManager en la escena.");
        }
    }

    void Update()
    {
        // Subir el suelo con el tiempo
        targetY += riseSpeed * Time.deltaTime;

        // Si la puntuación ha aumentado, bajar el suelo proporcionalmente
        if (gameManager != null && gameManager.CurrentScore > lastScore)
        {
            float fallAmount = (gameManager.CurrentScore - lastScore) * fallAmountPerPoint;
            targetY -= fallAmount;
            lastScore = gameManager.CurrentScore; // Actualizar la puntuación anterior
        }

        // **Limitar la bajada para que no baje más del límite mínimo**
        targetY = Mathf.Clamp(targetY, minY, transform.position.y + maxFallDistance);

        // Mover el suelo suavemente hacia la posición objetivo
        transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, targetY, fallSpeed * Time.deltaTime), transform.position.z);
    }
}
