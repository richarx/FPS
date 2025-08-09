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

        private bool isDisplayed;
        
        private void Start()
        {
            PlayerStateMachine player = PlayerStateMachine.instance;   
            
            player.scanner.OnScannerVisorAppear.AddListener(Display);
            player.scanner.OnScannerVisorDisappear.AddListener(Hide);

            border.MakeTransparent();
            background.MakeTransparent();
        }
        
        private void Display()
        {
            border.MakeVisible();
            StartCoroutine(Tools.FillImage(border, fillBorderDuration, true));
            StartCoroutine(Tools.Fade(background, fadeBackgroundDuration, true, backgroundMaxFade, delay:fillBorderDuration));
            isDisplayed = true;
        }

        private void Hide()
        {
            StartCoroutine(Tools.Fade(border, 0.05f, false));
            StartCoroutine(Tools.Fade(background, 0.05f, false, backgroundMaxFade));
            isDisplayed = false;
        }
    }
}
