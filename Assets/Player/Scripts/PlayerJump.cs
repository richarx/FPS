using Data;
using Tools_and_Scripts;
using UnityEngine;
using UnityEngine.Events;

namespace Player.Scripts
{
    public class PlayerJump : IPlayerBehaviour
    {
        private float frameLeftGrounded = float.MinValue;
        public bool isGrounded = true;
        public bool isFalling;
        
        private bool jumpToConsume;

        private bool canBeEndedEarly;
        private bool endedJumpEarly;
        private bool coyoteUsable;
        
        public int remainingJumps;

        public float lastJumpTimeStamp = float.MinValue;
        public float lastLandingTimeStamp = float.MinValue;

        public UnityEvent<bool, float> OnGroundedChanged = new UnityEvent<bool, float>();
        public UnityEvent OnJump = new UnityEvent();

        public PlayerJump(PlayerStateMachine player)
        {
            remainingJumps = player.playerData.maxJumpCount;
        }
        
        public void StartBehaviour(PlayerStateMachine player, BehaviourType previous)
        {
            Debug.Log("JUMP");

            isFalling = !jumpToConsume;
            
            canBeEndedEarly = false;
            endedJumpEarly = false;
        }
        
        public void StartJump(PlayerStateMachine player, bool wallJump = false, bool rollJump = false, bool spotJump = false, bool ladderJump = false)
        {
            jumpToConsume = true;
            isFalling = false;

            player.ChangeBehaviour(player.playerJump);
        }
        
        public bool CanJump()
        {
            return remainingJumps > 0;
        }

        public void UpdateBehaviour(PlayerStateMachine player)
        {
            if (!jumpToConsume && CanJump() && PlayerInputs.GetSouthButton())
            {
                StartJump(player);
                return;
            }

            if (PlayerInputs.GetEastButton())
            {
                player.ChangeBehaviour(player.playerSlam);
                return;
            }
        }

        public void FixedUpdateBehaviour(PlayerStateMachine player)
        {
            bool landed = CheckCollisions(player);

            HandleJump(player);
            HandleDirection(player);
            HandleGravity(player);

            player.ApplyMovement();
            
            if (landed)
                player.ChangeBehaviour(player.playerRun);
        }
        
        public bool CheckCollisions(PlayerStateMachine player)
        {
            bool groundHit = ShootDownRaycasts(player);
            bool ceilingHit = Physics.Raycast(player.position + (Vector3.up * 1.9f), Vector3.up, 0.2f, ~player.playerData.layersToIgnoreForGroundCheck);
            player.playerRun.isOnSlope = isGrounded && player.playerRun.CheckIsOnSlope(player);
            bool isWalkableSlope = player.playerRun.IsSlopeWalkable(player);
            
            if (ceilingHit) 
                player.moveVelocity.y = Mathf.Min(0, player.moveVelocity.y);

            bool landed = !isGrounded && (groundHit || isWalkableSlope);
            bool startsBeingInTheAir = isGrounded && !groundHit && !isWalkableSlope;
            if (landed)
            {
                landed = true;
                isGrounded = true;
                coyoteUsable = true;
                endedJumpEarly = false;
                RefillRemainingJumps(player.playerData);
                lastLandingTimeStamp = Time.time;
                OnGroundedChanged?.Invoke(true, Mathf.Abs(player.moveVelocity.y));
                player.moveVelocity.y = player.playerData.groundingForce;
            }
            else if (startsBeingInTheAir)
            {
                bool isStickingToGround = IsAllowedToStickToTheGround(player) && IsStickingToGround(player);
                if (!isStickingToGround)
                {
                    isGrounded = false;
                    frameLeftGrounded = Time.time;
                    OnGroundedChanged?.Invoke(false, 0);
                }
            }

            return landed;
        }

        private bool IsAllowedToStickToTheGround(PlayerStateMachine player)
        {
            BehaviourType type = player.currentBehaviour.GetBehaviourType();

            return type != BehaviourType.Jump;
        }
        
        private bool IsStickingToGround(PlayerStateMachine player)
        {
            Vector3 position = player.position + (Vector3.up * 0.1f);
            RaycastHit hitInfo;
            
            bool hasHit = Physics.Raycast(position, Vector3.down, out hitInfo, player.playerData.stickToGroundHeight + 0.1f, ~player.playerData.layersToIgnoreForGroundCheck);

            if (!hasHit)
                return false;

            Vector3 newPosition = position + Vector3.down * hitInfo.distance;
            player.rb.MovePosition(newPosition);

            return true;
        }

