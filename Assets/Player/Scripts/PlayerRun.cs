using Tools_and_Scripts;
using UnityEngine;
using UnityEngine.Events;

namespace Player.Scripts
{
    public class PlayerRun : IPlayerBehaviour
    {
        public UnityEvent OnStartSprinting = new UnityEvent();
        public UnityEvent OnStopSprinting = new UnityEvent();
        
        private RaycastHit slopeHit;
        public bool isOnSlope;
        public bool isSlopeWalkable;
        public bool isSprinting;

        private bool isCrouchInputReset;

        public PlayerRun(PlayerStateMachine player)
        {
            player.playerGun.OnChangeAimState.AddListener((isAiming) =>
            {
                if (isAiming)
                    CancelSprint();
            });
            player.playerGun.OnShoot.AddListener(CancelSprint);
            player.playerGun.OnStartReloading.AddListener(CancelSprint);
        }
        
        public void StartBehaviour(PlayerStateMachine player, BehaviourType previous)
        {
            Debug.Log("RUN");

            if (previous == BehaviourType.Crouch || previous == BehaviourType.Slide)
                isCrouchInputReset = false;
        }

        public void UpdateBehaviour(PlayerStateMachine player)
        {
            if (player.playerJump.CanJump() && PlayerInputs.GetSouthButton())
            {
                player.playerJump.StartJump(player);
                return;
            }

            if (isCrouchInputReset && PlayerInputs.GetEastButton(isHeld:true))
            {
                player.ChangeBehaviour(player.playerCrouch);
                return;
            }

            HandleSprint(player);

            isCrouchInputReset = isCrouchInputReset || !PlayerInputs.GetEastButton(isHeld:true);
        }

        private bool isJoystickSprint;
        public void HandleSprint(PlayerStateMachine player)
        {
            bool sprintInputJoystick = PlayerInputs.GetLeftStickClick();
            bool sprintInputKeyboard = PlayerInputs.GetLeftShift(true);

            bool sprintInput = (sprintInputJoystick || isJoystickSprint) || sprintInputKeyboard;

            if (isSprinting && isJoystickSprint && sprintInputJoystick)
                sprintInput = false;

            if (player.moveInput.y <= 0.5f)
            {
                sprintInput = false;
                isJoystickSprint = false;
            }

            if (isSprinting != sprintInput && !player.isAiming && !player.isReloading && !player.isShooting)
            {
                isSprinting = sprintInput;

                isJoystickSprint = isSprinting && sprintInputJoystick;
                
                if (isSprinting)
                    OnStartSprinting?.Invoke();
                else
                    OnStopSprinting?.Invoke();
            }
        }

        public void CancelSprint()
        {
            if (isSprinting)
            {
                isSprinting = false;
                isJoystickSprint = false;
                OnStopSprinting?.Invoke();
            }
        }

        public void FixedUpdateBehaviour(PlayerStateMachine player)
        {
            player.playerJump.CheckCollisions(player);
            
            HandleSlop(player);
            
            if (CanPlayerControlDirection())
                HandleDirection(player);
            
            UpdateMoveVelocityOnSlope(player);
            
            player.playerJump.HandleGravity(player);

            player.ApplyMovement();
            
            if (!player.playerJump.isGrounded)
                player.ChangeBehaviour(player.playerJump);
        }

        public bool CheckIsOnSlope(PlayerStateMachine player)
        {
            if (Physics.Raycast(player.position + (Vector3.up * 0.1f), Vector3.down, out slopeHit, 0.5f, ~player.playerData.layersToIgnoreForGroundCheck))
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

        public void HandleSlop(PlayerStateMachine player)
        {
            isSlopeWalkable = isOnSlope && IsSlopeWalkable(player);
        }

        public bool CanPlayerControlDirection()
        {
            return !isOnSlope || isSlopeWalkable;
        }

        public void HandleDirection(PlayerStateMachine player)
        {
            Vector3 move = (player.moveInput.x * player.orientationPivot.right + player.moveInput.y * player.orientationPivot.forward).normalized;
            float speed = ComputeMoveSpeed(player);
            move *= speed;
            
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
        }

        public void UpdateMoveVelocityOnSlope(PlayerStateMachine player)
        {
            if (isOnSlope && isSlopeWalkable)
            {
                float magnitude = player.moveVelocity.magnitude;
                player.moveVelocity = ComputeSlopeMoveDirection(player.moveVelocity) * magnitude;
            }
        }

        private float ComputeMoveSpeed(PlayerStateMachine player)
        {
            if (player.isAiming)
                return player.playerData.groundMaxSpeedAiming;

            return isSprinting ? player.playerData.sprintMaxSpeed : player.playerData.walkMaxSpeed;
        }

        public void StopBehaviour(PlayerStateMachine player, BehaviourType next)
        {
            slopeHit = new RaycastHit();
            isOnSlope = false;
            
            if (isSprinting)
                CancelSprint();
        }

        public BehaviourType GetBehaviourType()
        {
            return BehaviourType.Run;
        }
    }
}
