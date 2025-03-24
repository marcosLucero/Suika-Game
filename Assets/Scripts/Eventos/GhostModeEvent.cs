using UnityEngine;
using System.Collections;

public class GhostModeEvent : MonoBehaviour
{
    public float ghostDuration = 5f; // Tiempo que el jugador estará desactivado
    public SpriteRenderer _spriteRenderer;
    ThrowMineralController throwMineralController;
    public Transform childA;


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

        _spriteRenderer.enabled = false;
        throwMineralController = player.GetComponent<ThrowMineralController>();
        throwMineralController.isEventActive(1);
        //throwMineralController.enabled = false;
        childA = player.gameObject.transform.GetChild(1);
        if(childA.childCount > 0)
        {
            childA.GetChild(0).GetComponent<SpriteRenderer>().enabled=false;
        }
        //player.SetActive(false); // 🔴 Desactivar jugador

        yield return new WaitForSeconds(ghostDuration); // ⏳ Esperar los 5 segundos

        _spriteRenderer.enabled = true;
        throwMineralController = player.GetComponent<ThrowMineralController>();
        throwMineralController.isEventActive(0);
        //throwMineralController.enabled = true;
        if (childA.childCount > 0)
        {
            childA.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
        }

        //player.SetActive(true); // 🟢 Reactivar jugador

        Debug.Log("👻 Modo Fantasma DESACTIVADO: Jugador vuelve");
    }
}
