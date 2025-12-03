using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    void Update()
    {
        if (ScoreManager.Instance == null || scoreText == null) return;

        int displayScore = Mathf.FloorToInt(ScoreManager.Instance.Score);
        scoreText.text = "Score: " + displayScore;
    }
}
