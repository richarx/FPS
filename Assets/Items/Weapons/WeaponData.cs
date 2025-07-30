using UnityEngine;

namespace Items.Weapons
{
    [CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObjects/WeaponData")]
    public class WeaponData : ScriptableObject
    {
        [Header("Prefabs")] 
        public GameObject weaponPrefab;
        public GameObject lootPrefab;
        
        [Space] [Header("Stats")] 
        public float bulletDistance;
        public int startingAmmo;
        public float fireRate;
        public float reloadDuration;
        
        [Space]
        public bool isRaycast;
        public bool isFullAuto;

        [Space] [Header("Spread Shot")]
        public bool useSpreadShot;
        public int bulletsPerSpread;
        public Vector2 maxSpread;
        
        [Space] [Header("Burst Shot")]
        public bool useBurstShot;
        public int bulletsPerBurst;
        public float timeBetweenBurstBullets;
        
        [Space] [Header("Recoil")]
        public float xRecoil;
        public float yRecoil;
        public float shotDuration;
        public float recoilSnappiness;
        public float recoilCancelSnappiness;
        public float recoilCancelPower;

        [Space] [Header("Aim Down Sight")]
        public float aimDownSightSpeed;
        public float fovReductionOnAim;
    }
}
