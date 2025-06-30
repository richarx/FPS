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

        private void Start()
        {
            player = PlayerStateMachine.instance;
            Mouse.current.WarpCursorPosition(new Vector2(Screen.width / 2, Screen.height / 2));
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
        }

        private void LateUpdate()
        {
            Vector2 lookDirection = PlayerInputs.GetAimingDirectionWithSensibility(player.playerData);
            lookDirection *= Time.deltaTime;
        
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
