using System;
using System.Collections;
using Data;
using Dialog_System;
using Enemies;
using Items.Weapons;
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
        [SerializeField] private TrailRenderer trailPrefab;
        
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
                StartCoroutine(ShootDependingOnWeapon());
        }

        private IEnumerator ShootDependingOnWeapon()
        {
            WeaponData data = player.playerGun.CurrentWeapon;

            int shotsCount = data.useBurstShot ? Mathf.Min(player.playerAmmo.CurrentAmmo, data.bulletsPerBurst) : 1;
            int bulletsCount = data.useSpreadShot ? data.bulletsPerSpread : 1;

            for (int i = 0; i < shotsCount; i++)
            {
                for (int j = 0; j < bulletsCount; j++)
                {
                    ShootRaycast(data.useSpreadShot && j > 0 ? ComputeSpreadShootingDirection(data) : shootingDirection);
                }
                
                player.playerGunKickback.Kickback();
                player.playerAmmo.ConsumeAmmo();
                OnShoot?.Invoke();

                if (data.useBurstShot)
                    yield return new WaitForSeconds(data.timeBetweenBurstBullets);
            }
        }

        private Vector3 ComputeSpreadShootingDirection(WeaponData data)
        {
            float distance = 10.0f;

            Vector3 distancePosition = shootingPosition + shootingDirection * distance;
            distancePosition += shootingPivot.right * Tools.RandomPositiveOrNegative(data.maxSpread.x) + Vector3.up * Tools.RandomPositiveOrNegative(data.maxSpread.y);

            return (distancePosition - shootingPosition).normalized;
        }

        private void ShootRaycast(Vector3 direction)
        {
            float distance = player.playerGun.CurrentWeapon.bulletDistance;
            bool hit = Physics.Raycast(shootingPosition, direction, out RaycastHit hitInfo, distance, targetLayer);

            Damageable damageable = hit ? hitInfo.collider.GetComponent<Damageable>() : null;

            StartCoroutine(ShootTrailRenderer(shootingPosition, direction, hit ? hitInfo.distance : distance, ComputeSurfaceType(hit, damageable != null), damageable));
        }

        private SurfaceData.SurfaceType ComputeSurfaceType(bool hasHit, bool hasHitEnemy)
        {
            if (hasHitEnemy)
                return SurfaceData.SurfaceType.Enemy;

            if (hasHit)
                return SurfaceData.SurfaceType.Wall;

            return SurfaceData.SurfaceType.None;
        }

        private IEnumerator ShootTrailRenderer(Vector3 startPosition, Vector3 direction, float distance, SurfaceData.SurfaceType surfaceType, Damageable damageable)
        {
            Vector3 targetPosition = startPosition + direction.normalized * distance;
            
            TrailRenderer trail = Instantiate(trailPrefab, startPosition, Quaternion.identity);

            startPosition = ComputeTrailOffset(startPosition);
            
            float timer = 0.0f;
            float duration = trail.time;
            while (timer <= duration)
            {
                trail.transform.position = Vector3.Lerp(startPosition, targetPosition, Tools.NormalizeValue(timer, 0.0f, duration));
                yield return null;
                timer += Time.deltaTime;
            }

            trail.transform.position = targetPosition;
            Destroy(trail.gameObject, trail.time);
            
            if (damageable != null)
                damageable.TakeDamage(1.0f, targetPosition);

            if (surfaceType != SurfaceData.SurfaceType.None)
                OnHit?.Invoke(targetPosition, surfaceType);
        }

        private Vector3 ComputeTrailOffset(Vector3 position)
        {
            if (player.isAiming)
                return position + Vector3.down * 1;

            Vector3 offset = player.playerGun.CurrentWeapon.bulletTrailOffset;
            
            return position + shootingPivot.forward * offset.z + shootingPivot.right * offset.x + Vector3.up * offset.y;
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
