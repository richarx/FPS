using Player.Scripts;
using Tools_and_Scripts;
using UnityEngine;

namespace Scanner
{
    public class ScanCursorFollowTarget : MonoBehaviour
    {
        [SerializeField] private RectTransform canvas;
        [SerializeField] private RectTransform cursor;
        
        private ScannerCursor scannerCursor;
        private ScannerDetector scannerDetector;

        private Vector3 startingPosition;
        private Vector3 velocity;
        
        private void Start()
        {
            scannerCursor = GetComponent<ScannerCursor>();
            scannerDetector = GetComponent<ScannerDetector>();
            
            PlayerStateMachine.instance.scanner.OnScannerVisorAppear.AddListener(ResetCursorPosition);

            startingPosition = cursor.localPosition;
            
            Debug.Log($"Cursor starting position : {startingPosition}");
        }
        
        private void Update()
        {
            if (scannerCursor.IsDisplayed)
            {
                if (scannerDetector.HasTarget)
                    FollowTarget();
                else
                    FollowScreenCenter();
            }
        }

        private void FollowTarget()
        {
            Vector3 currentPosition = cursor.localPosition;
            Vector3 targetPosition = scannerDetector.CurrentTarget.transform.position + scannerDetector.CurrentTarget.GetDisplayOffset();
            Vector2 screenPosition = CameraScreenPosition.instance.WorldToScreen(targetPosition);

            Vector2 newPosition = CameraScreenPosition.instance.ScreenPointToLocalPointInRectangle(canvas, screenPosition);
            
            currentPosition = Vector3.SmoothDamp(currentPosition, newPosition, ref velocity, 0.1f);
            cursor.localPosition = currentPosition;
        }

        private void FollowScreenCenter()
        {
            Vector3 currentPosition = cursor.localPosition;
            
            currentPosition = Vector3.SmoothDamp(currentPosition, startingPosition, ref velocity, 0.1f);
            cursor.localPosition = currentPosition;
        }

        private void ResetCursorPosition()
        {
            cursor.localPosition = startingPosition;
        }
    }
}
