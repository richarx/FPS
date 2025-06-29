using UnityEngine;

namespace Player.Scripts
{
    public class PlayerAnimateGun : MonoBehaviour
    {
        [SerializeField] private Transform gun;

        private PlayerStateMachine player;
        
        private float sinTimer = 0.0f;
        private float cosTimer = 0.0f;

        private Vector3 basePosition;
        private Vector3 targetPosition;
        private Vector3 velocity;

        private float baseLateralPosition;
        private float targetLateralPosition;
        private float lateralVelocity;
        
        private void Start()
        {
            basePosition = gun.position;
            player = GetComponent<PlayerStateMachine>();
        }

        private void Update()
        {
            UpdateTimers();
            
            if (player.isShooting)
                ShootingGun();
            else if (player.isMoving)
                RunningGun();
            else
                IdleGun();

            UpdateLateralPosition();
            ApplyMovement();
        }

        private void UpdateTimers()
        {
            sinTimer += Time.deltaTime * player.playerData.gunAnimationSinSpeed;
            cosTimer += Time.deltaTime * player.playerData.gunAnimationCosSpeed;
            
            if (sinTimer >= 360.0f)
                sinTimer -= 360.0f;

            if (cosTimer >= 360.0f)
                cosTimer -= 360.0f;
        }

        private void ApplyMovement()
        {
            gun.position = Vector3.SmoothDamp(gun.position, targetPosition, ref velocity, player.playerData.gunAnimationSmoothTime);
        }

        private void IdleGun()
        {
            targetPosition = basePosition;
        }
        
        private void ShootingGun()
        {
            targetPosition = basePosition;
        }

        private void RunningGun()
        {
            float x = Mathf.Sin(Tools.DegreeToRadian(sinTimer)) * player.playerData.gunAnimationDistance;
            float y = Mathf.Cos(Tools.DegreeToRadian(cosTimer)) * player.playerData.gunAnimationDistance;

            targetPosition = basePosition + new Vector3(x, y, 0.0f);
        }

        private void UpdateLateralPosition()
        {
            float target = 0.0f;
            
            if (!player.isShooting && player.isMoving)
            {
                float dot = Vector3.Dot(player.orientationPivot.forward, player.moveVelocity);
                if (Mathf.Abs(dot) <= 0.9f)
                {
                    float angle = Vector3.SignedAngle(player.orientationPivot.forward, player.moveVelocity, Vector3.up);
                    target = Mathf.Sign(angle) * player.playerData.gunAnimationLateralDistance * (1 - Mathf.Abs(dot));
                }
            }
            
            targetLateralPosition = Mathf.SmoothDamp(targetLateralPosition, target, ref lateralVelocity, player.playerData.gunAnimationLateralSmoothTime);
            targetPosition.x += targetLateralPosition;
        }
    }
}
