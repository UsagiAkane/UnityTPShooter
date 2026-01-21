using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;

    private ScoreManager _scoreManager;

    public void Bind(ScoreManager scoreManager)
    {
        _scoreManager = scoreManager;
        _scoreManager.OnScoreChanged += UpdateScore;
        UpdateScore(_scoreManager.CurrentScore);
    }

    private void OnDestroy()
    {
        if (_scoreManager != null)
            _scoreManager.OnScoreChanged -= UpdateScore;
    }

    private void UpdateScore(int score)
    {
        scoreText.text = score.ToString();
    }
}