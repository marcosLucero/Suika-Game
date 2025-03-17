using UnityEngine;
using TMPro;

public class RowDisplay : MonoBehaviour
{
    public TextMeshProUGUI playerNameText;
    public TextMeshProUGUI scoreText;

    public void SetRow(string name, int score)
    {
        playerNameText.text = name;
        scoreText.text = score.ToString();
    }
}
