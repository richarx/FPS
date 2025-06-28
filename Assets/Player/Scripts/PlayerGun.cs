using Tools_and_Scripts;
using UnityEngine;
using UnityEngine.Events;

namespace Player.Scripts
{
    public class PlayerGun : MonoBehaviour
    {
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
            lastShotTimestamp = Time.time;
        }

        private bool CanShoot()
        {
            return Time.time - lastShotTimestamp >= 1.0f / player.playerData.fireRate;
        }
    }
}
