using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int CurrentScore { get; private set; }

    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private GameObject _gameOverPanel; // Panel de Game Over
    [SerializeField] private GameObject _panelImage; // Imagen de fondo del panel
    [SerializeField] private GameObject _box; // Referencia al objeto box
    [SerializeField] private GameObject _player; // Referencia al objeto player
    [SerializeField] private TextMeshProUGUI _finalScoreText; // Texto "Tu puntuación: X"
    [SerializeField] private float _fadeTime = 2f;

    // Tiempo hasta el game over
    public float TimeTillGamerOver = 1.5f;  // Aquí definimos el tiempo en segundos

    // Referencia al FirebaseManager
    public FirebaseManager firebaseManager;

    // Nombre del jugador (esto lo puedes obtener de un Text Input si lo deseas)
    public string playerName = "Jugador";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        if (firebaseManager == null)
        {
            firebaseManager = FindObjectOfType<FirebaseManager>(); // Buscar el componente FirebaseManager en la escena
        }

        _scoreText.text = CurrentScore.ToString("0");

        // Asegurar que el panel comienza desactivado
        _gameOverPanel.SetActive(false);
    }


    public void IncreaseScore(int amount)
    {
        CurrentScore += amount;
        _scoreText.text = CurrentScore.ToString("0");
    }

    // Este método lo llamas desde el botón para guardar los datos
    public void SaveData()
    {
        playerName = FindObjectOfType<ScoreManager>().playerNameInput.text.Trim(); // Tomar el nombre desde ScoreManager

        if (string.IsNullOrEmpty(playerName))
            playerName = "Jugador_" + UnityEngine.Random.Range(1000, 9999); // Nombre por defecto

        Debug.Log("Nombre enviado a Firebase desde GameManager: " + playerName);

        if (firebaseManager != null)
        {
            firebaseManager.SavePlayerData(playerName, CurrentScore);
        }
    }



    public void GameOver()
    {
        // Desactivar el jugador y la caja cuando el juego termina
        if (_box != null)
        {
            _box.SetActive(false);
        }

        if (_player != null)
        {
            _player.SetActive(false);
        }

        // Mostrar la puntuación final en el panel de Game Over
        if (_finalScoreText != null)
        {
            _finalScoreText.text = "Tu puntuación: " + CurrentScore;
        }

        // Mostrar el panel de Game Over con una animación de fade
        StartCoroutine(FadeInGameOverPanel());
    }

    private IEnumerator FadeInGameOverPanel()
    {
        _gameOverPanel.SetActive(true);

        // Fade in effect
        Color startColor = _panelImage.GetComponent<Image>().color;
        startColor.a = 0f;
        _panelImage.GetComponent<Image>().color = startColor;

        float elapsedTime = 0f;
        while (elapsedTime < _fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(0f, 1f, elapsedTime / _fadeTime);
            startColor.a = newAlpha;
            _panelImage.GetComponent<Image>().color = startColor;

            yield return null;
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
