using System.Collections.Generic;
using SFX;
using UnityEngine;

namespace Player.Scripts
{
    public class PlayerSfx : MonoBehaviour
    {
        [SerializeField] private AudioClip gunShotFirst;
        [SerializeField] private List<AudioClip> gunShot;
        [SerializeField] private AudioClip gunShotTail;
        [SerializeField] private List<AudioClip> emptyMag;
        [SerializeField] private List<AudioClip> ejectMag;
        [SerializeField] private List<AudioClip> insertMag;
        [SerializeField] private List<AudioClip> cockGun;
        [SerializeField] private AudioClip adsInWoosh;
        [SerializeField] private AudioClip adsOutWoosh;
        [SerializeField] private float wooshVolume;
        [SerializeField] private List<AudioClip> adsClick;
        [SerializeField] private float insertDelay;
        [SerializeField] private float cockDelay;
        [SerializeField] private float shotVolume;
        [SerializeField] private float tailVolume;
        [SerializeField] private float tailFadeDuration;
        
        private PlayerGun playerGun;
        private float lastShotTimestamp = -1.0f;
        private AudioSource lastTail = null;

        private void Start()
        {
            playerGun = GetComponent<PlayerGun>();
            playerGun.OnShoot.AddListener(PlayGunShotSound);
            playerGun.OnShootEmptyMag.AddListener(() => SFXManager.instance.PlayRandomSFX(emptyMag));
            playerGun.OnStartReloading.AddListener(() =>
            {
                SFXManager.instance.PlayRandomSFX(ejectMag);
                SFXManager.instance.PlayRandomSFX(insertMag, delay:insertDelay);
                SFXManager.instance.PlayRandomSFX(cockGun, delay:cockDelay);
            });
            playerGun.OnChangeAimState.AddListener((isAiming) =>
            {
                SFXManager.instance.PlaySFX(isAiming ? adsInWoosh : adsOutWoosh, wooshVolume);
                
                if (isAiming)
                    SFXManager.instance.PlayRandomSFX(adsClick);
            });
        }

        private void PlayGunShotSound()
        {
            if (Time.time - lastShotTimestamp >= 0.5f)
                SFXManager.instance.PlaySFX(gunShotFirst, shotVolume);
            else
                SFXManager.instance.PlayRandomSFX(gunShot, shotVolume);
            
            if (lastTail != null)
                lastTail.GetComponent<FadeSound>().Trigger(tailFadeDuration);
            lastTail = SFXManager.instance.PlaySFX(gunShotTail, tailVolume);
            
            lastShotTimestamp = Time.time;
        }
    }
}
