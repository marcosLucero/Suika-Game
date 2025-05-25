using UnityEngine;
using TMPro;

public class PauseManager : MonoBehaviour
{
    [Header("Pause Canvas")]
    public Canvas pauseCanvas;
    [SerializeField] private TextMeshProUGUI pauseText1;
    [SerializeField] private TextMeshProUGUI pauseText2;

    private bool isPaused = false;
    private float previousTimeScale;

    private void Awake()
    {
        // Asegurarse de que el canvas está desactivado al inicio
        if (pauseCanvas != null)
        {
            pauseCanvas.enabled = false;
            Debug.Log("PauseManager: Canvas inicializado y desactivado");
        }
        else
        {
            Debug.LogError("PauseManager: No se ha asignado el canvas de pausa!");
        }
    }

    private void Update()
    {
        // Si está pausado, cualquier tecla lo despausa
        if (isPaused && Input.anyKeyDown)
        {
            Debug.Log("PauseManager: Tecla presionada, despausando juego");
            UnpauseGame();
        }
    }

    // Esta función se llama desde el botón de pausa
    public void PauseButtonPressed()
    {
        Debug.Log("PauseManager: Botón de pausa presionado");
        if (!isPaused)
        {
            PauseGame();
        }
    }

    private void PauseGame()
    {
        Debug.Log("PauseManager: Pausando juego");
        isPaused = true;
        // Guardar el timeScale actual antes de pausar
        previousTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        
        // Mostrar el canvas de pausa
        if (pauseCanvas != null)
        {
            pauseCanvas.enabled = true;
            Debug.Log("PauseManager: Canvas de pausa activado");
        }
        else
        {
            Debug.LogError("PauseManager: No se puede activar el canvas de pausa - referencia nula!");
        }
    }

    private void UnpauseGame()
    {
        Debug.Log("PauseManager: Despausando juego");
        isPaused = false;
        // Restaurar el timeScale anterior
        Time.timeScale = previousTimeScale;
        
        // Ocultar el canvas de pausa
        if (pauseCanvas != null)
        {
            pauseCanvas.enabled = false;
            Debug.Log("PauseManager: Canvas de pausa desactivado");
        }
    }

    private void OnDestroy()
    {
        // Asegurarse de que el tiempo se restaura si el objeto se destruye
        if (isPaused)
        {
            Time.timeScale = previousTimeScale;
        }
    }
} 