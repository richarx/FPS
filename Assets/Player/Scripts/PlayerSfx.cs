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
        [SerializeField] private List<AudioClip> adsClick;
        [SerializeField] private List<AudioClip> jumpWoosh;
        [SerializeField] private List<AudioClip> landingLight;

        private const float wooshVolume = 0.01f;
        private const float insertDelay = 0.6f;
        private const float cockDelay = 1.4f;
        private const float shotVolume = 0.07f;
        private const float tailVolume = 0.03f;
        private const float tailFadeDuration = 0.05f;

        private PlayerStateMachine player;
        private float lastShotTimestamp = -1.0f;
        private AudioSource lastTail = null;

        private void Start()
        {
            player = GetComponent<PlayerStateMachine>();
            player.playerGun.OnShoot.AddListener(PlayGunShotSound);
            player.playerGun.OnShootEmptyMag.AddListener(() => SFXManager.instance.PlayRandomSFX(emptyMag));
            player.playerGun.OnStartReloading.AddListener(() =>
            {
                SFXManager.instance.PlayRandomSFX(ejectMag);
                SFXManager.instance.PlayRandomSFX(insertMag, delay:insertDelay);
                SFXManager.instance.PlayRandomSFX(cockGun, delay:cockDelay);
            });
            player.playerGun.OnChangeAimState.AddListener((isAiming) =>
            {
                SFXManager.instance.PlaySFX(isAiming ? adsInWoosh : adsOutWoosh, wooshVolume);
                
                if (isAiming)
                    SFXManager.instance.PlayRandomSFX(adsClick);
            });
            player.playerJump.OnJump.AddListener(() => SFXManager.instance.PlayRandomSFX(jumpWoosh));
            player.playerJump.OnGroundedChanged.AddListener((isGrounded, impactVelocity) =>
            {
                if (isGrounded)
                    SFXManager.instance.PlayRandomSFX(landingLight);
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
