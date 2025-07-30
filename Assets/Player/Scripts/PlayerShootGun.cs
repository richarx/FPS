using System;
using Data;
using Dialog_System;
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

        private PlayerStateMachine player;

        public Vector3 shootingPosition => shootingPivot.position;
        public Vector3 shootingDirection => shootingPivot.forward;
        public Vector3 rightDirection => shootingPivot.right;
        
        public bool isShooting => player.playerGun.hasWeapon && Time.time - lastShotTimestamp <= player.playerGun.CurrentWeapon.shotDuration;
        
        private float lastShotTimestamp;
        private bool isInputReset = true;

        private float unlockPlayerTimestamp = -1.0f;
        private bool isLocked => unlockPlayerTimestamp > 0.0f;

        private void Start()
        {
            player = GetComponent<PlayerStateMachine>();
            DialogManager.OnHideDialog.AddListener(() => unlockPlayerTimestamp = Time.time + 0.5f);
        }

        private void Update()
        {
            if (PauseMenu.instance.IsPaused || player.isLocked)
                return;

            if (isLocked)
            {
                if (Time.time >= unlockPlayerTimestamp)
                    unlockPlayerTimestamp = -1.0f;
                return;
            }
            
            if (player.playerAmmo.isReloading || !player.playerGun.hasWeapon)
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
            
            if (player.playerAmmo.IsEmpty)
            {
                if (PlayerInputs.GetRightTrigger())
                    OnShootEmptyMag?.Invoke();
            }
            else
            {
                ShootRaycast();
                player.playerGunKickback.Kickback();
                player.playerAmmo.ConsumeAmmo();
                OnShoot?.Invoke();
            }
        }

        private void ShootRaycast()
        {
            bool hit = Physics.Raycast(shootingPosition, shootingDirection, out RaycastHit hitInfo, player.playerGun.CurrentWeapon.bulletDistance, targetLayer);

            SurfaceData.SurfaceType surfaceType = SurfaceData.SurfaceType.None;
            if (hit)
            {
                Damageable damageable = hitInfo.collider.GetComponent<Damageable>();
                if (damageable != null)
                {
                    Vector3 hitPosition = shootingPosition + (shootingDirection.normalized * hitInfo.distance);
                    damageable.TakeDamage(1.0f, hitPosition);
                    OnHit?.Invoke(hitPosition, SurfaceData.SurfaceType.Enemy);
                    return;
                }
                else
                    surfaceType = SurfaceData.SurfaceType.Wall;
            }
            
            if (surfaceType != SurfaceData.SurfaceType.None)
                OnHit?.Invoke(shootingPosition + (shootingDirection.normalized * hitInfo.distance), surfaceType);
        }

        private bool CanShoot()
        {
            if (!player.playerGun.hasWeapon)
                return false;
            
            if (player.playerGun.CurrentWeapon.isFullAuto)
                return Time.time - lastShotTimestamp >= 1.0f / player.playerGun.CurrentWeapon.fireRate;
            else
                return isInputReset;
        }
    }
}
