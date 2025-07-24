using System.Collections.Generic;
using UnityEngine;

namespace Items.Weapons
{
    [CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObjects/WeaponData")]
    public class WeaponData : ScriptableObject
    {
        [Header("Prefabs")] 
        public GameObject weaponPrefab;
        public GameObject lootPrefab;
        
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
    }
}
