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
        public float shotDuration;
        public float recoilSnappiness;
        public float recoilCancelSnappiness;
        public float recoilCancelPower;
        public float aimDownSightSpeed;
        public bool isRaycast;
        public bool isFullAuto;
        
        [Space] [Header("FOV Animation")]
        public float fovReductionOnAim;
        public float fovReductionOnSprint;
        public float fovReductionOnSlide;
    }
}
