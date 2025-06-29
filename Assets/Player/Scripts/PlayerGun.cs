using System.Collections.Generic;
using Enemies;
using Tools_and_Scripts;
using UnityEngine;
using UnityEngine.Events;

namespace Player.Scripts
{
    public class PlayerGun : MonoBehaviour
    {
        [SerializeField] private Transform shootingPivot;
        [SerializeField] private PlayerLook playerLook;
        [SerializeField] private float xRecoil;
        [SerializeField] private float yRecoil;
        [SerializeField] private GameObject mainImpact;
        [SerializeField] private List<GameObject> secondaryImpacts;
        [SerializeField] private LayerMask targetLayer;
        
        private PlayerStateMachine player;

        [HideInInspector] public UnityEvent OnShoot = new UnityEvent();

        public bool isShooting => !CanShoot();
        
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
            Kickback();
            lastShotTimestamp = Time.time;
        }

        private void Kickback()
        {
            float xKickBack = Random.Range(-xRecoil, xRecoil);
            float yKickBack = Random.Range(0.0f, yRecoil);
            playerLook.KickBack(xKickBack, yKickBack);
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
                    SpawnImpact(position, direction, hit[i].distance);
                }
            }
        }

        private void SpawnImpact(Vector3 position, Vector3 direction, float distance)
        {
            Vector3 impactPosition = position + (direction.normalized * distance);
            
            int randomIndex = Random.Range(0, secondaryImpacts.Count);
            Instantiate(secondaryImpacts[randomIndex], impactPosition + (Random.insideUnitSphere * 0.5f), Quaternion.identity);
            Instantiate(mainImpact, impactPosition, Quaternion.identity);
        }

        private bool CanShoot()
        {
            return Time.time - lastShotTimestamp >= 1.0f / player.playerData.fireRate;
        }
    }
}
