using UnityEngine;
using System.Collections;

public class GhostModeEvent : MonoBehaviour
{
    public float ghostDuration = 5f; // Tiempo que el jugador estará desactivado

    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); // Buscar al jugador en la escena
    }

    public void TriggerGhostMode()
    {
        if (player != null)
        {
            StartCoroutine(GhostModeCountdown());
        }
    }

    IEnumerator GhostModeCountdown()
    {
        Debug.Log("👻 Modo Fantasma ACTIVADO: Jugador desaparece");

        player.SetActive(false); // 🔴 Desactivar jugador

        yield return new WaitForSeconds(ghostDuration); // ⏳ Esperar los 5 segundos

        player.SetActive(true); // 🟢 Reactivar jugador

        Debug.Log("👻 Modo Fantasma DESACTIVADO: Jugador vuelve");
    }
}
