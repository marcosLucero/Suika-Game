using UnityEngine;
using UnityEngine.UI;

public class CanvasIntroMainMenu : MonoBehaviour
{
    [SerializeField] private Canvas introCanvas;
    [SerializeField] private Button closeButton;

    private const string CanvasShownKey = "IntroCanvasShown_MainMenu";

    void Start()
    {
        // Mostrar el canvas solo si no se ha mostrado antes en esta sesión
        if (!PlayerPrefs.HasKey(CanvasShownKey))
        {
            introCanvas.gameObject.SetActive(true);
            Time.timeScale = 0f; // Pausar juego
            PlayerPrefs.SetInt(CanvasShownKey, 1);
        }
        else
        {
            introCanvas.gameObject.SetActive(false);
        }

        // Asignar funcionalidad al botón
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(HideCanvas);
        }
    }

    public void HideCanvas()
    {
        introCanvas.gameObject.SetActive(false);
        Time.timeScale = 1f; // Reanudar juego
    }
}
