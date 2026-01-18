using System;
using UnityEngine;

public class BulletTarget : MonoBehaviour
{
    [SerializeField] private AudioClip recieveDamageSound;

    public static event Action<float> OnTakeDamage;
    public static event Action<float, float> OnHealthChange;

    private float health = 100f;
    private float maxHealth = 100f;
    private float takenDamage = 0f;

    public float MaxHealth
    {
        get => maxHealth;
        set => maxHealth = value;
    }

    public float Health
    {
        get => health;
        protected set
        {
            health = value;
            OnHealthChange?.Invoke(health, MaxHealth);
        }
    }

    public float TakenDamage
    {
        get => takenDamage;
        protected set
        {
            OnTakeDamage?.Invoke(value - TakenDamage);
            takenDamage = value;
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