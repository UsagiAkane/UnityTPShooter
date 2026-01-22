using System;
using UnityEngine;

namespace Player.AimSystem
{
    public class AimSystem : MonoBehaviour
    {
        public event Action<AimResult> OnAimComputed;

        [Header("Aim params")] [Tooltip("Main camera")] [SerializeField]
        private Transform aimDirectionSource;
        [SerializeField] private LayerMask aimMask;
        [SerializeField] private float maxAimDistance = 100f;
        //[SerializeField] private bool enableRaycast = true;

        private IAimProvider _aimProvider;

        private AimResult _currentAim;
        private bool _hasAim;


        public bool HasAim => _hasAim;
        public AimResult CurrentAim => _currentAim;

        private void Update()
        {
            if (_aimProvider == null)
                return;

            PerformAim();
        }

        //bind
        public void SetAimProvider(IAimProvider provider)
        {
            _aimProvider = provider;
            _hasAim = false;
        }

        public void ClearAimProvider(IAimProvider provider)
        {
            if (_aimProvider == provider)
            {
                _aimProvider = null;
                _hasAim = false;
            }
        }

        //[SerializeField] private Transform aimPointDebug;
        private void PerformAim()
        {
            //Usually main camera
            Vector3 origin = aimDirectionSource.position;
            Vector3 direction = aimDirectionSource.forward;
            
            RaycastHit hit = default;
            bool hasHit = false;

            hasHit = Physics.Raycast(
                origin,
                direction,
                out hit,
                maxAimDistance,
                aimMask,
                QueryTriggerInteraction.Ignore
            );

            //aim point
            Vector3 aimPoint = hasHit ? hit.point : origin + direction * maxAimDistance;

            //aim data
            _currentAim = new AimResult(
                direction,
                aimPoint,
                hasHit,
                hasHit ? hit : default
            );

            _hasAim = true;

            OnAimComputed?.Invoke(_currentAim);
            
            
            //aimPointDebug.position = aimPoint;
            //Debug.Log(Physics.CheckSphere(origin, 0.01f));
            //Debug.DrawRay(origin, direction * 20f, hasHit ? Color.red : Color.green);
        }
    }
}