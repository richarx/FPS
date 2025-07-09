using Tools_and_Scripts;
using UnityEngine;
using UnityEngine.Events;

namespace Player.Scripts
{
    public class PlayerSlide : IPlayerBehaviour
    {
        public UnityEvent<bool> OnStartSlide = new UnityEvent<bool>();
        public UnityEvent<bool> OnStopSlide = new UnityEvent<bool>();
        
        private bool wasInputReset;
        
        public void StartBehaviour(PlayerStateMachine player, BehaviourType previous)
        {
            Debug.Log("SLIDE");

            Vector3 groundMoveVelocity = player.ComputeGroundMoveVelocity();
            float slideSpeed = Mathf.Max(player.playerData.slidePower, player.moveVelocity.magnitude);
            player.moveVelocity = groundMoveVelocity.normalized * slideSpeed;
            player.ApplyMovement();
            
            wasInputReset = false;
            OnStartSlide?.Invoke(previous == BehaviourType.Crouch);
        }

        public void UpdateBehaviour(PlayerStateMachine player)
        {
            if (player.playerJump.CanJump() && PlayerInputs.GetSouthButton())
            {
                player.playerJump.StartJump(player);
                return;
            }
            
            if (wasInputReset && PlayerInputs.GetEastButton())
            {
                player.ChangeBehaviour(player.playerRun);
                return;
            }
            
            player.playerRun.HandleSprint(player);

            if (player.playerRun.isSprinting)
            {
                player.ChangeBehaviour(player.playerRun);
                return;
            }

            wasInputReset = true;
        }

        public void FixedUpdateBehaviour(PlayerStateMachine player)
        {
            player.playerJump.CheckCollisions(player);
            player.playerRun.CheckIfSlopeIsWalkable(player);

            HandleFriction(player);
            HandleDirection(player);
            
            player.playerRun.UpdateMoveVelocityOnSlope(player);
            HandleGravity(player);
            player.ApplyMovement();

            if (!player.playerJump.isGrounded)
            {
                player.ChangeBehaviour(player.playerJump);
                return;
            }

            if (!player.IsMoving(player.playerData.slideVelocityThresholdToCrouch))
            {
                player.ChangeBehaviour(player.playerCrouch);
                return;
            }
        }

        private void HandleFriction(PlayerStateMachine player)
        {
            player.moveVelocity -= player.moveVelocity * (player.playerData.slideFriction * Time.fixedDeltaTime);
        }

        private void HandleGravity(PlayerStateMachine player)
        {
            Vector3 groundNormal = player.ComputeGroundNormal();

            Vector3 force = Vector3.ProjectOnPlane(Vector3.down, groundNormal) * player.playerData.slideGravity;

            player.moveVelocity -= force * Time.fixedDeltaTime;
        }

        public void HandleDirection(PlayerStateMachine player)
        {
            if (player.moveInput.magnitude <= 0.8f)
                return;
            
            float currentSpeed = player.moveVelocity.magnitude;
            Vector3 targetVelocity = player.ComputeGroundMoveInputDirection() * currentSpeed;
            Vector3 steerForce = (targetVelocity - player.moveVelocity) * (player.playerData.slideSteerAcceleration * Time.fixedDeltaTime);

            player.moveVelocity += steerForce;
            player.moveVelocity = Vector3.ClampMagnitude(player.moveVelocity, currentSpeed);
        }
        
        public void StopBehaviour(PlayerStateMachine player, BehaviourType next)
        {
            OnStopSlide?.Invoke(next == BehaviourType.Crouch);
        }

        public BehaviourType GetBehaviourType()
        {
            return BehaviourType.Slide;
        }
    }
}
