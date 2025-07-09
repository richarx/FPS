using Tools_and_Scripts;
using UnityEngine;
using UnityEngine.Events;

namespace Player.Scripts
{
    public class PlayerCrouch : IPlayerBehaviour
    {
        public UnityEvent<bool> OnStartCrouch = new UnityEvent<bool>();
        public UnityEvent<bool> OnStopCrouch = new UnityEvent<bool>();

        private bool wasInputReset;
        
        public void StartBehaviour(PlayerStateMachine player, BehaviourType previous)
        {
            Debug.Log("CROUCH");
            OnStartCrouch?.Invoke(previous == BehaviourType.Slide);

            wasInputReset = false;
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
            
            if (player.playerRun.CanPlayerControlDirection())
                HandleDirection(player);
            
            player.playerRun.UpdateMoveVelocityOnSlope(player);
            
            player.playerJump.HandleGravity(player);

            player.ApplyMovement();
            
            if (!player.playerJump.isGrounded)
                player.ChangeBehaviour(player.playerJump);
        }
        
        public void HandleDirection(PlayerStateMachine player)
        {
            Vector3 move = (player.moveInput.x * player.orientationPivot.right + player.moveInput.y * player.orientationPivot.forward).normalized;
            float speed = ComputeMoveSpeed(player);
            move *= speed;
            
            if (player.moveInput.magnitude <= 0.05f)
            {
                player.moveVelocity.x = Mathf.MoveTowards(player.moveVelocity.x, 0.0f, player.playerData.crouchDeceleration * Time.fixedDeltaTime);
                player.moveVelocity.z = Mathf.MoveTowards(player.moveVelocity.z, 0.0f, player.playerData.crouchDeceleration * Time.fixedDeltaTime);
            }
            else
            {
                player.moveVelocity.x = Mathf.MoveTowards(player.moveVelocity.x, move.x, player.playerData.crouchAcceleration * Time.fixedDeltaTime);
                player.moveVelocity.z = Mathf.MoveTowards(player.moveVelocity.z, move.z, player.playerData.crouchAcceleration * Time.fixedDeltaTime);
            }
        }
        
        private float ComputeMoveSpeed(PlayerStateMachine player)
        {
            return player.isAiming ? player.playerData.crouchMaxSpeedAiming : player.playerData.crouchMaxSpeed;
        }

        public void StopBehaviour(PlayerStateMachine player, BehaviourType next)
        {
            OnStopCrouch?.Invoke(next == BehaviourType.Slide);
        }

        public BehaviourType GetBehaviourType()
        {
            return BehaviourType.Crouch;
        }
    }
}
