using UnityEngine;

namespace Guns
{
    public class Laser : HitscanGun
    {
        protected override void OnHit(RaycastHit hit, Vector3 direction)
        {
            if (hit.collider.TryGetComponent<IDamageable>(out var damageable))
            {
                DamageInfo damageInfo = new DamageInfo
                {
                    amount = damage,
                    source = gameObject,   // gun
                    instigator = instigator,    // holder
                    hitPoint = hit.point,
                    hitDirection = direction
                };

                damageable.TakeDamage(damageInfo);
            }

            //TODO impact FX
        }

        protected override void OnMiss(Vector3 direction)
        {
            //TODO FX without hit
        }
    }
}