using UnityEngine;

public class Fusion : MonoBehaviour
{
    public GameObject Explosion; // 🔹 Prefab de la animación de fusión

    public void PlayFusionEffect()
    {
        if (Explosion != null)
        {
            GameObject explosionInstance = Instantiate(Explosion, transform.position, Quaternion.identity);

            // 🔹 Obtener el tamaño del mineral
            Collider2D collider = GetComponent<Collider2D>();
            if (collider != null)
            {
                Vector2 mineralSize = collider.bounds.size; // Tamaño del mineral
                float scaleFactor = Mathf.Max(mineralSize.x, mineralSize.y) * 2f; // Factor de escala
                explosionInstance.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1f);
            }

            Destroy(explosionInstance, 0.3f); // 🔹 Destruir la explosión tras 0.3 segundos
        }
    }
}
