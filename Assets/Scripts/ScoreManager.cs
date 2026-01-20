using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    // Listens to Damage / Kill events
    // Aggregates damage
    // Calculates score
    // Notifies UI

    private float totalDamage;
    private int score;

    private void OnEnable()
    {
        BulletTarget.OnDamageTaken += RegisterDamage;
        BulletTarget.OnTargetKilled += RegisterKill;
    }

    private void RegisterDamage(float dmg)
    {
        totalDamage += dmg;
    }

    private void RegisterKill(BulletTarget target)
    {
        score += 100; //або з config
    }
}