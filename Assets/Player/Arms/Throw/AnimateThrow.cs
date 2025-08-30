using Player.Scripts;
using UnityEngine;

namespace Weapons.Throw
{
    public class AnimateThrow : MonoBehaviour
    {
        [SerializeField] private RectTransform hand;
        
        [Space]
        [SerializeField] private float throwImpulsePower;
        
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
        public float gunAnimationIdleDistance;
        public float gunAnimationIdleSpeed;

        [Space]
        public Vector3 reloadPosition;
        public Vector3 hipPosition;
        
        private PlayerStateMachine player;

        private float sinTimer = 0.0f;
        private float cosTimer = 0.0f;
        private float idleTimer = 0.0f;
        
        private Vector3 targetPosition;
        private Vector3 offsetPosition;
        private Vector3 velocity;
        private float offsetVelocity;
        
        private float lastThrowTimestamp;

        private bool isUnEquipping;
        
        private void Start()
        {
            offsetPosition = Vector3.zero;
            
            player = PlayerStateMachine.instance;
            
            targetPosition = hipPosition;
            hand.localPosition = targetPosition + reloadPosition;
            
            player.playerTools.OnThrowItem.AddListener(OnThrow);
            
            player.playerArms.OnUnEquipTool.AddListener(() => isUnEquipping = true);
            
            player.playerJump.OnJump.AddListener(() => { offsetPosition.y = -jumpImpulsePower; });
            player.playerJump.OnGroundedChanged.AddListener((isGrounded, impactPower) =>
            {
                if (isGrounded)
                    offsetPosition.y = -landingImpulsePower * impactPower;
            });
        }
        
        private void Update()
        {
            if (player.playerArms.currentArmType != PlayerArms.ArmType.Throw)
                return;
            
            UpdateTimers();

            bool isGrounded = player.playerJump.isGrounded;
            bool isThrowing = IsThrowing();
            bool isReloadingThrow = Time.time - lastThrowTimestamp >= 0.3f && Time.time - lastThrowTimestamp <= 0.6f;
            
            if (isReloadingThrow || isUnEquipping || player.isLocked || player.isScanning || player.isBackpackOpen)
                Hide();
            else if (isThrowing)
                ThrowItem();
            else if (player.isSliding)
                Slide();
            else if (player.IsMoving() && isGrounded)
                Running();
            else if (isGrounded)
                Idle();
            else
                targetPosition = hipPosition;

            if (!isThrowing)
                Jump(isGrounded);

            ApplyMovement();
        }

        private void OnThrow()
        {
            lastThrowTimestamp = Time.time;
            offsetPosition.y = throwImpulsePower;
        }

        private void ThrowItem()
        {
            offsetPosition.y = Mathf.SmoothDamp(offsetPosition.y, 0.0f, ref offsetVelocity, 0.3f);
        }

        private bool IsThrowing()
        {
            return Time.time - lastThrowTimestamp <= 0.3f;
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
            if (hand == null)
                return;
            
            hand.localPosition = Vector3.SmoothDamp(hand.localPosition, targetPosition + offsetPosition, ref velocity, gunAnimationSmoothTime);
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
        
        private void Idle()
        {
            float y = Mathf.Cos(Tools.DegreeToRadian(idleTimer)) * gunAnimationIdleDistance;
            targetPosition = hipPosition + new Vector3(0.0f, y, 0.0f);
        }
        
        private void Hide()
        {
            targetPosition = hipPosition + reloadPosition;
        }

        private void Running()
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
    }
}
