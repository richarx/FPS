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

        private Vector3 followTargetVelocity;
        private bool isLookDirectionReturningToBeforeFollow;
        
        private void Start()
        {
            player = PlayerStateMachine.instance;
            
            Mouse.current.WarpCursorPosition(new Vector2(Screen.width / 2, Screen.height / 2));
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void LateUpdate()
        {
            if (player.isLocked && player.playerLocked.hasTarget)
                FollowTarget();
            else if (player.currentBehaviour.GetBehaviourType() == BehaviourType.Backpack)
                LookAtBackpack();
            else
                FollowMouse();
        }

        private void LookAtBackpack()
        {
            Transform target = player.backpackDisplay.GetCurrentLookTarget();

            if (target == null)
            {
                xRotation += 100.0f * Time.deltaTime;
                xRotation = Mathf.Clamp(xRotation, -90.0f, 30.0f);

                transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
                orientation.rotation = Quaternion.Euler(0, yRotation, 0);
            }
            else
            {
                Vector3 finalTargetPosition = target.position;
                Vector3 currentPosition = transform.position;
                float targetDistance = Vector3.Distance(currentPosition, finalTargetPosition);

                Vector3 currentTarget = currentPosition + transform.forward * targetDistance;
                currentTarget = Vector3.SmoothDamp(currentTarget, finalTargetPosition, ref followTargetVelocity, 0.1f);

                Vector3 targetFlatPosition = currentTarget;
                targetFlatPosition.y = orientation.position.y;
            
                transform.LookAt(currentTarget, Vector3.up);
                orientation.LookAt(targetFlatPosition, Vector3.up);

                float x = transform.rotation.eulerAngles.x;
            
                xRotation = x >= 180.0f ? x - 360.0f : x;
                yRotation = transform.rotation.eulerAngles.y;
            }
        }

        private void FollowTarget()
        {
            Vector3 finalTargetPosition = player.playerLocked.targetPosition;
            Vector3 currentPosition = transform.position;
            float targetDistance = Vector3.Distance(currentPosition, finalTargetPosition);

            Vector3 currentTarget = currentPosition + transform.forward * targetDistance;
            currentTarget = Vector3.SmoothDamp(currentTarget, finalTargetPosition, ref followTargetVelocity, 0.5f);
            
            if (player.isAiming)
            {
                Vector2 lookDirection = player.inputPackage.GetLook;
                lookDirection *= Time.deltaTime;
                lookDirection *= PauseMenu.instance.aimSensitivityMultiplier;
                Vector3 delta = transform.right * lookDirection.x + Vector3.up * lookDirection.y;
                delta = delta.normalized * player.playerData.maxCameraMoveDistanceDuringDialog;
                currentTarget += delta;
            }

            Vector3 targetFlatPosition = currentTarget;
            targetFlatPosition.y = orientation.position.y;
            
            transform.LookAt(currentTarget, Vector3.up);
            orientation.LookAt(targetFlatPosition, Vector3.up);

            float x = transform.rotation.eulerAngles.x;
            
            xRotation = x >= 180.0f ? x - 360.0f : x;
            yRotation = transform.rotation.eulerAngles.y;
        }

        private void FollowMouse()
        {
            Vector2 lookDirection = player.inputPackage.GetLook;
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
