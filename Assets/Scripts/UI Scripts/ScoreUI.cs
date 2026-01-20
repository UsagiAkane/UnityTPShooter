using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;

    private ScoreManager _scoreManager;

    private void Awake()
    {
        _scoreManager = UIContext.Instance.ScoreManager;
    }

    private void OnEnable()
    {
        if (_scoreManager == null)
            return;

        _scoreManager.OnScoreChanged += UpdateScore;
        UpdateScore(_scoreManager.CurrentScore);
    }

    private void OnDisable()
    {
        if (_scoreManager == null)
            return;

        _scoreManager.OnScoreChanged -= UpdateScore;
    }

    private void UpdateScore(int score)
    {
        scoreText.text = score.ToString();
    }
}