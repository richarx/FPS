using UnityEngine;

namespace Player.Scripts
{
    public class PlayerAnimateGun : MonoBehaviour
    {
        [SerializeField] private RectTransform gun;
        
        [Space]
        [SerializeField] private float jumpImpulsePower;
        [SerializeField] private float landingImpulsePower;
        [SerializeField] private float fallOffsetPower;
        
        [Space]
        [SerializeField] private float slideShakePowerX;
        [SerializeField] private float slideShakePowerY;
        
        [Space]
        public float gunAnimationCosSpeed;
        public float gunAnimationCosSpeedSprinting;
        public float gunAnimationSinSpeed;
        public float gunAnimationSinSpeedSprinting;
        
        [Space]
        public float gunAnimationDistance;
        public float gunAnimationDistanceSprinting;
        public float gunAnimationSmoothTime;
        
        [Space]
        public float gunAnimationLateralDistance;
        public float gunAnimationLateralSmoothTime;
        
        [Space]
        public float gunAnimationIdleDistance;
        public float gunAnimationIdleSpeed;

        [Space]
        public float emptyGunShakeDuration;
        public float emptyGunShakePower;
        
        [Space]
        public Vector3 reloadPosition;
        public Vector3 hipPosition;
        public Vector3 adsPosition;

        private PlayerStateMachine player;

        private float sinTimer = 0.0f;
        private float cosTimer = 0.0f;
        private float idleTimer = 0.0f;
        
        private float shakeTimer = 0.0f;

        private Vector3 targetPosition;
        private Vector3 offsetPosition;
        private Vector3 velocity;
        private float offsetVelocity;

        private float baseLateralPosition;
        private float targetLateralPosition;
        private float lateralVelocity;

        private void Start()
        {
            offsetPosition = Vector3.zero;
            
            player = PlayerStateMachine.instance;
            
            Vector3 position = player.isAiming ? adsPosition : hipPosition;
            gun.localPosition = position + reloadPosition;
            
            player.playerShootGun.OnShootEmptyMag.AddListener(() => shakeTimer = emptyGunShakeDuration);
            
            player.playerJump.OnJump.AddListener(() => { offsetPosition.y = -jumpImpulsePower; });
            player.playerJump.OnGroundedChanged.AddListener((isGrounded, impactPower) =>
            {
                if (isGrounded)
                    offsetPosition.y = -landingImpulsePower * impactPower;
            });
        }

        private void Update()
        {
            if (!player.playerGun.hasWeapon)
                return;
            
            UpdateTimers();

            bool isGrounded = player.playerJump.isGrounded;

            if (player.isReloading || player.isLocked)
                HideGun();
            else if (player.isAiming)
                AimDownSight();
            else if (player.isShooting)
                ShootingGun();
            else if (player.isSliding)
                Slide();
            else if (player.IsMoving() && isGrounded)
                RunningGun();
            else if (isGrounded)
                IdleGun();
            else
                targetPosition = hipPosition;

            if (shakeTimer > 0.0f)
                Shake();
            
            Jump(isGrounded);

            UpdateLateralPosition();
            ApplyMovement();
        }

        private void Shake()
        {
            if (shakeTimer > 0.0f)
                shakeTimer -= Time.deltaTime;

            float randomPower = Random.Range(-emptyGunShakePower, emptyGunShakePower);
            targetPosition += Vector3.right * randomPower;
        }

        private void Slide()
        {
            Vector3 position = Random.insideUnitCircle.ToVector3();
            position.x *= slideShakePowerX;
            position.y *= slideShakePowerY;

            targetPosition = hipPosition + position;
        }

        private void Jump(bool isGrounded)
        {
            if (!isGrounded && Time.time - player.playerJump.lastJumpTimeStamp <= 0.3f)
            {
                offsetPosition.y = Mathf.SmoothDamp(offsetPosition.y, 0.0f, ref offsetVelocity, 0.3f);
            }
            else if (!isGrounded && player.moveVelocity.y <= 0.0f)
            {
                offsetPosition.y = fallOffsetPower;
            }
            else if (isGrounded && Time.time - player.playerJump.lastLandingTimeStamp <= 0.3f)
            {
                offsetPosition.y = Mathf.SmoothDamp(offsetPosition.y, 0.0f, ref offsetVelocity, 0.3f);
            }
            else if (player.moveVelocity.y <= 0.0f)
            {
                offsetPosition = Vector3.zero;
            }
        }

        private void UpdateTimers()
        {
            sinTimer += Time.deltaTime * gunAnimationSinSpeed * (player.playerRun.isSprinting ? gunAnimationSinSpeedSprinting : 1.0f);
            cosTimer += Time.deltaTime * gunAnimationCosSpeed * (player.playerRun.isSprinting ? gunAnimationCosSpeedSprinting : 1.0f);
            idleTimer += Time.deltaTime * gunAnimationIdleSpeed;
            
            if (sinTimer >= 360.0f)
                sinTimer -= 360.0f;

            if (cosTimer >= 360.0f)
                cosTimer -= 360.0f;
            
            if (idleTimer >= 360.0f)
                idleTimer -= 360.0f;
        }

        private void ApplyMovement()
        {
            if (gun == null)
                return;
            
            gun.localPosition = Vector3.SmoothDamp(gun.localPosition, targetPosition + offsetPosition, ref velocity, gunAnimationSmoothTime);
        }

        private void IdleGun()
        {
            float y = Mathf.Cos(Tools.DegreeToRadian(idleTimer)) * gunAnimationIdleDistance;
            targetPosition = hipPosition + new Vector3(0.0f, y, 0.0f);
        }
        
        private void HideGun()
        {
            Vector3 position = player.isAiming ? adsPosition : hipPosition;
            targetPosition = position + reloadPosition;
        }
        
        private void ShootingGun()
        {
            targetPosition = hipPosition;
        }
        
        private void AimDownSight()
        {
            targetPosition = adsPosition;
        }

        private void RunningGun()
        {
            float x = Mathf.Sin(Tools.DegreeToRadian(sinTimer)) * gunAnimationDistance;
            float y = Mathf.Cos(Tools.DegreeToRadian(cosTimer)) * gunAnimationDistance;

            if (player.playerRun.isSprinting)
            {
                x *= gunAnimationDistanceSprinting;
                y *= gunAnimationDistanceSprinting;
            }

            targetPosition = hipPosition + new Vector3(x, y, 0.0f);
        }

        private void UpdateLateralPosition()
        {
            float target = 0.0f;
            
            if (!player.isShooting && !player.isAiming && player.IsMoving())
            {
                float dot = Vector3.Dot(player.orientationPivot.forward, player.moveVelocity);
                if (Mathf.Abs(dot) <= 0.9f)
                {
                    float angle = Vector3.SignedAngle(player.orientationPivot.forward, player.moveVelocity, Vector3.up);
                    target = Mathf.Sign(angle) * gunAnimationLateralDistance * (1 - Mathf.Abs(dot));
                }
            }
            
            targetLateralPosition = Mathf.SmoothDamp(targetLateralPosition, target, ref lateralVelocity, gunAnimationLateralSmoothTime);
            targetPosition.x += targetLateralPosition;
        }
    }
}
