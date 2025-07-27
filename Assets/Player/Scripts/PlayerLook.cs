using Pause_Menu;
using Tools_and_Scripts;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Scripts
{
    public class PlayerLook : MonoBehaviour
    {
        public Transform orientation;

        private PlayerStateMachine player;
        
        [HideInInspector] public float xRotation;
        [HideInInspector] public float yRotation;

        private Vector3 lookDirectionBeforeFollow;
        private Vector3 followTargetVelocity;
        private bool isLookDirectionReturningToBeforeFollow;
        
        private void Start()
        {
            player = PlayerStateMachine.instance;
            
            player.playerLocked.OnLockPlayer.AddListener(() => lookDirectionBeforeFollow = transform.forward);
            player.playerLocked.OnUnlockPlayer.AddListener(() => isLookDirectionReturningToBeforeFollow = true);
            
            Mouse.current.WarpCursorPosition(new Vector2(Screen.width / 2, Screen.height / 2));
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void LateUpdate()
        {
            //if (isLookDirectionReturningToBeforeFollow)
              //  GoBackToPreviousPosition();
            //else
            if (player.isLocked && player.playerLocked.hasTarget)
                FollowTarget();
            else
                FollowMouse();
        }

        private void GoBackToPreviousPosition()
        {
            Vector3 position = transform.position;
            Vector3 currentPosition = position + transform.forward * 10.0f;
            Vector3 targetPosition = position + lookDirectionBeforeFollow * 10.0f;
            currentPosition = Vector3.SmoothDamp(currentPosition, targetPosition, ref followTargetVelocity, 0.2f);
            
            Vector3 targetFlatPosition = currentPosition;
            targetFlatPosition.y = orientation.position.y;
            
            transform.LookAt(currentPosition, Vector3.up);
            orientation.LookAt(targetFlatPosition, Vector3.up);

            float distance = Vector3.Distance(currentPosition, targetPosition);
            if (distance <= 0.1f)
                isLookDirectionReturningToBeforeFollow = false;
        }

        private void FollowTarget()
        {
            Vector3 finalTargetPosition = player.playerLocked.targetPosition;
            
            Vector3 currentPosition = transform.position;
            float targetDistance = Vector3.Distance(currentPosition, finalTargetPosition);

            Vector3 currentTarget = currentPosition + transform.forward * targetDistance;
            currentTarget = Vector3.SmoothDamp(currentTarget, finalTargetPosition, ref followTargetVelocity, 0.5f);
            
            Vector2 lookDirection = PlayerInputs.GetAimingDirectionWithSensibility();
            lookDirection *= Time.deltaTime;

            if (player.isAiming)
                lookDirection *= PauseMenu.instance.aimSensitivityMultiplier;

            lookDirection = lookDirection.normalized * player.playerData.maxCameraMoveDistanceDuringDialog;
            
            currentTarget += lookDirection.ToVector3();
            
            Vector3 targetFlatPosition = currentTarget;
            targetFlatPosition.y = orientation.position.y;
            
            transform.LookAt(currentTarget, Vector3.up);
            orientation.LookAt(targetFlatPosition, Vector3.up);

            xRotation = transform.rotation.eulerAngles.x - 360.0f;
            yRotation = transform.rotation.eulerAngles.y;
        }

        private void FollowMouse()
        {
            Vector2 lookDirection = PlayerInputs.GetAimingDirectionWithSensibility();
            lookDirection *= Time.deltaTime;

            if (player.isAiming)
                lookDirection *= PauseMenu.instance.aimSensitivityMultiplier;

            yRotation += lookDirection.x;
            xRotation -= lookDirection.y;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        }

        public void ApplyKickBack(float x, float y)
        {
            xRotation -= y;
            yRotation += x;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        }
    }
}
