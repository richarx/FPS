using Player.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Scanner
{
    public class ScannerVhsEffect : MonoBehaviour
    {
        [SerializeField] private Image vhsEffect;
        [SerializeField] private Material vhsMaterial;
        [SerializeField] private float maxFade;
        [SerializeField] private float vhsScrollSpeed;

        private bool isDisplayed;
        
        private void Start()
        {
            PlayerStateMachine player = PlayerStateMachine.instance;   
            
            player.scanner.OnScannerVisorAppear.AddListener(Display);
            player.scanner.OnScannerVisorDisappear.AddListener(Hide);
            
            vhsEffect.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (isDisplayed)
                AnimateVhsEffect();
        }
        
        private void AnimateVhsEffect()
        {
            Vector2 offset = vhsMaterial.mainTextureOffset;
            offset.y += vhsScrollSpeed * Time.deltaTime;
            vhsMaterial.mainTextureOffset = offset;
        }
    
        private void Display()
        {
            StartCoroutine(Tools.Fade(vhsEffect, 0.1f, true, maxFade));
            isDisplayed = true;
        }

        private void Hide()
        {
            StartCoroutine(Tools.Fade(vhsEffect, 0.05f, false, maxFade));
            isDisplayed = false;
        }
    
        private void OnDestroy()
        {
            vhsMaterial.mainTextureOffset = Vector2.zero;
        }
    }
}