        private bool ShootDownRaycasts(PlayerStateMachine player)
        {
            Vector3 position = player.position + (Vector3.up * 1.5f);
            
            if (Physics.Raycast(position, Vector3.down, 1.6f, ~player.playerData.layersToIgnoreForGroundCheck))
                return true;

            position += Vector3.up * 0.505f;
            
            Vector3 forward = player.orientationPivot.forward * 0.25f;
            
            if (Physics.Raycast(position + forward, Vector3.down, 2.105f, ~player.playerData.layersToIgnoreForGroundCheck))
                return true;

            if (Physics.Raycast(position - forward, Vector3.down, 2.105f, ~player.playerData.layersToIgnoreForGroundCheck))
                return true;
            
            Vector3 right = player.orientationPivot.right * 0.25f;

            if (Physics.Raycast(position + right, Vector3.down, 2.105f, ~player.playerData.layersToIgnoreForGroundCheck))
                return true;

            if (Physics.Raycast(position - right, Vector3.down, 2.105f, ~player.playerData.layersToIgnoreForGroundCheck))
                return true;
            
            return false;
        }

        public void RefillRemainingJumps(PlayerData data)
        {
            remainingJumps = data.maxJumpCount;
        }

        private bool CanUseCoyote(PlayerData data)
        {
            return coyoteUsable && !isGrounded && Time.time < frameLeftGrounded + data.coyoteTime;
        }
        
        private void HandleJump(PlayerStateMachine player)
        {
            if (canBeEndedEarly && !endedJumpEarly && !isGrounded && !PlayerInputs.GetSouthButton(true, false) && player.rb.velocity.y > 0)
                endedJumpEarly = true;

            if (!jumpToConsume)
                return;

            if ((isGrounded || CanUseCoyote(player.playerData)) || remainingJumps > 0) 
                ExecuteJump(player);
            
            jumpToConsume = false;
        }
        
        private void ExecuteJump(PlayerStateMachine player)
        {
            if (!isGrounded && remainingJumps == player.playerData.maxJumpCount && !CanUseCoyote(player.playerData))
                remainingJumps -= 2;
            else
                remainingJumps -= 1;

            endedJumpEarly = false;
            canBeEndedEarly = true;
            coyoteUsable = false;

            lastJumpTimeStamp = Time.time;
            
            player.moveVelocity.y = player.playerData.jumpForce;
            OnJump?.Invoke();
        }
        
        public void HandleDirectionNoDeceleration(PlayerStateMachine player)
        {            
            float currentSpeed = player.moveVelocity.magnitude;

            Vector3 move = (player.moveInput.x * player.orientationPivot.right + player.moveInput.y * player.orientationPivot.forward).normalized;
            float speed = Mathf.Max(player.playerData.airMaxSpeed, player.ComputeGroundMoveVelocity().magnitude);
            move *= speed;
            
            player.moveVelocity.x = Mathf.MoveTowards(player.moveVelocity.x, move.x, player.playerData.airAcceleration * Time.fixedDeltaTime);
            player.moveVelocity.z = Mathf.MoveTowards(player.moveVelocity.z, move.z, player.playerData.airAcceleration * Time.fixedDeltaTime);
            
            player.moveVelocity = player.moveVelocity.normalized * currentSpeed;
        }
        
        public void HandleDirection(PlayerStateMachine player)
        {
            Vector3 move = (player.moveInput.x * player.orientationPivot.right + player.moveInput.y * player.orientationPivot.forward).normalized;
            float speed = Mathf.Max(player.playerData.airMaxSpeed, player.ComputeGroundMoveVelocity().magnitude);
            move *= speed;
            
            if (player.moveInput.magnitude <= 0.05f)
            {
                player.moveVelocity.x = Mathf.MoveTowards(player.moveVelocity.x, 0.0f, player.playerData.airDeceleration * Time.fixedDeltaTime);
                player.moveVelocity.z = Mathf.MoveTowards(player.moveVelocity.z, 0.0f, player.playerData.airDeceleration * Time.fixedDeltaTime);
            }
            else
            {
                player.moveVelocity.x = Mathf.MoveTowards(player.moveVelocity.x, move.x, player.playerData.airAcceleration * Time.fixedDeltaTime);
                player.moveVelocity.z = Mathf.MoveTowards(player.moveVelocity.z, move.z, player.playerData.airAcceleration * Time.fixedDeltaTime);
            }
        }

        public void HandleGravity(PlayerStateMachine player)
        {
            if (player.playerRun.isOnSlope)
            {
                if (!player.playerRun.IsSlopeWalkable(player))
                    player.moveVelocity = player.playerRun.ComputeSlopeFallDirection() * player.playerData.steepSlopeFallSpeed;
                return;
            }

            if (isGrounded && player.moveVelocity.y <= 0f)
            {
                player.moveVelocity.y = player.playerData.groundingForce;
            }
            else
            {
                float inAirGravity = player.playerData.fallAcceleration;
                
                if (endedJumpEarly && player.moveVelocity.y > 0)
                    inAirGravity *= player.playerData.jumpEndEarlyGravityModifier;
                
                player.moveVelocity.y = Mathf.MoveTowards(player.moveVelocity.y, -player.playerData.fallMaxSpeed, inAirGravity * Time.fixedDeltaTime);
            }
        }

        public void StopBehaviour(PlayerStateMachine player, BehaviourType next)
        {
            endedJumpEarly = false;
        }

        public BehaviourType GetBehaviourType()
        {
            return BehaviourType.Jump;
        }
    }
}
