using System;
using UnityEngine;

public class BulletTarget : MonoBehaviour
{
    public static event Action<float> OnTookDamage;
    public static event Action<float, float> OnHealthChanged;
    
    [SerializeField] private AudioClip recieveDamageSound;

    private float _health = 100f;
    private float _maxHealth = 100f;
    private float _takenDamage = 0f;

    public float MaxHealth
    {
        get => _maxHealth;
        set => _maxHealth = value;
    }

    public float Health
    {
        get => _health;
        protected set
        {
            _health = value;
            OnHealthChanged?.Invoke(_health, MaxHealth);
        }
    }

    public float TakenDamage
    {
        get => _takenDamage;
        protected set
        {
            OnTookDamage?.Invoke(value - TakenDamage);
            _takenDamage = value;
        }
    }

    public void TookDamage(float damage)
    {
        TakenDamage += damage;
        Health -= damage;

        //play sfx
        SFXmanager.instance.PlaySFXClip(recieveDamageSound, transform, 1f);
    }
}