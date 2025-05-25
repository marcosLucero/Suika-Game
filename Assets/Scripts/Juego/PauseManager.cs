using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    [Header("Pause Canvas")]
    public Canvas pauseCanvas;
    [SerializeField] private TextMeshProUGUI pauseText1;
    [SerializeField] private TextMeshProUGUI pauseText2;

    [Header("Audio")]
    [SerializeField] private AudioSource[] audioSources; // Array de todas las fuentes de audio que queremos pausar

    private bool isPaused = false;
    private float previousTimeScale;
    private float[] previousPitchValues; // Para guardar los valores de pitch originales

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

        // Inicializar el array de pitch si hay fuentes de audio
        if (audioSources != null && audioSources.Length > 0)
        {
            previousPitchValues = new float[audioSources.Length];
        }
    }

    private void Update()
    {
        // Detectar el botón izquierdo del mando (Y en Xbox, Triangle en PlayStation)
        if (Gamepad.current != null && Gamepad.current.buttonWest.wasPressedThisFrame)
        {
            if (isPaused)
            {
                UnpauseGame();
            }
            else
            {
                PauseGame();
            }
        }

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
        
        // Pausar el audio
        if (audioSources != null)
        {
            for (int i = 0; i < audioSources.Length; i++)
            {
                if (audioSources[i] != null)
                {
                    previousPitchValues[i] = audioSources[i].pitch;
                    audioSources[i].pitch = 0f; // Esto efectivamente pausa el audio
                }
            }
        }
        
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
        
        // Restaurar el audio
        if (audioSources != null)
        {
            for (int i = 0; i < audioSources.Length; i++)
            {
                if (audioSources[i] != null)
                {
                    audioSources[i].pitch = previousPitchValues[i];
                }
            }
        }
        
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
            // Restaurar el audio si es necesario
            if (audioSources != null)
            {
                for (int i = 0; i < audioSources.Length; i++)
                {
                    if (audioSources[i] != null)
                    {
                        audioSources[i].pitch = previousPitchValues[i];
                    }
                }
            }
        }
    }
} 