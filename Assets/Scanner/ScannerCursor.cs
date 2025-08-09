using System.Collections;
using Player.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Scanner
{
    public class ScannerCursor : MonoBehaviour
    {
        [Space] 
        [SerializeField] private GameObject holder;
        
        [Space]
        [SerializeField] private RectTransform corners;
        [SerializeField] private RectTransform sides;
        [SerializeField] private RectTransform line;
        [SerializeField] private RectTransform circle;
        [SerializeField] private float lineMoveSpeed;

        private PlayerStateMachine player;
        
        private Image cornersImage;
        private Image sidesImage;
        private Image lineImage;
        private Image circleImage;

        private float maxFade = 1.0f;

        private bool isDisplaying;
        private bool isDisplayed;
        public bool IsDisplayed => isDisplayed;
        private bool animateDown;
        
        private float lineStartingHeight;
        private float liveVelocity;

        private float lineMoveHeight => player.isAiming ? 8 : 11;
        
        private Vector3 lineTargetScale => player.isAiming ? Vector3.one * 0.65f : Vector3.one * 0.85f;
        private Vector3 lineScaleVelocity;

        private Vector3 cornerTargetScale => player.isAiming ? Vector3.one * 0.8f : Vector3.one;
        private Vector3 cornerScaleVelocity;

        private void Start()
        {
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
            
            holder.SetActive(false);
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
            scale = Vector3.SmoothDamp(scale, cornerTargetScale, ref cornerScaleVelocity, 0.1f);
            corners.localScale = scale;
            
            scale = line.localScale;
            scale = Vector3.SmoothDamp(scale, lineTargetScale, ref lineScaleVelocity, 0.1f);
            line.localScale = scale;
        }

        private void AnimateLineHeight()
        {
            Vector3 position = line.localPosition;
            
            if (animateDown)
            {
                if (position.y < lineStartingHeight - lineMoveHeight)
                    animateDown = false;
                else
                    position.y = Mathf.SmoothDamp(position.y, lineStartingHeight - lineMoveHeight - 0.5f, ref liveVelocity, lineMoveSpeed);
            }
            else
            {
                if (position.y > lineStartingHeight + lineMoveHeight)
                    animateDown = true;
                else
                    position.y = Mathf.SmoothDamp(position.y, lineStartingHeight + lineMoveHeight + 0.5f, ref liveVelocity, lineMoveSpeed);
            }

            line.localPosition = position;
        }

        private void DisplayCursor()
        {
            StopAllCoroutines();
            StartCoroutine(DisplayCursorCoroutine());
        }

        private IEnumerator DisplayCursorCoroutine()
        {
            isDisplaying = true;
            yield return new WaitForSeconds(0.3f);
            holder.SetActive(true);
            
            StartCoroutine(Tools.Fade(cornersImage, 0.3f, true, maxFade));
            corners.localScale = new Vector3(2.0f, 2.0f, 1.0f);
            yield return Tools.TweenScale(corners, cornerTargetScale.x - 0.2f, cornerTargetScale.y - 0.2f, 1.0f, 0.2f);
            yield return Tools.TweenScale(corners, cornerTargetScale.x, cornerTargetScale.y, 1.0f, 0.1f);
            
            StartCoroutine(Tools.Fade(sidesImage, 0.3f, true, maxFade));
            sides.localScale = new Vector3(2.0f, 2.0f, 1.0f);
            StartCoroutine(Tools.TweenScale(sides, 1.0f, 1.0f, 1.0f, 0.5f));
            
            line.localPosition = new Vector3(line.localPosition.x, lineStartingHeight, 0.0f);
            StartCoroutine(Tools.Fade(lineImage, 0.3f, true, maxFade));
            line.localScale = new Vector3(0.0f, 1.0f, 1.0f);
            yield return Tools.TweenScale(line, 1.0f, 1.0f, 1.0f, 0.5f);
            
            yield return Tools.Fade(circleImage, 0.1f, true, maxFade);
            circle.localScale = new Vector3(2.0f, 2.0f, 1.0f);
            yield return Tools.TweenScale(circle, 0.8f, 0.8f, 1.0f, 0.1f);
            yield return Tools.TweenScale(circle, 1.0f, 1.0f, 1.0f, 0.1f);

            isDisplaying = false;
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
            holder.SetActive(false);
            
            isDisplayed = false;
        }
    }
}
