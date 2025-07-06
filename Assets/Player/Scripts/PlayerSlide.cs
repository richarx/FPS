using Tools_and_Scripts;
using UnityEngine;
using UnityEngine.Events;

namespace Player.Scripts
{
    public class PlayerSlide : IPlayerBehaviour
    {
        public UnityEvent<bool> OnStartSLide = new UnityEvent<bool>();
        public UnityEvent<bool> OnStopSlide = new UnityEvent<bool>();
        
        private bool wasInputReset;
        private Vector2 slideDirection;
        
        public void StartBehaviour(PlayerStateMachine player, BehaviourType previous)
        {
            Debug.Log("SLIDE");

            slideDirection = new Vector2(player.moveVelocity.x, player.moveVelocity.z).normalized;

            player.moveVelocity.x += slideDirection.x * player.playerData.slidePower;
            player.moveVelocity.z += slideDirection.y * player.playerData.slidePower;
            player.ApplyMovement();
            
            wasInputReset = false;
            OnStartSLide?.Invoke(previous == BehaviourType.Crouch);
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
            
            player.playerRun.HandleSlop(player);

            HandleDirection(player);
            
            player.playerRun.UpdateMoveVelocityOnSlope(player);
            
            player.playerJump.HandleGravity(player);

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
        
        public void HandleDirection(PlayerStateMachine player)
        {
            if (!player.playerRun.isOnSlope)
            {
               player.moveVelocity.x = Mathf.MoveTowards(player.moveVelocity.x, 0.0f, player.playerData.slideDeceleration * Time.fixedDeltaTime);
                player.moveVelocity.z = Mathf.MoveTowards(player.moveVelocity.z, 0.0f, player.playerData.slideDeceleration * Time.fixedDeltaTime);
                return;
            }
            
            float direction = player.playerRun.ComputeSlopeMoveDirection(slideDirection * 10.0f).y;

            if (direction >= 0.0f)
            {
                player.moveVelocity.x = Mathf.MoveTowards(player.moveVelocity.x, 0.0f, player.playerData.slideSlopeDeceleration * Time.fixedDeltaTime);
                player.moveVelocity.z = Mathf.MoveTowards(player.moveVelocity.z, 0.0f, player.playerData.slideSlopeDeceleration * Time.fixedDeltaTime);
            }
            else
            {
                float speed = Vector3.Angle(Vector3.up, player.playerRun.slopeHit.normal) * player.playerData.slideSlopeAngleMultiplier;
                player.moveVelocity.x = Mathf.MoveTowards(player.moveVelocity.x, slideDirection.x * player.playerData.slideMaxSpeed * speed, player.playerData.slideSlopeAcceleration * Time.fixedDeltaTime);
                player.moveVelocity.z = Mathf.MoveTowards(player.moveVelocity.z, slideDirection.y * player.playerData.slideMaxSpeed * speed, player.playerData.slideSlopeAcceleration * Time.fixedDeltaTime);
            }
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
