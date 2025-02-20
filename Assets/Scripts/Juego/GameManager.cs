using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int CurrentScore { get; private set; }

    [SerializeField] private TextMeshProUGUI _scoreText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        _scoreText.text = CurrentScore.ToString("0");
    }

    public void IncreaseScore(int amount)
    {
        CurrentScore += amount;
        _scoreText.text = CurrentScore.ToString("0");
    }
}
