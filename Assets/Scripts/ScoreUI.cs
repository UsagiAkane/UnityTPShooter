using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Awake()
    {
        //BulletTarget.OnHealthChange += HandleScoreTextHP;
        BulletTarget.OnDamageTaken += HandleScoreTextDamage;
    }

    private void HandleScoreTextHP(float current, float max)
    {
        scoreText.text = $"{current}/{max}";
    }

    private void HandleScoreTextDamage(float dmg)
    {
        float.TryParse(scoreText.text, out float damage);//DELETE
        scoreText.text = (damage + dmg).ToString();
    }

    private void OnDestroy()
    {
        //BulletTarget.OnHealthChange -= HandleScoreTextHP;
        BulletTarget.OnDamageTaken -= HandleScoreTextDamage;
    }
}