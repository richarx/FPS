using UnityEngine;

namespace Player.Scripts
{
    public class PlayerAnimateGun : MonoBehaviour
    {
        [SerializeField] private RectTransform gun;
        [SerializeField] private Vector3 reloadPosition;
        [SerializeField] private Vector3 adsPosition;
        [SerializeField] private Vector2 adsSize;
        [SerializeField] private float transitionFalloff;
        [SerializeField] private float jumpImpulsePower;
        [SerializeField] private float landingImpulsePower;
        [SerializeField] private float fallOffsetPower;

        private Animator graphics;
        private PlayerStateMachine player;

        private float sinTimer = 0.0f;
        private float cosTimer = 0.0f;
        private float idleTimer = 0.0f;

        private Vector3 basePosition;
        private Vector3 targetPosition;
        private Vector3 offsetPosition;
        private Vector3 velocity;
        private float offsetVelocity;
        private Vector2 sizeVelocity;

        private float baseLateralPosition;
        private float targetLateralPosition;
        private float lateralVelocity;

        private void Start()
        {
            graphics = gun.GetComponent<Animator>();
            basePosition = gun.localPosition;
            offsetPosition = Vector3.zero;
            player = GetComponent<PlayerStateMachine>();
            player.playerGun.OnShoot.AddListener(() =>
            {
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
            UpdateTimers();

            bool isGrounded = player.playerJump.isGrounded;

            if (player.isReloading)
                ReloadGun();
            else if (player.isAiming)
                AimDownSight();
            else if (player.isShooting)
                ShootingGun();
            else if (player.IsMoving() && isGrounded)
                RunningGun();
            else if (isGrounded)
                IdleGun();
            else
                targetPosition = basePosition;
            
            Jump(isGrounded);

            UpdateLateralPosition();
            ApplyMovement();
            UpdateAnimator();
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
            Vector2 target = player.isAiming ? adsSize : new Vector2(880.0f, 640.0f);
            Vector2 newSize = Vector2.SmoothDamp(gun.sizeDelta, target, ref sizeVelocity, player.playerData.gunAnimationSizeSpeed);
            gun.sizeDelta = newSize;
            
            if (!graphics.GetCurrentAnimatorStateInfo(0).IsName("Shoot") && !graphics.GetCurrentAnimatorStateInfo(0).IsName("Shoot_ADS"))
                graphics.Play(player.isAiming || newSize.x >= transitionFalloff ? "Idle_ADS" : "Idle");
        }

        private void UpdateTimers()
        {
            sinTimer += Time.deltaTime * player.playerData.gunAnimationSinSpeed;
            cosTimer += Time.deltaTime * player.playerData.gunAnimationCosSpeed;
            idleTimer += Time.deltaTime * player.playerData.gunAnimationIdleSpeed;
            
            if (sinTimer >= 360.0f)
                sinTimer -= 360.0f;

            if (cosTimer >= 360.0f)
                cosTimer -= 360.0f;
            
            if (idleTimer >= 360.0f)
                idleTimer -= 360.0f;
        }

        private void ApplyMovement()
        {
            gun.localPosition = Vector3.SmoothDamp(gun.localPosition, targetPosition + offsetPosition, ref velocity, player.playerData.gunAnimationSmoothTime);
        }

        private void IdleGun()
        {
            float y = Mathf.Cos(Tools.DegreeToRadian(idleTimer)) * player.playerData.gunAnimationIdleDistance;
            targetPosition = basePosition + new Vector3(0.0f, y, 0.0f);
        }
        
        private void ReloadGun()
        {
            Vector3 position = player.isAiming ? adsPosition : basePosition;
            targetPosition = position + reloadPosition;
        }
        
        private void ShootingGun()
        {
            targetPosition = basePosition;
        }
        
        private void AimDownSight()
        {
            targetPosition = adsPosition;
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
            
            if (!player.isShooting && player.IsMoving())
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
