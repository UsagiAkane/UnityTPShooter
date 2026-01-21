using UnityEngine;


//AimSystem нічого не крутить, він просто ретьорнить результат прицілювання
public class AimSystem : MonoBehaviour
{
    [Header("Aim params")]
    [Tooltip("Main camera")]
    [SerializeField] private Transform aimDirectionSource;
    [SerializeField] private LayerMask aimMask;
    [SerializeField] private float maxAimDistance = 100f;
    [SerializeField] private bool enableRaycast = true;

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

    private void PerformAim()
    {
        Transform originTransform = _aimProvider.AimOrigin;

        Vector3 origin = aimDirectionSource.position;   // CAMERA
        Vector3 direction = aimDirectionSource.forward; // CAMERA
        
        

        bool hasHit = false;
        RaycastHit hit = default;

        if (enableRaycast)
        {
            hasHit = Physics.Raycast(
                origin,
                direction,
                out hit,
                maxAimDistance,
                aimMask,
                QueryTriggerInteraction.Ignore
            );
        }

        Vector3 aimPoint = hasHit ? hit.point : origin + direction * maxAimDistance;

        _currentAim = new AimResult(
            direction,
            aimPoint,
            hasHit,
            hit
        );

        _hasAim = true;

        //visual реакція
        _aimProvider.OnAimUpdated(_currentAim);

        // Debug (safe)
        Debug.DrawRay(origin, direction * 2f, hasHit ? Color.red : Color.green);
    }
}