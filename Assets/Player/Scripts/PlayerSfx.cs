using System.Collections.Generic;
using Items.Weapons;
using SFX;
using UnityEngine;

namespace Player.Scripts
{
    public class PlayerSfx : MonoBehaviour
    {
        [SerializeField] private AudioClip adsInWoosh;
        [SerializeField] private AudioClip adsOutWoosh;
        [SerializeField] private List<AudioClip> jumpWoosh;
        [SerializeField] private List<AudioClip> landingLight;
        [SerializeField] private List<AudioClip> slideStart;

        private const float wooshVolume = 0.01f;
        private const float insertDelay = 0.6f;
        private const float tailVolume = 0.05f;
        private const float tailFadeDuration = 0.05f;

        private PlayerStateMachine player;
        private float lastShotTimestamp = -1.0f;
        private AudioSource lastTail = null;

        private WeaponData currentWeapon => player.playerGun.CurrentWeapon;

        private void Start()
        {
            player = GetComponent<PlayerStateMachine>();
            player.playerShootGun.OnShoot.AddListener(PlayGunShotSound);
            player.playerShootGun.OnShootEmptyMag.AddListener(() => SFXManager.instance.PlayRandomSFX(currentWeapon.emptyMag));
            player.playerAmmo.OnStartReloading.AddListener(() =>
            {
                SFXManager.instance.PlayRandomSFX(currentWeapon.ejectMag);
                SFXManager.instance.PlayRandomSFX(currentWeapon.insertMag, delay:insertDelay);
                SFXManager.instance.PlayRandomSFX(currentWeapon.cockGun, delay:currentWeapon.cockDelay);
            });
            player.playerAiming.OnChangeAimState.AddListener((isAiming) =>
            {
                SFXManager.instance.PlaySFX(isAiming ? adsInWoosh : adsOutWoosh, wooshVolume);
                
                if (isAiming && player.playerGun.hasWeapon)
                    SFXManager.instance.PlayRandomSFX(currentWeapon.adsClick);
            });
            player.playerJump.OnJump.AddListener(() => SFXManager.instance.PlayRandomSFX(jumpWoosh));
            player.playerJump.OnGroundedChanged.AddListener((isGrounded, impactVelocity) =>
            {
                if (isGrounded)
                    SFXManager.instance.PlayRandomSFX(landingLight);
            });
            player.playerSlide.OnStartSlide.AddListener((_) => SFXManager.instance.PlayRandomSFX(slideStart, 0.03f));
        }

        private void PlayGunShotSound()
        {
            if (currentWeapon.gunShotFirst != null && Time.time - lastShotTimestamp >= 0.5f)
                SFXManager.instance.PlaySFX(currentWeapon.gunShotFirst, currentWeapon.shotVolume);
            else
                SFXManager.instance.PlayRandomSFX(currentWeapon.gunShot, currentWeapon.shotVolume);
            
            if (lastTail != null)
                lastTail.GetComponent<FadeSound>().Trigger(tailFadeDuration);
            if (currentWeapon.gunShotTail != null)
                lastTail = SFXManager.instance.PlaySFX(currentWeapon.gunShotTail, tailVolume);
            
            lastShotTimestamp = Time.time;
        }
    }
}
