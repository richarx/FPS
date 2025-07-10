using System.Collections;
using Data;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Player.Scripts
{
    public class PlayerVignette : MonoBehaviour
    {
        private Vignette vignette;

        private PlayerData playerData;
        
        private bool isSetup;
        private bool isDisplayed;

        private void Start()
        {
            Volume volume = GetComponent<Volume>();
            VolumeProfile volumeProfile = volume.profile;
            isSetup = volumeProfile.TryGet<Vignette>(out vignette);

            PlayerStateMachine player = PlayerStateMachine.instance;

            playerData = player.playerData;
            player.playerCrouch.OnStartCrouch.AddListener((isFromSlide) => DisplayVignette());
            player.playerCrouch.OnStopCrouch.AddListener((isToSlide) => HideVignette());
            player.playerSlide.OnStartSlide.AddListener((isFromCrouch) => DisplayVignette());
            player.playerSlide.OnStopSlide.AddListener((isToCrouch) => HideVignette());
        }

        private void DisplayVignette()
        {
            if (isDisplayed || !isSetup)
                return;

            isDisplayed = true;
            
            StopAllCoroutines();
            StartCoroutine(FadeVignette(vignette.intensity.value, playerData.crouchVignetteIntensity));
        }

        private void HideVignette()
        {
            if (!isDisplayed || !isSetup)
                return;
            
            isDisplayed = false;
            
            StopAllCoroutines();
            StartCoroutine(FadeVignette(vignette.intensity.value, 0.0f));
        }
        
        private IEnumerator FadeVignette(float current, float target)
        {
            float duration = playerData.vignetteIntensityTransitionDuration;

            float timer = 0.0f;
            while (timer <= duration)
            {
                vignette.intensity.value = Tools.NormalizeValueInRange(timer, 0.0f, duration, current, target);
                yield return null;
                timer += Time.deltaTime;
            }
        }
    }
}
