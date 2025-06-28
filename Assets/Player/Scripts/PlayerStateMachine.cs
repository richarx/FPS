using Data;
using Tools_and_Scripts;
using UnityEngine;

namespace Player.Scripts
{
    public class PlayerStateMachine : MonoBehaviour
    {
        public Transform orientationPivot;
        
        public PlayerData playerData;
        
        public static PlayerStateMachine instance;

        private PlayerRun playerGroundMovement = new PlayerRun();
        
        public IPlayerBehaviour currentBehaviour;
        
        public Vector2 position => transform.position;
        
        [HideInInspector] public Vector2 moveInput;
        [HideInInspector] public Vector3 moveVelocity;
        
        [HideInInspector] public bool canBeInterruptedByLanding = true;
        
        [HideInInspector] public bool isLocked;

        [HideInInspector] public Rigidbody rb;
        
        private void Awake()
        {
            instance = this;
            rb = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            if (!Application.isEditor)
                Cursor.visible = false;
            
            currentBehaviour = playerGroundMovement;
            currentBehaviour.StartBehaviour(this, BehaviourType.Run);
        }
        
        private void Update()
        {
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
    }
}
