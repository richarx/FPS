using System;
using Data;
using Enemies;
using Pause_Menu;
using Tools_and_Scripts;
using UnityEngine;
using UnityEngine.Events;

namespace Player.Scripts
{
    public class PlayerShootGun : MonoBehaviour
    {
        [SerializeField] private Transform shootingPivot;
        [SerializeField] private LayerMask targetLayer;
        
        [HideInInspector] public UnityEvent OnShoot = new UnityEvent();
        [HideInInspector] public UnityEvent OnShootEmptyMag = new UnityEvent();
        [HideInInspector] public UnityEvent<Vector3, SurfaceData.SurfaceType> OnHit = new UnityEvent<Vector3, SurfaceData.SurfaceType>();

        private PlayerGun playerGun;
        private PlayerAmmo playerAmmo;
        private PlayerGunKickback playerGunKickback;

        public Vector3 shootingPosition => shootingPivot.position;
        public Vector3 shootingDirection => shootingPivot.forward;
        public Vector3 rightDirection => shootingPivot.right;
        
        public bool isShooting => Time.time - lastShotTimestamp <= 0.2f;
        
        private float lastShotTimestamp;
        private bool isInputReset = true;

        private void Start()
        {
            playerGun = GetComponent<PlayerGun>();
            playerAmmo = GetComponent<PlayerAmmo>();
            playerGunKickback = GetComponent<PlayerGunKickback>();
        }

        private void Update()
        {
            if (PauseMenu.instance.IsPaused)
                return;
            
            if (playerAmmo.isReloading || !playerGun.hasWeapon)
                return;

            if (CanShoot() && PlayerInputs.GetRightTrigger(isHeld: true))
            {
                Shoot();
                isInputReset = false;
            }
            
            if (!isInputReset && !PlayerInputs.GetRightTrigger(isHeld: true))
                isInputReset = true;
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
                playerGunKickback.Kickback();
                playerAmmo.ConsumeAmmo();
                OnShoot?.Invoke();
            }
        }

        private void ShootRaycast()
        {
            RaycastHit[] hit = Physics.RaycastAll(shootingPosition, shootingDirection, playerGun.CurrentWeapon.bulletDistance, targetLayer);

            SurfaceData.SurfaceType surfaceType = SurfaceData.SurfaceType.None;
            for (int i = 0; i < hit.Length; i++)
            {
                Damageable damageable = hit[i].collider.GetComponent<Damageable>();
                if (damageable != null)
                {
                    Vector3 hitPosition = shootingPosition + (shootingDirection.normalized * hit[i].distance);
                    damageable.TakeDamage(1.0f, hitPosition);
                    OnHit?.Invoke(hitPosition, SurfaceData.SurfaceType.Enemy);
                    return;
                }
                else
                    surfaceType = SurfaceData.SurfaceType.Wall;
            }
            
            if (surfaceType != SurfaceData.SurfaceType.None)
                OnHit?.Invoke(shootingPosition + (shootingDirection.normalized * hit[0].distance), surfaceType);
        }

        private bool CanShoot()
        {
            if (!playerGun.hasWeapon)
                return false;
            
            if (playerGun.CurrentWeapon.isFullAuto)
                return Time.time - lastShotTimestamp >= 1.0f / playerGun.CurrentWeapon.fireRate;
            else
                return isInputReset;
        }
    }
}
