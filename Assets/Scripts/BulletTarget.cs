using System;
using UnityEngine;

public class BulletTarget : MonoBehaviour, IDamageable
{
    [SerializeField] private AudioClip recieveDamageSound;//TODO REWORK?
    
    public static event Action<BulletTarget, DamageInfo> OnTargetKilled;//TODO
    public static event Action<float> OnDamageTaken; //для UI

    [SerializeField] private float maxHealth = 100f;
    private float _health;

    private void Awake()
    {
        _health = maxHealth;
    }

    public void TakeDamage(DamageInfo damage)
    {
        if (_health <= 0f) return;//бо дестрой асінк???

        _health -= damage.amount;

        if (_health <= 0f)
        {
            OnTargetKilled?.Invoke(this, damage);
            Destroy(gameObject);
        }
    }
}