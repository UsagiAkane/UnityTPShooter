using UnityEngine;

namespace Guns
{
    public abstract class HitscanGun : Gun
    {
        [Header("Hitscan params")]
        [SerializeField] protected float maxRange = 100f;
        [SerializeField] protected LayerMask hitMask;
        
        
        protected override void ShootLogic(AimResult aim)
        {
            if (CheckHit(aim, out RaycastHit hit, out Vector3 direction))
            {
                OnHit(hit, direction);
            }
            else
            {
                OnMiss(direction);
            }
        }
        
        //Hitscan logic
        protected virtual bool CheckHit(
            AimResult aim,
            out RaycastHit hit,
            out Vector3 direction)
        {
            direction = aim.Direction;

            if (aim.HasHit)
            {
                hit = aim.Hit;
                return true;
            }
            
            hit = default;//to compile but dont use hit
            return false;
        }
        
        protected abstract void OnHit(RaycastHit hit, Vector3 direction);

        protected virtual void OnMiss(Vector3 direction)
        {
            //FX without hit
        }
    }
}