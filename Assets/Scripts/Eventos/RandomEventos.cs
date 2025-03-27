using UnityEngine;

public class RandomEvents : MonoBehaviour
{
    private int lastScore = 0;
    private GameManager gameManager;
    private BouncyEffect bouncyEffect;
    private ExplosionEvent explosionEvent;
    private GhostModeEvent ghostModeEvent;
    private VanishingEvent vanishingEvent;
    private AutoThrowEvent autoThrowEvent;
    private bool eventActive = false; // Indica si un evento está en curso
    private SpeedUpEvent speedUpEvent;


    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        bouncyEffect = FindObjectOfType<BouncyEffect>();
        explosionEvent = FindObjectOfType<ExplosionEvent>();
        ghostModeEvent = FindObjectOfType<GhostModeEvent>();
        vanishingEvent = FindObjectOfType<VanishingEvent>();
        speedUpEvent = FindObjectOfType<SpeedUpEvent>();
        autoThrowEvent = FindObjectOfType<AutoThrowEvent>();

        if (gameManager != null)
        {
            lastScore = gameManager.CurrentScore;
        }
    }

    private void Update()
    {
        // 🛑 No activar eventos si hay uno en curso
        if (eventActive || gameManager == null) return;

        if (gameManager.CurrentScore >= lastScore + 50)
        {
            TriggerRandomEvent();
        }
    }

    private void TriggerRandomEvent()
    {
        eventActive = true; // Bloquear nuevos eventos hasta que termine

        int randomEvent = Random.Range(0, 6); // 6 eventos posibles

        if (randomEvent == 0 && bouncyEffect != null && !bouncyEffect.IsBouncyActive)
        {
            Debug.Log("Evento activado: Rebote alto por 10 segundos");
            bouncyEffect.ActivateBouncyEffect(10f);
            Invoke(nameof(ResetEvent), 10f);
        }
        else if (randomEvent == 1 && explosionEvent != null)
        {
            Debug.Log("Evento activado: Explosión");
            explosionEvent.TriggerExplosion();
            Invoke(nameof(ResetEvent), 4f);
        }
        else if (randomEvent == 2 && ghostModeEvent != null)
        {
            Debug.Log("Evento activado: Modo Fantasma (5s sin jugador)");
            ghostModeEvent.TriggerGhostMode();
            Invoke(nameof(ResetEvent), 5f);
        }
        else if (randomEvent == 3 && vanishingEvent != null)
        {
            Debug.Log("Evento activado: Modo Desvanecimiento");
            vanishingEvent.TriggerVanishing();
            Invoke(nameof(ResetEvent), 10f);
        }
        else if (randomEvent == 4 && autoThrowEvent != null)
        {
            Debug.Log("Evento activado: Lanzamiento automático por 10s");
            autoThrowEvent.StartAutoThrow();
            Invoke(nameof(ResetEvent), 10f);
        }
        else if (randomEvent == 5 && speedUpEvent != null)
        {
            Debug.Log("Evento activado: Modo Turbo");
            speedUpEvent.TriggerSpeedUp();
            Invoke(nameof(ResetEvent), 8f);
        }
        else
        {
            eventActive = false; // Si no se activó nada, permitir otro intento
        }
    }

    private void ResetEvent()
    {
        eventActive = false; // Permitir un nuevo evento
        if (gameManager != null)
        {
            lastScore = gameManager.CurrentScore; // 🔥 Ahora los puntos solo cuentan después de que el evento termine
        }
    }
}
