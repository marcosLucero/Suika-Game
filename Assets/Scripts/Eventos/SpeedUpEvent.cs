using UnityEngine;
using System.Collections;

public class SpeedUpEvent : MonoBehaviour
{
    public float speedMultiplier = 1.5f; // Cuánto se acelera el juego
    public float eventDuration = 8f; // Cuánto dura el evento
    public float transitionDuration = 0.5f; // Duración de la transición

    private bool isActive = false;
    private bool isTransitioning = false;
    private float originalTimeScale;

    public bool IsEventTrulyFinished => !isActive && !isTransitioning;

    private void OnDisable()
    {
        // Asegurarse de que el timeScale vuelva a la normalidad al desactivar el objeto
        if (isActive || isTransitioning)
        {
            Time.timeScale = originalTimeScale;
            isActive = false;
            isTransitioning = false;
        }
    }

    public void TriggerSpeedUp()
    {
        if (!isActive && !isTransitioning)
        {
            StartCoroutine(SpeedUpRoutine());
        }
    }

    private IEnumerator SpeedUpRoutine()
    {
        isActive = true;
        isTransitioning = true;
        originalTimeScale = Time.timeScale;
        Debug.Log("🚀 Modo Turbo ACTIVADO: ¡Todo se mueve más rápido!");

        // Transición suave hacia la velocidad aumentada
        float elapsedTime = 0f;
        float startTimeScale = Time.timeScale;
        float targetTimeScale = startTimeScale * speedMultiplier;

        while (elapsedTime < transitionDuration)
        {
            Time.timeScale = Mathf.Lerp(startTimeScale, targetTimeScale, elapsedTime / transitionDuration);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        Time.timeScale = targetTimeScale;
        isTransitioning = false;
        yield return new WaitForSecondsRealtime(eventDuration);

        // Transición suave de vuelta a la velocidad normal
        isTransitioning = true;
        elapsedTime = 0f;
        startTimeScale = Time.timeScale;
        targetTimeScale = originalTimeScale;

        while (elapsedTime < transitionDuration)
        {
            Time.timeScale = Mathf.Lerp(startTimeScale, targetTimeScale, elapsedTime / transitionDuration);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        Time.timeScale = originalTimeScale;
        Debug.Log("🛑 Modo Turbo FINALIZADO: Todo vuelve a la normalidad.");
        isActive = false;
        isTransitioning = false;
    }
}
