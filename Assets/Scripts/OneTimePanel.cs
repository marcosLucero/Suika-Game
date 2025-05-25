using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CanvasOncePerSession : MonoBehaviour
{
    [SerializeField] private Canvas targetCanvas;
    [SerializeField] private Button closeButton;
    private static bool canvasShown = false;
    private ThrowMineralController throwMineralController;

    private void Start()
    {
        throwMineralController = FindObjectOfType<ThrowMineralController>();
        
        if (!canvasShown)
        {
            targetCanvas.enabled = true;
            Time.timeScale = 0f; // ⏸️ Pausar el juego
            canvasShown = true;

            if (closeButton != null)
            {
                closeButton.onClick.AddListener(HideCanvas);
            }

            // Deshabilitar el lanzamiento de minerales mientras el panel está activo
            if (throwMineralController != null)
            {
                throwMineralController.CanThrow = false;
            }
        }
        else
        {
            targetCanvas.enabled = false;
        }
    }

    public bool IsCanvasActive()
    {
        return targetCanvas != null && targetCanvas.enabled;
    }

    public void HideCanvas()
    {
        targetCanvas.enabled = false;
        Time.timeScale = 1f; // ▶️ Reanudar el juego
        StartCoroutine(EnableMineralThrowing());
    }

    private IEnumerator EnableMineralThrowing()
    {
        // Esperar un frame para asegurarnos de que el input del botón X se ha procesado
        yield return null;
        
        // Esperar un poco más para estar seguros
        yield return new WaitForSeconds(0.1f);

        // Habilitar el lanzamiento de minerales cuando se cierra el panel
        if (throwMineralController != null)
        {
            throwMineralController.CanThrow = true;
        }
    }
}
