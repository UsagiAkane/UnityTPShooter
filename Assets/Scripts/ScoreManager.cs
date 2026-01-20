using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    //Listen to Damage / Kill events
    //Aggregate damage
    //Calculate score
    //Notify UI

    public static event Action<int> OnScoreChanged;

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
        Debug.Log(
            $"Instigator: {damage.instigator.name}, " +
            $"tag: {damage.instigator.tag}"
        );
        if (!damage.instigator.CompareTag("Player")) return;

        _score += 100;//поки хардкодом
        OnScoreChanged?.Invoke(_score);
    }
}