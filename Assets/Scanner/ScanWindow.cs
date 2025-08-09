using Player.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Scanner
{
    public class ScanWindow : MonoBehaviour
    {
        [SerializeField] private Image border;
        [SerializeField] private float fillBorderDuration;
        [SerializeField] private Image background;
        [SerializeField] private float fadeBackgroundDuration;
        [SerializeField] private float backgroundMaxFade;

        private ScannerDetector scannerDetector;
        private bool isDisplayed;
        
        private void Start()
        {
            scannerDetector = GetComponent<ScannerDetector>();
            
            scannerDetector.OnScanNewTarget.AddListener(Display);
            scannerDetector.OnLoseScanTarget.AddListener(Hide);
            PlayerStateMachine.instance.scanner.OnScannerVisorDisappear.AddListener(HideFast);

            border.MakeTransparent();
            background.MakeTransparent();
        }
        
        private void Display()
        {
            if (isDisplayed)
                return;
            
            StopAllCoroutines();
            border.MakeVisible();
            StartCoroutine(Tools.FillImage(border, fillBorderDuration, true));
            StartCoroutine(Tools.Fade(background, fadeBackgroundDuration, true, backgroundMaxFade, delay:fillBorderDuration / 2.0f));
            isDisplayed = true;
        }

        private void Hide()
        {
            if (!isDisplayed)
                return;
            
            StopAllCoroutines();
            StartCoroutine(Tools.FillImage(border, fillBorderDuration, false));
            StartCoroutine(Tools.Fade(background, 0.05f, false, backgroundMaxFade));
            isDisplayed = false;
        }
        
        private void HideFast()
        {
            if (!isDisplayed)
                return;
            
            StopAllCoroutines();
            StartCoroutine(Tools.Fade(border, 0.05f, false));
            StartCoroutine(Tools.Fade(background, 0.05f, false, backgroundMaxFade));
            isDisplayed = false;
        }
    }
}
