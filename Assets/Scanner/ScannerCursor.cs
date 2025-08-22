using System.Collections;
using Player.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Scanner
{
    public class ScannerCursor : MonoBehaviour
    {
        [Space]
        [SerializeField] private RectTransform corners;
        [SerializeField] private RectTransform sides;
        [SerializeField] private RectTransform line;
        [SerializeField] private RectTransform circle;
        [SerializeField] private float lineMoveSpeed;

        private PlayerStateMachine player;
        private ScannerDetector scannerDetector;
        
        private Image cornersImage;
        private Image sidesImage;
        private Image lineImage;
        private Image circleImage;

        private float maxFade = 1.0f;

        private bool isDisplayed;
        public bool IsDisplayed => isDisplayed;
        private bool animateDown;
        
        private float lineStartingHeight;
        private float liveVelocity;

        private Vector3 lineScaleVelocity;
        private Vector3 cornerScaleVelocity;

        private void Start()
        {
            scannerDetector = GetComponent<ScannerDetector>();
            
            player = PlayerStateMachine.instance;

            player.scanner.OnScannerVisorAppear.AddListener(DisplayCursor);
            player.scanner.OnScannerVisorDisappear.AddListener(HideCursor);

            Color transparent = Color.white;
            transparent.a = 0.0f;

            cornersImage = corners.GetComponent<Image>();
            cornersImage.color = transparent;
            sidesImage = sides.GetComponent<Image>();
            sidesImage.color = transparent;
            lineImage = line.GetComponent<Image>();
            lineImage.color = transparent;
            circleImage = circle.GetComponent<Image>();
            circleImage.color = transparent;

            lineStartingHeight = line.localPosition.y;
        }

        private void Update()
        {
            if (isDisplayed)
            {
                AnimateLineHeight();
                AnimateScale();
            }
        }

        private void AnimateScale()
        {
            Vector3 scale = corners.localScale;
            scale = Vector3.SmoothDamp(scale, ComputeCornerScale(), ref cornerScaleVelocity, 0.1f);
            corners.localScale = scale;
            
            scale = line.localScale;
            scale = Vector3.SmoothDamp(scale, ComputeLineScale(), ref lineScaleVelocity, 0.1f);
            line.localScale = scale;
        }

        private Vector3 ComputeLineScale()
        {
            if (player.isAiming && scannerDetector.HasTarget)
                return Vector3.one;
                
            if (scannerDetector.HasTarget)
                return Vector3.one * 1.25f;
            
            if (player.isAiming)
                return Vector3.one * 0.65f;

            return Vector3.one * 0.85f;
        }

        private Vector3 ComputeCornerScale()
        {
            Vector3 cornerTargetScale = Vector3.one;

            if (scannerDetector.HasTarget)
                cornerTargetScale *= 1.5f;
            
            if (player.isAiming)
                cornerTargetScale *= 0.8f;

            return cornerTargetScale;
        }

        private void AnimateLineHeight()
        {
            Vector3 position = line.localPosition;
            float speed = scannerDetector.HasTarget ? lineMoveSpeed / 8.0f : lineMoveSpeed;
            float lineHeight = ComputeLineHeight();
            
            if (animateDown)
            {
                if (position.y < lineStartingHeight - lineHeight)
                    animateDown = false;
                else
                    position.y = Mathf.SmoothDamp(position.y, lineStartingHeight - lineHeight - 0.5f, ref liveVelocity, speed);
            }
            else
            {
                if (position.y > lineStartingHeight + lineHeight)
                    animateDown = true;
                else
                    position.y = Mathf.SmoothDamp(position.y, lineStartingHeight + lineHeight + 0.5f, ref liveVelocity, speed);
            }

            line.localPosition = position;
        }

        private float ComputeLineHeight()
        {
            if (player.isAiming && scannerDetector.HasTarget)
                return 12;
            
            if (player.isAiming)
                return 8;

            if (scannerDetector.HasTarget)
                return 15;
            
            return 11;
        }

        private void DisplayCursor()
        {
            StopAllCoroutines();
            StartCoroutine(DisplayCursorCoroutine());
        }

        private IEnumerator DisplayCursorCoroutine()
        {
            yield return new WaitForSeconds(0.3f);
            
            StartCoroutine(Tools.Fade(cornersImage, 0.3f, true, maxFade));
            corners.localScale = new Vector3(2.0f, 2.0f, 1.0f);
            Vector3 cornerScale = ComputeCornerScale();
            yield return Tools.TweenLocalScale(corners, cornerScale.x - 0.2f, cornerScale.y - 0.2f, 1.0f, 0.2f);
            yield return Tools.TweenLocalScale(corners, cornerScale.x, cornerScale.y, 1.0f, 0.1f);
            
            StartCoroutine(Tools.Fade(sidesImage, 0.3f, true, maxFade));
            sides.localScale = new Vector3(2.0f, 2.0f, 1.0f);
            StartCoroutine(Tools.TweenLocalScale(sides, 1.0f, 1.0f, 1.0f, 0.5f));
            
            line.localPosition = new Vector3(line.localPosition.x, lineStartingHeight, 0.0f);
            StartCoroutine(Tools.Fade(lineImage, 0.3f, true, maxFade));
            line.localScale = new Vector3(0.0f, 1.0f, 1.0f);
            yield return Tools.TweenLocalScale(line, 1.0f, 1.0f, 1.0f, 0.5f);
            
            yield return Tools.Fade(circleImage, 0.1f, true, maxFade);
            circle.localScale = new Vector3(2.0f, 2.0f, 1.0f);
            yield return Tools.TweenLocalScale(circle, 0.8f, 0.8f, 1.0f, 0.1f);
            yield return Tools.TweenLocalScale(circle, 1.0f, 1.0f, 1.0f, 0.1f);

            isDisplayed = true;
        }

        private void HideCursor()
        {
            StopAllCoroutines();
            StartCoroutine(HideCursorCoroutine());
        }
        
        private IEnumerator HideCursorCoroutine()
        {
            StartCoroutine(Tools.Fade(cornersImage, 0.05f, false, maxFade));
            StartCoroutine(Tools.Fade(sidesImage, 0.05f, false, maxFade));
            StartCoroutine(Tools.Fade(lineImage, 0.05f, false, maxFade));
            yield return Tools.Fade(circleImage, 0.05f, false, maxFade);
            
            isDisplayed = false;
        }
    }
}
