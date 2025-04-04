using UnityEngine;
using System.Collections;

public class ExplosionEvent : MonoBehaviour
{
    public float explosionForce = 1000f; // Fuerza de la explosión
    public float explosionRadius = 2f; // Radio de la explosión
    public float eventDuration = 3f; // Duración total del evento
    public LayerMask mineralLayer; // Capas de los minerales

    public AudioClip explosionSound; // Sonido de la explosión
    private AudioSource audioSource; // Componente de audio

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // 🟡 Registrar el AudioSource al SoundManager para control de mute
        if (audioSource != null)
        {
            SoundManager.Instance?.RegisterAudioSource(audioSource);
            audioSource.mute = SoundManager.Instance?.IsMuted() ?? false;
        }
    }

    public void TriggerExplosion()
    {
        // Obtener todos los minerales en la escena
        GameObject[] minerals = GameObject.FindGameObjectsWithTag("Mineral");

        if (minerals.Length == 0) return; // Si no hay minerales, salir

        // Seleccionar un mineral aleatorio
        GameObject selectedMineral = minerals[Random.Range(0, minerals.Length)];

        // 🔊 Reproducir sonido de explosión (solo si no está muteado)
        if (audioSource != null && explosionSound != null)
        {
            audioSource.PlayOneShot(explosionSound);
        }

        // Iniciar la cuenta regresiva para la explosión
        StartCoroutine(ExplosionCountdown(selectedMineral));
    }

    IEnumerator ExplosionCountdown(GameObject mineral)
    {
        SpriteRenderer mineralRenderer = mineral.GetComponent<SpriteRenderer>();
        if (mineralRenderer == null) yield break;

        float elapsedTime = 0f;
        Color originalColor = mineralRenderer.color;
        Color targetColor = Color.red;

        while (elapsedTime < eventDuration)
        {
            // Verificar si el mineral ha sido destruido antes de terminar la cuenta atrás
            if (mineral == null)
            {
                if (audioSource != null && audioSource.isPlaying)
                {
                    audioSource.Stop();
                }
                yield break;
            }

            mineralRenderer.color = Color.Lerp(originalColor, targetColor, elapsedTime / eventDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (mineral != null)
        {
            mineralRenderer.color = targetColor;
            Explode(mineral);
            Destroy(mineral);
        }
    }

    void Explode(GameObject mineral)
    {
        Vector2 explosionPosition = mineral.transform.position;
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(explosionPosition, explosionRadius, mineralLayer);

        foreach (Collider2D hit in hitColliders)
        {
            Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 direction = (hit.transform.position - (Vector3)explosionPosition).normalized;
                rb.AddForce(direction * explosionForce);
            }
        }
    }
}
