using UnityEngine;

namespace Player.Scripts
{
    public class PlayerAnimateGun : MonoBehaviour
    {
        [SerializeField] private RectTransform gun;
        [SerializeField] private Animator graphics;
        
        [Space]
        [SerializeField] private float transitionFalloff;
        [SerializeField] private float jumpImpulsePower;
        [SerializeField] private float landingImpulsePower;
        [SerializeField] private float fallOffsetPower;

        [Space]
        [SerializeField] private float tiltRotationAmount;
        [SerializeField] private float tiltSmoothTime;
        [SerializeField] private float tiltSnapBackTime;
        [SerializeField] private float minTilt;
        [SerializeField] private float maxTilt;
        
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
        public float gunAnimationSizeSpeed;
        
        [Space]
        public Vector3 reloadPosition;
        public Vector3 hipPosition;
        public Vector3 adsPosition;
        public Vector3 hipSize;
        public Vector2 adsSize;

        private PlayerStateMachine player;

        private float sinTimer = 0.0f;
        private float cosTimer = 0.0f;
        private float idleTimer = 0.0f;

        private Vector3 targetPosition;
        private Vector3 offsetPosition;
        private Vector3 velocity;
        private float offsetVelocity;
        private Vector2 sizeVelocity;

        private float baseLateralPosition;
        private float targetLateralPosition;
        private float lateralVelocity;

        private Quaternion initialRotation;

        private RectTransform rootTransform;
        private RectTransform graphicsTransform;

        private void Start()
        {
            offsetPosition = Vector3.zero;
            initialRotation = Quaternion.identity;
            rootTransform = GetComponent<RectTransform>();
            rootTransform.localPosition = Vector3.zero;
            graphicsTransform = graphics.gameObject.GetComponent<RectTransform>();
            
            player = PlayerStateMachine.instance;
            
            Vector3 position = player.isAiming ? adsPosition : hipPosition;
            gun.localPosition = position + reloadPosition;
            
            player.playerShootGun.OnShoot.AddListener(() =>
            {
                if (graphics != null)
                    graphics.Play(player.isAiming ? "Shoot_ADS" : "Shoot", 0, 0.0f);
            });
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

            if (player.isReloading)
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
            
            Jump(isGrounded);

            UpdateTilt();
            UpdateLateralPosition();
            ApplyMovement();
            UpdateAnimator();
        }

        private void Slide()
        {
            Vector3 position = Random.insideUnitCircle.ToVector3();
            position.x *= slideShakePowerX;
            position.y *= slideShakePowerY;
            
            targetPosition = hipPosition + position;
        }

        private void UpdateTilt()
        {
            if (gun == null)
                return;
            
            float tilt = 0.0f;
            float time = tiltSnapBackTime;

            if (player.isAiming)
            {
                float input = player.moveInput.x;

                tilt = Mathf.Clamp(input * tiltRotationAmount, minTilt, maxTilt);
             
                if (Mathf.Abs(input) >= 0.15f)
                    time = tiltSmoothTime;
            }
            
            Quaternion finalRotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, tilt));

            rootTransform.localRotation = Quaternion.Slerp(rootTransform.localRotation, finalRotation * initialRotation, time * Time.deltaTime);
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

        private void UpdateAnimator()
        {
            if (graphics == null)
                return;
            
            Vector2 target = player.isAiming ? adsSize : hipSize;
            Vector2 newSize = Vector2.SmoothDamp(graphicsTransform.sizeDelta, target, ref sizeVelocity, gunAnimationSizeSpeed);
            graphicsTransform.sizeDelta = newSize;
            
            Debug.Log($"Update Size : {target} / {graphicsTransform.sizeDelta} / {gunAnimationSizeSpeed}");

            bool isPlayingShootingAnimation = graphics.GetCurrentAnimatorStateInfo(0).IsName("Shoot") ||
                                              graphics.GetCurrentAnimatorStateInfo(0).IsName("Shoot_ADS");

            if (!isPlayingShootingAnimation)
                graphics.Play(player.isAiming || newSize.x >= transitionFalloff ? "Idle_ADS" : "Idle");
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
            Debug.Log($"Apply movement : {targetPosition} / {offsetPosition} / {gunAnimationSmoothTime}");
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
