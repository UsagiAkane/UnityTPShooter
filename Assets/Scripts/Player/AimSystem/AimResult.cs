using UnityEngine;

namespace Player.AimSystem
{
    public readonly struct AimResult
    {
        public readonly Vector3 Direction;//normalized
        public readonly Vector3 AimPoint;//direction * distance, AimPoint = targeting intent
        public readonly bool HasHit;
        public readonly RaycastHit Hit;

        public AimResult(
            Vector3 direction,
            Vector3 aimPoint,
            bool hasHit,
            RaycastHit hit)
        {
            Direction = direction;
            AimPoint = aimPoint;
            HasHit = hasHit;
            Hit = hit;
        }
    }
}
