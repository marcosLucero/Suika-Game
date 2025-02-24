using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int CurrentScore { get; private set; }

    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private GameObject _gameOverPanel; // Panel de Game Over
    [SerializeField] private Image _panelImage; // Imagen de fondo del panel
    [SerializeField] private GameObject _box; // Referencia al objeto box
    [SerializeField] private GameObject _player; // Referencia al objeto player
    [SerializeField] private TextMeshProUGUI _finalScoreText; // Texto "Tu puntuación: X"
    [SerializeField] private float _fadeTime = 2f;

    public float TimeTillGamerOver = 1.5f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
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

    public void GameOver()
    {
        // Desactivar box y player
        if (_box != null) _box.SetActive(false);
        if (_player != null) _player.SetActive(false);

        // Mostrar la puntuación final en el panel de Game Over
        if (_finalScoreText != null)
        {
            _finalScoreText.text = "Tu puntuación: " + CurrentScore;
            _finalScoreText.gameObject.SetActive(true);
        }

        StartCoroutine(FadeInGameOverPanel());
    }

    private IEnumerator FadeInGameOverPanel()
    {
        _gameOverPanel.SetActive(true);

        Color startColor = _panelImage.color;
        startColor.a = 0f;
        _panelImage.color = startColor;

        float elapsedTime = 0f;
        while (elapsedTime < _fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(0f, 1f, elapsedTime / _fadeTime);
            startColor.a = newAlpha;
            _panelImage.color = startColor;

            yield return null;
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
