using UnityEngine;

public class RandomEvents : MonoBehaviour
{
    private int lastScore = 0;
    private GameManager gameManager;
    private BouncyEffect bouncyEffect;
    private ExplosionEvent explosionEvent;
    private bool eventActive = false; // Indica si un evento est� en curso

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        bouncyEffect = FindObjectOfType<BouncyEffect>();
        explosionEvent = FindObjectOfType<ExplosionEvent>();

        if (gameManager != null)
        {
            lastScore = gameManager.CurrentScore;
        }
    }

    private void Update()
    {
        if (gameManager != null && gameManager.CurrentScore >= lastScore + 50 && !eventActive) // Solo si no hay evento activo
        {
            lastScore = gameManager.CurrentScore;
            TriggerRandomEvent();
        }
    }

    private void TriggerRandomEvent()
    {
        eventActive = true; // Bloquear nuevos eventos hasta que termine

        int randomEvent = Random.Range(0,2); // 0 = rebote, 1 = explosi�n

        if (randomEvent == 0 && bouncyEffect != null && !bouncyEffect.IsBouncyActive)
        {
            Debug.Log("Evento activado: Rebote alto por 10 segundos");
            bouncyEffect.ActivateBouncyEffect(10f);
            Invoke(nameof(ResetEvent), 10f); // Esperar 10 segundos antes de permitir otro evento
        }
        else if (randomEvent == 1 && explosionEvent != null)
        {
            Debug.Log("Evento activado: Explosi�n");
            explosionEvent.TriggerExplosion();
            Invoke(nameof(ResetEvent), 4f); // Esperar 3 segundos (aj�stalo si la explosi�n dura m�s)
        }
        else
        {
            // Si el evento no se activ�, permitir otro intento en la siguiente verificaci�n
            eventActive = false;
        }
    }

    private void ResetEvent()
    {
        eventActive = false; // Permitir un nuevo evento
    }
}
