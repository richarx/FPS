using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Player.Scripts
{
    public class PlayerChromaticAberration : MonoBehaviour
    {
        private ChromaticAberration chromatic;

        private VFXData vfxData;
        
        private bool isSetup;
        
        private void Start()
        {
            Volume volume = GetComponent<Volume>();
            VolumeProfile volumeProfile = volume.profile;
            isSetup = volumeProfile.TryGet<ChromaticAberration>(out chromatic);
            
            PlayerStateMachine player = PlayerStateMachine.instance;

            vfxData = player.vfxData;
            player.playerShootGun.OnHit.AddListener((_, surfaceData) =>
            {
                if (isSetup && surfaceData == SurfaceData.SurfaceType.Enemy)
                {
                    StopAllCoroutines();
                    StartCoroutine(TriggerChromaticAberration());
                }
            });
        }

        private IEnumerator TriggerChromaticAberration()
        {
            float timer = 0.0f;
            while (timer <= vfxData.chromaticAberrationFadeDuration)
            {
                chromatic.intensity.value = Tools.NormalizeValue(timer, 0.0f, vfxData.chromaticAberrationFadeDuration) * vfxData.chromaticAberrationIntensity;
                yield return null;
                timer += Time.deltaTime;
            }

            yield return new WaitForSeconds(vfxData.chromaticAberrationDuration);
            
            timer = 0.0f;
            while (timer <= vfxData.chromaticAberrationFadeDuration)
            {
                chromatic.intensity.value = vfxData.chromaticAberrationIntensity - (Tools.NormalizeValue(timer, 0.0f, vfxData.chromaticAberrationFadeDuration) * vfxData.chromaticAberrationIntensity);
                yield return null;
                timer += Time.deltaTime;
            }
        }
    }
}
