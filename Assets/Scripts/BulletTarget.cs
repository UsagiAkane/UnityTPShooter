using System;
using UnityEngine;

public class BulletTarget : MonoBehaviour, IDamageable
{
    [SerializeField] private AudioClip recieveDamageSound;//TODO REWORK?
    
    public static event Action<BulletTarget, DamageInfo> OnTargetKilled;//TODO
    //public static event Action<float> OnDamageTaken;
    
    public static int AliveCount { get; private set; } //TODO Enemy spawner with runtime set list<SO>
    

    [SerializeField] private float maxHealth = 100f;
    private float _health;

    private void Awake()
    {
        AliveCount++;
        _health = maxHealth;
    }

    public void TakeDamage(DamageInfo damage)
    {
        if (_health <= 0f) return;

        _health -= damage.amount;

        if (_health <= 0f)
        {
            OnTargetKilled?.Invoke(this, damage);
            Destroy(gameObject);
        }
    }
    
    private void OnDestroy()
    {
        AliveCount--;
    }
}