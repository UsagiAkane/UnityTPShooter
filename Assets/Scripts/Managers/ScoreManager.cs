using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    //Listen to Damage / Kill events
    //Aggregate damage
    //Calculate score
    //Notify UI
    
    [SerializeField] private int playerTeamId = 0;
    [SerializeField] private int scorePerKill = 100;

    public event Action<int> OnScoreChanged;

    public static ScoreManager Instance { get; private set; }

    public int CurrentScore => _score;

    private int _score;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void OnEnable()
    {
        BulletTarget.OnTargetKilled += HandleTargetKilled;
    }

    private void OnDisable()
    {
        BulletTarget.OnTargetKilled -= HandleTargetKilled;
    }

    private void HandleTargetKilled(BulletTarget target, DamageInfo damage)
    {
        if (damage.instigator.TeamId != playerTeamId) return;

        _score += scorePerKill;
        OnScoreChanged?.Invoke(_score);
    }
}