using UnityEngine;
using UnityEngine.UI;

public class CanvasOncePerSession : MonoBehaviour
{
    [SerializeField] private Canvas targetCanvas;
    [SerializeField] private Button closeButton;
    private static bool canvasShown = false;

    private void Start()
    {
        if (!canvasShown)
        {
            targetCanvas.gameObject.SetActive(true);
            Time.timeScale = 0f; // ⏸️ Pausar el juego
            canvasShown = true;

            if (closeButton != null)
            {
                closeButton.onClick.AddListener(HideCanvas);
            }
        }
        else
        {
            targetCanvas.gameObject.SetActive(false);
        }
    }

    public void HideCanvas()
    {
        targetCanvas.gameObject.SetActive(false);
        Time.timeScale = 1f; // ▶️ Reanudar el juego
    }
}
