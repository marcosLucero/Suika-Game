using UnityEngine;

public class Fusion : MonoBehaviour
{
    public GameObject Explosion; // 🔹 Prefab de la animación de fusión

    public void PlayFusionEffect()
    {
        if (Explosion != null)
        {
            GameObject explosionInstance = Instantiate(Explosion, transform.position, Quaternion.identity);
            Destroy(explosionInstance, 0.3f); // 🔹 Destruir la explosión tras 1 segundo
        }
    }
}
