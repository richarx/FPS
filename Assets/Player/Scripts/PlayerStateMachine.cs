using Backpack;
using Data;
using Inventory;
using Pause_Menu;
using Tools_and_Scripts;
using UnityEngine;

namespace Player.Scripts
{
    public class PlayerStateMachine : MonoBehaviour
    {
        public Transform orientationPivot;
        public PlayerData playerData;
        public VFXData vfxData;
        public SurfaceData surfaceData;
        public Transform muzzleFlashHolder;
        public Light muzzleFlashLight;
        public BackpackDisplay backpackDisplay;

        public static PlayerStateMachine instance;

        // Behaviour States
        public PlayerRun playerRun;
        public PlayerJump playerJump;
        public PlayerSlam playerSlam = new PlayerSlam();
        public PlayerCrouch playerCrouch = new PlayerCrouch();
        public PlayerSlide playerSlide = new PlayerSlide();
        public PlayerLocked playerLocked = new PlayerLocked();
        public PlayerScanning playerScanning = new PlayerScanning();
        public PlayerBackpack playerBackpack = new PlayerBackpack();

        public IPlayerBehaviour currentBehaviour;
        
        public Vector3 position => transform.position;
        public bool isShooting => playerShootGun.isShooting;
        public bool isAiming => playerAiming.IsAiming;
        public bool isReloading => playerAmmo.isReloading;
        public bool isSliding => currentBehaviour.GetBehaviourType() == BehaviourType.Slide;
        public bool isLocked => currentBehaviour.GetBehaviourType() == BehaviourType.Locked || playerRun.IsSkippingFrame;
        public bool isScanning => currentBehaviour.GetBehaviourType() == BehaviourType.Scanning;
        public bool isBackpackOpen => currentBehaviour.GetBehaviourType() == BehaviourType.Backpack;

        [HideInInspector] public Vector2 moveInput;
        [HideInInspector] public Vector3 moveVelocity;

        [HideInInspector] public bool canBeInterruptedByLanding = true;

        [HideInInspector] public Rigidbody rb;
        [HideInInspector] public PlayerGun playerGun;
        [HideInInspector] public PlayerAiming playerAiming;
        [HideInInspector] public PlayerAmmo playerAmmo;
        [HideInInspector] public PlayerShootGun playerShootGun;
        [HideInInspector] public PlayerGunKickback playerGunKickback;
        [HideInInspector] public Scanner.Scanner scanner;
        [HideInInspector] public BackpackStorage backpackStorage;
        [HideInInspector] public PlayerArms playerArms;
        
        private void Awake()
        {
            instance = this;
            rb = GetComponent<Rigidbody>();
            playerGun = GetComponent<PlayerGun>();
            playerAiming = GetComponent<PlayerAiming>();
            playerAmmo = GetComponent<PlayerAmmo>();
            playerShootGun = GetComponent<PlayerShootGun>();
            playerGunKickback = GetComponent<PlayerGunKickback>();
            scanner = GetComponent<Scanner.Scanner>();
            backpackStorage = GetComponent<BackpackStorage>();
            playerArms = GetComponent<PlayerArms>();

            playerRun = new PlayerRun(this);
            playerJump = new PlayerJump(this);
        }

        private void Start()
        {
            if (!Application.isEditor)
                Cursor.visible = false;
            
            currentBehaviour = playerRun;
            currentBehaviour.StartBehaviour(this, BehaviourType.Run);
        }
        
        private void Update()
        {
            if (PauseMenu.instance.IsPaused)
                return;
            
            PlayerInputs.UpdateInputBuffers();
            moveInput = PlayerInputs.GetMoveDirection();
            
            currentBehaviour.UpdateBehaviour(this);
        }

        private void FixedUpdate()
        {
            currentBehaviour.FixedUpdateBehaviour(this);
        }
        
        public void ChangeBehaviour(IPlayerBehaviour newBehaviour)
        {
            if (newBehaviour == null || newBehaviour == currentBehaviour)
                return;

            BehaviourType previous = currentBehaviour.GetBehaviourType();
            currentBehaviour.StopBehaviour(this, newBehaviour.GetBehaviourType());
            currentBehaviour = newBehaviour;
            
            currentBehaviour.StartBehaviour(this, previous);
        }
        
        public void ApplyMovement()
        {
            rb.velocity = moveVelocity;
        }

        public bool IsMoving(float maxVelocity = 0.01f)
        {
            return ComputeGroundMoveVelocity().magnitude >= maxVelocity;
        }

        public Vector3 ComputeGroundMoveInputDirection()
        {
            return (moveInput.x * orientationPivot.right + moveInput.y * orientationPivot.forward).normalized;
        }

        public Vector3 ComputeGroundMoveVelocity()
        {
            Vector3 velocity = moveVelocity;
            velocity.y = 0.0f;

            return velocity;
        }

        public Vector3 ComputeGroundNormal()
        {
            return playerRun.isOnSlope ? playerRun.slopeHit.normal : Vector3.up;
        }
    }
}
