using UnityEngine;

namespace Player.Scripts
{
    public class PlayerAnimateGun : MonoBehaviour
    {
        [SerializeField] private RectTransform gun;
        [SerializeField] private Vector3 adsPosition;
        [SerializeField] private Vector2 adsSize;
        [SerializeField] private float transitionFalloff;

        private Animator graphics;
        private PlayerStateMachine player;

        private float sinTimer = 0.0f;
        private float cosTimer = 0.0f;
        private float idleTimer = 0.0f;

        private Vector3 basePosition;
        private Vector3 targetPosition;
        private Vector3 velocity;
        private Vector2 sizeVelocity;

        private float baseLateralPosition;
        private float targetLateralPosition;
        private float lateralVelocity;

        private void Start()
        {
            graphics = gun.GetComponent<Animator>();
            basePosition = gun.localPosition;
            player = GetComponent<PlayerStateMachine>();
            player.playerGun.OnShoot.AddListener(() =>
            {
                if (!player.isAiming)
                    graphics.Play("Shoot", 0, 0.0f);
            });
        }

        private void Update()
        {
            UpdateTimers();

            if (player.isAiming)
                AimDownSight();
            else if (player.isShooting)
                ShootingGun();
            else if (player.IsMoving())
                RunningGun();
            else
                IdleGun();

            UpdateLateralPosition();
            ApplyMovement();
            UpdateAnimator();
        }

        private void UpdateAnimator()
        {
            Vector2 target = player.isAiming ? adsSize : new Vector2(880.0f, 640.0f);
            Vector2 newSize = Vector2.SmoothDamp(gun.sizeDelta, target, ref sizeVelocity, player.playerData.gunAnimationSizeSpeed);
            gun.sizeDelta = newSize;
            
            if (!graphics.GetCurrentAnimatorStateInfo(0).IsName("Shoot"))
                graphics.Play(player.isAiming || newSize.x >= transitionFalloff ? "Idle_ADS" : "Idle");
            
            //1200 900
           
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
            gun.localPosition = Vector3.SmoothDamp(gun.localPosition, targetPosition, ref velocity, player.playerData.gunAnimationSmoothTime);
        }

        private void IdleGun()
        {
            float y = Mathf.Cos(Tools.DegreeToRadian(idleTimer)) * player.playerData.gunAnimationIdleDistance;
            targetPosition = basePosition + new Vector3(0.0f, y, 0.0f);
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
