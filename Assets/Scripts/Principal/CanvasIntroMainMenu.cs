using UnityEngine;
using UnityEngine.UI;

public class CanvasIntroMainMenu : MonoBehaviour
{
    [SerializeField] private Canvas introCanvas;
    [SerializeField] private Button closeButton;

    private static bool canvasShown = false;

    void Start()
    {
        if (!canvasShown)
        {
            introCanvas.gameObject.SetActive(true);
            Time.timeScale = 0f; // Pausar juego
            canvasShown = true;

            if (closeButton != null)
            {
                closeButton.onClick.AddListener(HideCanvas);
            }
        }
        else
        {
            introCanvas.gameObject.SetActive(false);
        }
    }

    public void HideCanvas()
    {
        introCanvas.gameObject.SetActive(false);
        Time.timeScale = 1f; // Reanudar juego
    }
}
