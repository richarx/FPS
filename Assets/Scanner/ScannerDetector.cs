using Items;
using Tools_and_Scripts;
using UnityEngine;
using UnityEngine.Events;

namespace Scanner
{
    public class ScannerDetector : MonoBehaviour
    {
        [SerializeField] private Transform scanPivot;        
        [SerializeField] private LayerMask targetLayer;
        [SerializeField] private float maxDistanceBeforeLosingTarget;

        [HideInInspector] public UnityEvent OnScanNewTarget = new UnityEvent();
        [HideInInspector] public UnityEvent OnLoseScanTarget = new UnityEvent();
        
        private ScannerCursor scannerCursor;

        private Scanable currentTarget;
        public Scanable CurrentTarget => currentTarget;
        public bool HasTarget => currentTarget != null;

        public Vector3 scanPosition => scanPivot.position;
        public Vector3 scanDirection => scanPivot.forward;
        
        private void Start()
        {
            scannerCursor = GetComponent<ScannerCursor>();
        }

        private void Update()
        {
            if (scannerCursor.IsDisplayed)
                ShootDetectionRay();

            if (HasTarget && !IsInCursorRange(currentTarget))
                LoseTarget();
        }

        private void ShootDetectionRay()
        {
            float distance = 200.0f;
            bool hit = Physics.Raycast(scanPosition, scanDirection, out RaycastHit hitInfo, distance, targetLayer);

            Scanable scanable = hit ? hitInfo.collider.GetComponent<Scanable>() : null;

            if (scanable != null && scanable != currentTarget && IsInCursorRange(scanable))
                SetNewTarget(scanable);
        }
        
        private bool IsInCursorRange(Scanable target)
        {
            return ComputeTargetDistance(target.transform.position + target.GetDisplayOffset()) < maxDistanceBeforeLosingTarget;
        }

        private float ComputeTargetDistance(Vector3 position)
        {
            Vector2 screenPosition = CameraScreenPosition.instance.WorldToScreen(position);
            Vector2 center = CameraScreenPosition.instance.GetScreenCenterPosition();

            return (screenPosition - center).magnitude;
        }

        private void SetNewTarget(Scanable scanable)
        {
            if (currentTarget != null)
                currentTarget.DeactivateOutline();
            
            currentTarget = scanable;
            currentTarget.ActivateOutline();
            OnScanNewTarget?.Invoke();
        }
        
        private void LoseTarget()
        {
            currentTarget.DeactivateOutline();
            currentTarget = null;
            OnLoseScanTarget?.Invoke();
        }
    }
}
