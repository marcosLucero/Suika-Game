using UnityEngine;
using System.Collections;

public class GhostModeEvent : MonoBehaviour
{
    public float ghostDuration = 5f; // Tiempo que el jugador estará desactivado
    public SpriteRenderer _spriteRenderer;
    ThrowMineralController throwMineralController;
    public Transform childA;
    private bool isActive = false;

    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); // Buscar al jugador en la escena
    }

    private void OnDisable()
    {
        if (isActive && player != null)
        {
            _spriteRenderer.enabled = true;
            throwMineralController = player.GetComponent<ThrowMineralController>();
            throwMineralController.isEventActive(0);
            if (childA != null && childA.childCount > 0)
            {
                childA.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
            }
            isActive = false;
        }
    }

    public void TriggerGhostMode()
    {
        if (player != null && !isActive)
        {
            isActive = true;
            StartCoroutine(GhostModeCountdown());
        }
    }

    IEnumerator GhostModeCountdown()
    {
        Debug.Log("👻 Modo Fantasma ACTIVADO: Jugador desaparece");

        _spriteRenderer.enabled = false;
        throwMineralController = player.GetComponent<ThrowMineralController>();
        throwMineralController.isEventActive(1);
        childA = player.gameObject.transform.GetChild(1);
        if(childA.childCount > 0)
        {
            childA.GetChild(0).GetComponent<SpriteRenderer>().enabled=false;
        }

        yield return new WaitForSeconds(ghostDuration);

        _spriteRenderer.enabled = true;
        throwMineralController = player.GetComponent<ThrowMineralController>();
        throwMineralController.isEventActive(0);
        if (childA.childCount > 0)
        {
            childA.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
        }

        Debug.Log("👻 Modo Fantasma DESACTIVADO: Jugador vuelve");
        isActive = false;
    }
}
