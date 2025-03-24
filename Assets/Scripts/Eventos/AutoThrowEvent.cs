using UnityEngine;
using System.Collections;

public class AutoThrowEvent : MonoBehaviour
{
    private bool isActive = false;
    private float duration = 10f;
    private float pressInterval = 0.2f; // Intervalo entre pulsaciones

    public void StartAutoThrow()
    {
        if (!isActive)
        {
            isActive = true;
            StartCoroutine(AutoThrowCoroutine());
        }
    }

    private IEnumerator AutoThrowCoroutine()
    {
        float endTime = Time.time + duration;

        while (Time.time < endTime)
        {
            SimulateKeyPress(); // Simular la pulsación de la tecla espacio
            yield return new WaitForSeconds(pressInterval);
        }

        isActive = false;
    }

    private void SimulateKeyPress()
    {
        UserInput.IsThrowPressed = true;
        StartCoroutine(ResetThrowPress());
    }

    private IEnumerator ResetThrowPress()
    {
        yield return null; // Esperar un frame
        UserInput.IsThrowPressed = false;
    }
}
