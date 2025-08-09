using System.Collections.Generic;
using Player.Scripts;
using SFX;
using UnityEngine;

namespace Items.Weapons
{
    public class WeaponSFX : MonoBehaviour
    {
        [Space][Header("Shoot")]
        [SerializeField] private AudioClip gunShotFirst;
        [SerializeField] private List<AudioClip> gunShot;
        [SerializeField] private AudioClip gunShotTail;
        [SerializeField] private float shotVolume;

        [Space][Header("Empty")]
        [SerializeField] private List<AudioClip> emptyMag;
        
        [Space][Header("Reload")]
        [SerializeField] private List<AudioClip> ejectMag;
        [SerializeField] private List<AudioClip> insertMag;
        [SerializeField] private float insertDelay = 0.6f;
        [SerializeField] private float reloadCockDelay = 0.6f;
     
        [Space][Header("Equip")]
        [SerializeField] private List<AudioClip> equipWeapon;
        [SerializeField] private List<AudioClip> equipWeapon_2;
        [SerializeField] private float equipWeapon_2_Delay;
        [SerializeField] private float equipWeaponCockDelay;

        [Space][Header("Handle")]
        [SerializeField] private List<AudioClip> cockGun;
        [SerializeField] private List<AudioClip> adsClick;
        
        private const float tailVolume = 0.05f;
        private const float tailFadeDuration = 0.05f;
        
        private PlayerStateMachine player;
        private float lastShotTimestamp = -1.0f;
        private AudioSource lastTail = null;

        private bool isAkimbo;
        
        private void Start()
        {
            player = PlayerStateMachine.instance;

            isAkimbo = GetComponent<AnimateGun>().isAkimbo;
            
            if (isAkimbo)
            {
                player.playerShootGun.OnShootAkimbo.AddListener(PlayGunShotSound);
                player.playerShootGun.OnShootAkimboEmptyMag.AddListener(() => SFXManager.instance.PlayRandomSFX(emptyMag));
            }
            else
            {
                player.playerShootGun.OnShoot.AddListener(PlayGunShotSound);
                player.playerShootGun.OnShootEmptyMag.AddListener(() => SFXManager.instance.PlayRandomSFX(emptyMag));
            }

            player.playerAmmo.OnStartReloading.AddListener(PlayReloadSound);
            player.playerGun.OnSwapWeapon.AddListener((_) => PlaySwapSound());
            player.playerAiming.OnChangeAimState.AddListener((isAiming) =>
            {
                if (isAiming && player.playerGun.hasWeapon && !player.isLocked && !player.isScanning)
                    SFXManager.instance.PlayRandomSFX(adsClick);
            });
        }
        
        private void PlayGunShotSound()
        {
            if (gunShotFirst != null && Time.time - lastShotTimestamp >= 0.5f)
                SFXManager.instance.PlaySFX(gunShotFirst, shotVolume);
            else
                SFXManager.instance.PlayRandomSFX(gunShot, shotVolume);
            
            if (lastTail != null)
                lastTail.GetComponent<FadeSound>().Trigger(tailFadeDuration);
            if (gunShotTail != null)
                lastTail = SFXManager.instance.PlaySFX(gunShotTail, tailVolume);
            
            lastShotTimestamp = Time.time;
        }

        private void PlayReloadSound(bool isRight, bool isLeft)
        {
            if ((isRight && !isAkimbo) || (isLeft && isAkimbo))
            {
                SFXManager.instance.PlayRandomSFX(ejectMag);
                SFXManager.instance.PlayRandomSFX(insertMag, delay:insertDelay);
                SFXManager.instance.PlayRandomSFX(cockGun, delay:reloadCockDelay);
            }
        }

        private void PlaySwapSound()
        {
            SFXManager.instance.PlayRandomSFX(equipWeapon);
            SFXManager.instance.PlayRandomSFX(equipWeapon_2, delay:equipWeapon_2_Delay);
            SFXManager.instance.PlayRandomSFX(cockGun, delay:equipWeaponCockDelay);
        }

        private void OnDestroy()
        {
            player.playerAmmo.OnStartReloading.RemoveListener(PlayReloadSound);
            player.playerGun.OnSwapWeapon.RemoveListener((_) => PlaySwapSound());
        }
    }
}
