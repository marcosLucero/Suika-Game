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
    private SlotMachine slotMachine;
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        bouncyEffect = FindObjectOfType<BouncyEffect>();
        explosionEvent = FindObjectOfType<ExplosionEvent>();
        ghostModeEvent = FindObjectOfType<GhostModeEvent>();
        vanishingEvent = FindObjectOfType<VanishingEvent>();
        speedUpEvent = FindObjectOfType<SpeedUpEvent>();
        autoThrowEvent = FindObjectOfType<AutoThrowEvent>();
        slotMachine = FindObjectOfType<SlotMachine>();

        if (gameManager != null)
        {
            lastScore = gameManager.CurrentScore;
        }
    }

    private void Update()
    {
        // 🛑 No activar eventos si hay uno en curso
        if (eventActive || gameManager == null) return;

        if (gameManager.CurrentScore >= lastScore + 5)
        {
            //TriggerRandomEvent();
            StartEventWithSlot();
        }
    }
     private void StartEventWithSlot()
    {
        eventActive = true; // Bloqueamos nuevos eventos
        if (slotMachine != null)
        {
            slotMachine.StartSpin(TriggerEventFromSlot); // 📌 Inicia la ruleta y espera el resultado
        }
    }

    private void TriggerEventFromSlot(int resultIndex)
    {
        switch (resultIndex)
        {
            case 0:
                if (bouncyEffect != null && !bouncyEffect.IsBouncyActive)
                {
                    Debug.Log("Evento: Rebote alto por 10 segundos");
                    bouncyEffect.ActivateBouncyEffect(10f);
                    Invoke(nameof(ResetEvent), 10f);
                }
                break;
            case 1:
                if (explosionEvent != null)
                {
                    Debug.Log("Evento: Explosión");
                    explosionEvent.TriggerExplosion();
                    Invoke(nameof(ResetEvent), 4f);
                }
                break;
            case 2:
                if (ghostModeEvent != null)
                {
                    Debug.Log("Evento: Modo Fantasma (5s sin jugador)");
                    ghostModeEvent.TriggerGhostMode();
                    Invoke(nameof(ResetEvent), 5f);
                }
                break;
            case 3:
                if (vanishingEvent != null)
                {
                    Debug.Log("Evento: Modo Desvanecimiento");
                    vanishingEvent.TriggerVanishing();
                    Invoke(nameof(ResetEvent), 10f);
                }
                break;
            case 4:
                if (autoThrowEvent != null)
                {
                    Debug.Log("Evento: Lanzamiento automático por 10s");
                    autoThrowEvent.StartAutoThrow();
                    Invoke(nameof(ResetEvent), 10f);
                }
                break;
            case 5:
                if (speedUpEvent != null)
                {
                    Debug.Log("Evento: Modo Turbo");
                    speedUpEvent.TriggerSpeedUp();
                    Invoke(nameof(ResetEvent), 8f);
                }
                break;
            default:
                Debug.LogWarning("No se encontró un evento para este índice.");
                ResetEvent();
                break;
        }
    }

    private void ResetEvent()
    {
        eventActive = false;
        if (gameManager != null)
        {
            lastScore = gameManager.CurrentScore;
        }
    }
}
