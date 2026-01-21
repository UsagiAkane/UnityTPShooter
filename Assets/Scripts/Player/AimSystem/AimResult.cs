using UnityEngine;

//не впевнений щодо цієї структури але мені здається що це оптимальний варіант оскільки
//без логіки
//без MonoBehaviour
//можна кешувати / логувати
namespace Player.AimSystem
{
    public readonly struct AimResult
    {
        public readonly Vector3 Direction;// normalized
        public readonly Vector3 AimPoint;// direction * distance AimPoint = targeting intent, не гарант
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
