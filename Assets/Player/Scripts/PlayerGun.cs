using Data;
using Enemies;
using Items.Weapons;
using Pause_Menu;
using Tools_and_Scripts;
using UnityEngine;
using UnityEngine.Events;

namespace Player.Scripts
{
    public class PlayerGun : MonoBehaviour
    {
        [SerializeField] private Transform shootingPivot;
        [SerializeField] private Transform gunPivot;
        [SerializeField] private PlayerRecoil playerRecoil;
        [SerializeField] private LayerMask targetLayer;
        
        private PlayerStateMachine player;
        private PlayerAmmo playerAmmo;

        [HideInInspector] public UnityEvent OnEquipWeapon = new UnityEvent();
        [HideInInspector] public UnityEvent<GameObject> OnSwapWeapon = new UnityEvent<GameObject>();
        [HideInInspector] public UnityEvent OnShoot = new UnityEvent();
        [HideInInspector] public UnityEvent OnShootEmptyMag = new UnityEvent();
        [HideInInspector] public UnityEvent OnStartReloading = new UnityEvent();
        [HideInInspector] public UnityEvent OnStopReloading = new UnityEvent();
        [HideInInspector] public UnityEvent<bool> OnChangeAimState = new UnityEvent<bool>();
        [HideInInspector] public UnityEvent<Vector3, SurfaceData.SurfaceType> OnHit = new UnityEvent<Vector3, SurfaceData.SurfaceType>();

        [HideInInspector] public bool isAiming;
        [HideInInspector] public bool isReloading;
        [HideInInspector] public bool isEquippingWeapon;
        public bool isShooting => !CanShoot();
        public bool hasWeapon => currentWeapon != null;
        
        private float lastShotTimestamp;
        private float startReloadTimestamp;
        private float startEquipWeaponTimestamp;
        private bool hasWeaponBeenSwapped;

        private WeaponData currentWeapon;
        public WeaponData CurrentWeapon => currentWeapon;
        
        private void Start()
        {
            player = GetComponent<PlayerStateMachine>();
            playerAmmo = GetComponent<PlayerAmmo>();
        }

        private void Update()
        {
            if (PauseMenu.instance.IsPaused)
                return;

            if (isReloading && Time.time - startReloadTimestamp >= currentWeapon.reloadDuration)
            {
                isReloading = false;
                playerAmmo.RefillAmmo();
                OnStopReloading?.Invoke();
            }
            
            if (isEquippingWeapon && !hasWeaponBeenSwapped && Time.time - startEquipWeaponTimestamp >= 0.5f)
                SwapWeaponsVisuals();
            
            if (isEquippingWeapon && Time.time - startEquipWeaponTimestamp >= 1.0f)
                isEquippingWeapon = false;

            if (isAiming != PlayerInputs.GetLeftTrigger(isHeld: true))
            {
                isAiming = !isAiming;
                OnChangeAimState?.Invoke(isAiming);
            }
            
            if (isReloading || isEquippingWeapon || currentWeapon == null)
                return;

            if (CanShoot() && PlayerInputs.GetRightTrigger(isHeld: true))
                Shoot();
            else if (PlayerInputs.GetWestButton())
                ReloadGun();
        }

        private void ReloadGun()
        {
            isReloading = true;
            startReloadTimestamp = Time.time;
            OnStartReloading?.Invoke();
        }

        private void Shoot()
        {
            lastShotTimestamp = Time.time;
            
            if (playerAmmo.IsEmpty)
            {
                if (PlayerInputs.GetRightTrigger())
                    OnShootEmptyMag?.Invoke();
            }
            else
            {
                ShootRaycast();
                Kickback();
                playerAmmo.ConsumeAmmo();
                OnShoot?.Invoke();
            }
        }

        private void Kickback()
        {
            float xKickBack = Tools.RandomPositiveOrNegative(Tools.RandomAround(currentWeapon.xRecoil, 0.3f));
            float yKickBack = Tools.RandomAround(currentWeapon.yRecoil, 0.15f);

            if (isAiming)
            {
                xKickBack *= 0.3f;
                yKickBack *= 0.3f;
            }
            
            playerRecoil.KickBack(xKickBack, yKickBack);
        }
        
        private void ShootRaycast()
        {
            Vector3 position = shootingPivot.position;
            Vector3 direction = shootingPivot.forward;
            RaycastHit[] hit = Physics.RaycastAll(position, direction, currentWeapon.bulletDistance, targetLayer);

            SurfaceData.SurfaceType surfaceType = SurfaceData.SurfaceType.None;
            for (int i = 0; i < hit.Length; i++)
            {
                
                Damageable damageable = hit[i].collider.GetComponent<Damageable>();
                if (damageable != null)
                {
                    Vector3 hitPosition = position + (direction.normalized * hit[i].distance);
                    damageable.TakeDamage(1.0f, hitPosition);
                    OnHit?.Invoke(hitPosition, SurfaceData.SurfaceType.Enemy);
                    return;
                }
                else
                    surfaceType = SurfaceData.SurfaceType.Wall;
            }
            
            if (surfaceType != SurfaceData.SurfaceType.None)
                OnHit?.Invoke(position + (direction.normalized * hit[0].distance), surfaceType);
        }

        private bool CanShoot()
        {
            return currentWeapon != null && Time.time - lastShotTimestamp >= 1.0f / currentWeapon.fireRate;
        }

        public void EquipNewWeapon(WeaponData weapon)
        {
            isEquippingWeapon = true;
            startEquipWeaponTimestamp = Time.time;
            hasWeaponBeenSwapped = false;
            currentWeapon = weapon;
            OnEquipWeapon?.Invoke();
        }

        private void SwapWeaponsVisuals()
        {
            if (gunPivot.childCount > 0)
                Destroy(gunPivot.GetChild(0).gameObject);

            Transform newWeapon = Instantiate(currentWeapon.weaponPrefab, Vector3.zero, Quaternion.identity, gunPivot).transform;
            newWeapon.localPosition = new Vector3(0.0f, -600.0f, 0.0f);
            hasWeaponBeenSwapped = true;
            OnSwapWeapon?.Invoke(newWeapon.gameObject);
        }
    }
}
