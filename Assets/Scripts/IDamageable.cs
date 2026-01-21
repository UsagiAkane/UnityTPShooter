using UnityEngine;

public interface IDamageable
{
    //TODO
    //float maxHealth //TODO Property
    //float currentHealth; //TODO Property
    
    void TakeDamage(DamageInfo damage);
}