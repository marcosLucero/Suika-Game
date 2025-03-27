using UnityEngine;
using System.Collections;

public class SpeedUpEvent : MonoBehaviour
{
    public float speedMultiplier = 1.5f; // Cuánto se acelera el juego
    public float eventDuration = 8f; // Cuánto dura el evento

    private bool isActive = false;

    public void TriggerSpeedUp()
    {
        if (!isActive)
        {
            StartCoroutine(SpeedUpRoutine());
        }
    }

    private IEnumerator SpeedUpRoutine()
    {
        isActive = true;
        Debug.Log("🚀 Modo Turbo ACTIVADO: ¡Todo se mueve más rápido!");

        Time.timeScale *= speedMultiplier; // Aumentar la velocidad del juego

        yield return new WaitForSecondsRealtime(eventDuration); // Espera en tiempo real

        Time.timeScale /= speedMultiplier; // Restaurar la velocidad normal
        Debug.Log("🛑 Modo Turbo FINALIZADO: Todo vuelve a la normalidad.");

        isActive = false;
    }
}
