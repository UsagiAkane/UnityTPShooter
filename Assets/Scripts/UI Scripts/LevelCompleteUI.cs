using TMPro;
using UnityEngine;

public class LevelCompleteUI : MonoBehaviour
{
    [SerializeField] private GameObject root;
    [SerializeField] private TMP_Text scoreText;

    private void Awake()
    {
        root.SetActive(false);
    }

    public void Show()
    {
        root.SetActive(true);

        scoreText.text =
            ScoreManager.Instance.CurrentScore.ToString();
    }
}