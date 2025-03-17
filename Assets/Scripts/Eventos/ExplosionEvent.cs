using UnityEngine;
using System.Collections;

public class ExplosionEvent : MonoBehaviour
{
    public float explosionForce = 500f; // Fuerza de la explosi�n
    public float explosionRadius = 2f; // Radio de la explosi�n
    public float warningTime = 2f; // Tiempo antes de la explosi�n
    public LayerMask mineralLayer; // Capas de los minerales

    public void TriggerExplosion()
    {
        // Obtener todos los minerales en la escena
        GameObject[] minerals = GameObject.FindGameObjectsWithTag("Mineral");

        if (minerals.Length == 0) return; // Si no hay minerales, salir

        // Seleccionar un mineral aleatorio
        GameObject selectedMineral = minerals[Random.Range(0, minerals.Length)];

        // Iniciar la cuenta regresiva para la explosi�n
        StartCoroutine(ExplosionCountdown(selectedMineral));
    }

    IEnumerator ExplosionCountdown(GameObject mineral)
    {
        SpriteRenderer mineralRenderer = mineral.GetComponent<SpriteRenderer>();
        if (mineralRenderer == null) yield break;

        float elapsedTime = 0f;
        Color originalColor = mineralRenderer.color;
        Color targetColor = Color.red;

        while (elapsedTime < warningTime)
        {
            // Cambiar color poco a poco a rojo
            mineralRenderer.color = Color.Lerp(originalColor, targetColor, elapsedTime / warningTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Asegurar que el color sea rojo al explotar
        mineralRenderer.color = targetColor;

        // Aplicar explosi�n
        Explode(mineral);
    }

    void Explode(GameObject mineral)
    {
        Vector2 explosionPosition = mineral.transform.position;

        // Detectar minerales cercanos
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(explosionPosition, explosionRadius, mineralLayer);

        foreach (Collider2D hit in hitColliders)
        {
            Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Aplicar una fuerza de explosi�n
                Vector2 direction = (hit.transform.position - (Vector3)explosionPosition).normalized;
                rb.AddForce(direction * explosionForce);
            }
        }

        // Destruir el mineral que explot�
        Destroy(mineral);

        // (Opcional) Agregar efectos visuales de explosi�n aqu�
    }
}
