using System.Collections.Generic;
using Enemies;
using Pause_Menu;
using Tools_and_Scripts;
using UnityEngine;
using UnityEngine.Events;

namespace Player.Scripts
{
    public class PlayerGun : MonoBehaviour
    {
        [SerializeField] private Transform shootingPivot;
        [SerializeField] private PlayerRecoil playerRecoil;
        [SerializeField] private LayerMask targetLayer;
        
        private PlayerStateMachine player;
        private PlayerAmmo playerAmmo;

        [HideInInspector] public UnityEvent OnShoot = new UnityEvent();
        [HideInInspector] public UnityEvent OnShootEmptyMag = new UnityEvent();
        [HideInInspector] public UnityEvent OnStartReloading = new UnityEvent();
        [HideInInspector] public UnityEvent OnStopReloading = new UnityEvent();
        [HideInInspector] public UnityEvent<bool> OnChangeAimState = new UnityEvent<bool>();
        [HideInInspector] public UnityEvent<Vector3> OnHit = new UnityEvent<Vector3>();

        [HideInInspector] public bool isAiming;
        [HideInInspector] public bool isReloading;
        public bool isShooting => !CanShoot();
        
        private float lastShotTimestamp;
        private float startReloadTimestamp;
        
        private void Start()
        {
            player = GetComponent<PlayerStateMachine>();
            playerAmmo = GetComponent<PlayerAmmo>();
        }

        private void Update()
        {
            if (PauseMenu.instance.IsPaused)
                return;

            if (isReloading && Time.time - startReloadTimestamp >= player.playerData.reloadDuration)
            {
                isReloading = false;
                playerAmmo.RefillAmmo();
                OnStopReloading?.Invoke();
            }
            
            if (isAiming != PlayerInputs.GetLeftTrigger(isHeld: true))
            {
                isAiming = !isAiming;
                OnChangeAimState?.Invoke(isAiming);
            }
            
            if (isReloading)
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
            float xKickBack = Tools.RandomPositiveOrNegative(Tools.RandomAround(player.playerData.xRecoil, 0.3f));
            float yKickBack = Tools.RandomAround(player.playerData.yRecoil, 0.15f);

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
            RaycastHit[] hit = Physics.RaycastAll(position, direction, player.playerData.bulletDistance, targetLayer);

            for (int i = 0; i < hit.Length; i++)
            {
                Damageable damageable = hit[i].collider.GetComponent<Damageable>();
                if (damageable != null)
                {
                    damageable.TakeDamage(1.0f);
                    OnHit?.Invoke(position + (direction.normalized * hit[i].distance));
                }
            }
        }

        private bool CanShoot()
        {
            return Time.time - lastShotTimestamp >= 1.0f / player.playerData.fireRate;
        }
    }
}
