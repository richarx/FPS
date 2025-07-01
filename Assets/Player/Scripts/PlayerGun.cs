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

        [HideInInspector] public UnityEvent OnShoot = new UnityEvent();
        [HideInInspector] public UnityEvent<Vector3> OnHit = new UnityEvent<Vector3>();

        [HideInInspector] public bool isAiming;
        public bool isShooting => !CanShoot();
        
        private float lastShotTimestamp;
        
        private void Start()
        {
            player = GetComponent<PlayerStateMachine>();
        }

        private void Update()
        {
            if (isAiming != PlayerInputs.GetLeftTrigger(isHeld: true))
                isAiming = !isAiming;

            if (CanShoot() && !PauseMenu.instance.IsPaused && PlayerInputs.GetRightTrigger(isHeld: true))
                Shoot();
        }

        private void Shoot()
        {
            OnShoot?.Invoke();
            ShootRaycast();
            Kickback();
            lastShotTimestamp = Time.time;
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
            RaycastHit[] hit = Physics.RaycastAll(position, direction, 50.0f, targetLayer);

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
