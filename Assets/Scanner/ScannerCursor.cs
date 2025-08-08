using System;
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
        [SerializeField] private RectTransform vhsEffect;
        [SerializeField] private Material vhsMaterial;
        [SerializeField] private float vhsScrollSpeed;

        private Image cornersImage;
        private Image sidesImage;
        private Image lineImage;
        private Image circleImage;

        private float maxFade = 1.0f;

        private bool isDisplaying;
        private bool isDisplayed;
        
        private void Start()
        {
            PlayerStateMachine player = PlayerStateMachine.instance;
            
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
            
            holder.SetActive(false);
            vhsEffect.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (isDisplaying || isDisplayed)
            {
                AnimateVhsEffect();
            }
        }

        private void AnimateVhsEffect()
        {
            Vector2 offset = vhsMaterial.mainTextureOffset;
            offset.y += vhsScrollSpeed * Time.deltaTime;
            vhsMaterial.mainTextureOffset = offset;
        }

        private void DisplayCursor()
        {
            StopAllCoroutines();
            StartCoroutine(DisplayCursorCoroutine());
        }

        private IEnumerator DisplayCursorCoroutine()
        {
            isDisplaying = true;
            vhsEffect.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.3f);
            holder.SetActive(true);
            
            StartCoroutine(Tools.Fade(cornersImage, 0.3f, true, maxFade));
            corners.localScale = new Vector3(2.0f, 2.0f, 1.0f);
            yield return Tools.TweenScale(corners, 0.8f, 0.8f, 1.0f, 0.2f);
            yield return Tools.TweenScale(corners, 1.0f, 1.0f, 1.0f, 0.1f);
            
            StartCoroutine(Tools.Fade(sidesImage, 0.3f, true, maxFade));
            sides.localScale = new Vector3(2.0f, 2.0f, 1.0f);
            StartCoroutine(Tools.TweenScale(sides, 1.0f, 1.0f, 1.0f, 0.5f));
            
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
            vhsEffect.gameObject.SetActive(false);
            isDisplayed = false;
        }

        private void OnDestroy()
        {
            vhsMaterial.mainTextureOffset = Vector2.zero;
        }
    }
}
