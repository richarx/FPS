using Enemies;
using Tools_and_Scripts;
using UnityEngine;
using UnityEngine.Events;

namespace Player.Scripts
{
    public class PlayerGun : MonoBehaviour
    {
        [SerializeField] private Transform shootingPivot;
        [SerializeField] private LayerMask targetLayer;
        
        private PlayerStateMachine player;

        [HideInInspector] public UnityEvent OnShoot = new UnityEvent();

        private float lastShotTimestamp;
        
        private void Start()
        {
            player = GetComponent<PlayerStateMachine>();
        }

        private void Update()
        {
            if (CanShoot() && PlayerInputs.GetRightTrigger(isHeld: true))
                Shoot();
        }

        private void Shoot()
        {
            OnShoot?.Invoke();
            ShootRaycast();
            lastShotTimestamp = Time.time;
        }

        private void ShootRaycast()
        {
            RaycastHit[] hit = Physics.RaycastAll(shootingPivot.position, shootingPivot.forward, 50.0f, targetLayer);

            for (int i = 0; i < hit.Length; i++)
            {
                Damageable damageable = hit[i].collider.GetComponent<Damageable>();
                if (damageable != null)
                    damageable.TakeDamage(1.0f);
            }
        }

        private bool CanShoot()
        {
            return Time.time - lastShotTimestamp >= 1.0f / player.playerData.fireRate;
        }
    }
}
