using Tools_and_Scripts;
using UnityEngine;

namespace Player.Scripts
{
    public class PlayerRun : IPlayerBehaviour
    {
        private RaycastHit slopeHit;
        public bool isOnSlope;

        public void StartBehaviour(PlayerStateMachine player, BehaviourType previous)
        {
            Debug.Log("RUN");
        }

        public void UpdateBehaviour(PlayerStateMachine player)
        {
            if (player.playerJump.CanJump() && PlayerInputs.GetSouthButton())
            {
                player.playerJump.StartJump(player);
                return;
            }
        }

        public void FixedUpdateBehaviour(PlayerStateMachine player)
        {
            player.playerJump.CheckCollisions(player, player.playerData);
            
            HandleDirection(player);
            
            player.playerJump.HandleGravity(player);

            player.ApplyMovement();
            
            if (!player.playerJump.isGrounded)
                player.ChangeBehaviour(player.playerJump);
        }

        public bool CheckIsOnSlope(PlayerStateMachine player)
        {
            if (Physics.Raycast(player.position + (Vector3.up * 0.1f), Vector3.down, out slopeHit, 0.3f, ~player.playerData.layersToIgnoreForGroundCheck))
            {
                float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
                return angle != 0;
            }

            return false;
        }

        public bool IsSlopeWalkable(PlayerStateMachine player)
        {
            if (!isOnSlope)
                return false;
            
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < player.playerData.maxSlopeAngle && angle != 0.0f;
        }

        private Vector3 ComputeSlopeMoveDirection(Vector3 moveDirection)
        {
            if (!isOnSlope)
                return moveDirection.normalized;
            
            return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
        }
        
        public Vector3 ComputeSlopeFallDirection()
        {
            if (!isOnSlope)
                return Vector3.down;
            
            Vector3 acrossSlope = Vector3.Cross(Vector3.up, slopeHit.normal);
            Vector3 downSlope = Vector3.Cross(acrossSlope, slopeHit.normal);
            
            return downSlope.normalized;
        }

        public void HandleDirection(PlayerStateMachine player)
        {
            bool isSlopeWalkable = IsSlopeWalkable(player);
            if (isOnSlope && !isSlopeWalkable)
            {
                
                return;
            }
                
            Vector3 move = (player.moveInput.x * player.orientationPivot.right + player.moveInput.y * player.orientationPivot.forward).normalized;
            move *= player.isAiming ? player.playerData.groundMaxSpeedAiming : player.playerData.groundMaxSpeed;
            
            if (player.moveInput.magnitude <= 0.05f)
            {
                player.moveVelocity.x = Mathf.MoveTowards(player.moveVelocity.x, 0.0f, player.playerData.groundDeceleration * Time.fixedDeltaTime);
                player.moveVelocity.z = Mathf.MoveTowards(player.moveVelocity.z, 0.0f, player.playerData.groundDeceleration * Time.fixedDeltaTime);
            }
            else
            {
                player.moveVelocity.x = Mathf.MoveTowards(player.moveVelocity.x, move.x, player.playerData.groundAcceleration * Time.fixedDeltaTime);
                player.moveVelocity.z = Mathf.MoveTowards(player.moveVelocity.z, move.z, player.playerData.groundAcceleration * Time.fixedDeltaTime);
            }
            
            if (isOnSlope && isSlopeWalkable)
            {
                float magnitude = player.moveVelocity.magnitude;
                player.moveVelocity = ComputeSlopeMoveDirection(player.moveVelocity) * magnitude;
            }    
        }

        public void StopBehaviour(PlayerStateMachine player, BehaviourType next)
        {
            slopeHit = new RaycastHit();
            isOnSlope = false;
        }

        public BehaviourType GetBehaviourType()
        {
            return BehaviourType.Run;
        }
    }
}
