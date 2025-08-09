using Items;
using UnityEngine;

namespace Scanner
{
    public class ScannerDetector : MonoBehaviour
    {
        [SerializeField] private Transform scanPivot;        
        [SerializeField] private LayerMask targetLayer;
        
        private ScannerCursor scannerCursor;
        
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
        }

        private void ShootDetectionRay()
        {
            float distance = 200.0f;
            bool hit = Physics.Raycast(scanPosition, scanDirection, out RaycastHit hitInfo, distance, targetLayer);

            Scanable scanable = hit ? hitInfo.collider.GetComponent<Scanable>() : null;
            
            if (scanable != null)
                Debug.Log($"Scan target : {scanable.gameObject.name}");
        }
    }
}
