using System.Collections;
using UnityEngine;

public class TriggerLoss : MonoBehaviour
{
    private float _timer = 0f;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 10)  // Asegúrate de que esta sea la capa correcta
        {
            _timer += Time.deltaTime;

            // Verificamos si el temporizador ha pasado el umbral de TimeTillGamerOver
            if (_timer >= GameManager.Instance.TimeTillGamerOver)
            {
                // Llamamos a GameOver en el GameManager cuando el tiempo se acaba
                GameManager.Instance.GameOver();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 10)  // Asegúrate de que esta sea la capa correcta
        {
            _timer = 0f;  // Reiniciamos el temporizador cuando el jugador sale de la zona
        }
    }
}
