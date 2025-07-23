using System.Collections.Generic;
using UnityEngine;

namespace Items.Weapons
{
    [CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObjects/WeaponData")]
    public class WeaponData : ScriptableObject
    {
        [Header("Prefabs")] 
        public GameObject weaponPrefab;
        
        [Header("Stats")] 
        public float bulletDistance;
        public int startingAmmo;
        public float reloadDuration;
        public float fireRate;
        public float xRecoil;
        public float yRecoil;
        public float recoilSnappiness;
        public float recoilCancelSnappiness;
        public float recoilCancelPower;
        public bool isRaycast;
        public bool isFullAuto;
        
        [Space] [Header("Sounds")]
        public AudioClip gunShotFirst;
        public List<AudioClip> gunShot;
        public AudioClip gunShotTail;
        public List<AudioClip> emptyMag;
        public List<AudioClip> ejectMag;
        public List<AudioClip> insertMag;
        public List<AudioClip> cockGun;
        public List<AudioClip> adsClick;
        public float cockDelay;
        public float shotVolume;

        [Space] [Header("FOV Animation")]
        public float fovReductionOnAim;
        public float fovReductionOnSprint;
        public float fovReductionOnSlide;

        [Space] [Header("Weapon Animation")]
        public float gunAnimationCosSpeed;
        public float gunAnimationCosSpeedSprinting;
        public float gunAnimationSinSpeed;
        public float gunAnimationSinSpeedSprinting;
        
        [Space]
        public float gunAnimationDistance;
        public float gunAnimationDistanceSprinting;
        public float gunAnimationSmoothTime;
        
        [Space]
        public float gunAnimationLateralDistance;
        public float gunAnimationLateralSmoothTime;
        
        [Space]
        public float gunAnimationIdleDistance;
        public float gunAnimationIdleSpeed;
        
        [Space]
        public float gunAnimationSizeSpeed;
        
        [Space]
        public Vector3 reloadPosition;
        public Vector3 adsPosition;
        public Vector3 hipSize;
        public Vector2 adsSize;
    }
}
