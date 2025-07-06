using Data;
using Pause_Menu;
using Tools_and_Scripts;
using UnityEngine;

namespace Player.Scripts
{
    public class PlayerStateMachine : MonoBehaviour
    {
        public Transform orientationPivot;
        public PlayerData playerData;
        public SurfaceData surfaceData;
        
        public static PlayerStateMachine instance;

        public PlayerRun playerRun;
        public PlayerJump playerJump;
        public PlayerCrouch playerCrouch = new PlayerCrouch();
        public PlayerSlide playerSlide = new PlayerSlide();

        public IPlayerBehaviour currentBehaviour;
        
        public Vector3 position => transform.position;
        public bool isShooting => playerGun.isShooting;
        public bool isAiming => playerGun.isAiming;
        public bool isReloading => playerGun.isReloading;

        [HideInInspector] public Vector2 moveInput;
        [HideInInspector] public Vector3 moveVelocity;
        
        [HideInInspector] public bool canBeInterruptedByLanding = true;
        [HideInInspector] public bool isLocked;

        [HideInInspector] public Rigidbody rb;
        [HideInInspector] public PlayerGun playerGun;
        
        private void Awake()
        {
            instance = this;
            rb = GetComponent<Rigidbody>();
            playerGun = GetComponent<PlayerGun>();

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
            
            if (isLocked)
                return;
            
            currentBehaviour.UpdateBehaviour(this);
        }

        private void FixedUpdate()
        {
            if (isLocked)
                return;

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
            Vector3 velocity = moveVelocity;
            velocity.y = 0.0f;

            return velocity.magnitude >= maxVelocity;
        }
    }
}
