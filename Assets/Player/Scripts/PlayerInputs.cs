using Data;
using Pause_Menu;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tools_and_Scripts
{
    public static class PlayerInputs
    {
        public static Vector2 GetMoveDirection()
        {
            Vector2 gamepadInput = Vector2.zero;
            Vector2 keyBoardInput = Vector2.zero;
            
            if (Gamepad.current != null)
            {
                gamepadInput = new Vector2(Gamepad.current.leftStick.x.ReadValue(), Gamepad.current.leftStick.y.ReadValue());

                if (Mathf.Abs(gamepadInput.x) <= 0.15f)
                    gamepadInput.x = 0.0f;
        
                if (Mathf.Abs(gamepadInput.y) <= 0.15f)
                    gamepadInput.y = 0.0f;
                
                if (gamepadInput.magnitude > 0.15f)
                    return gamepadInput;
            }

            if (Keyboard.current.zKey.isPressed || Keyboard.current.wKey.isPressed)
                keyBoardInput.y += 1;
            
            if (Keyboard.current.sKey.isPressed)
                keyBoardInput.y -= 1;
            
            if (Keyboard.current.qKey.isPressed || Keyboard.current.aKey.isPressed)
                keyBoardInput.x -= 1;
            
            if (Keyboard.current.dKey.isPressed)
                keyBoardInput.x += 1;

            return keyBoardInput.normalized;
        }
    
        public static Vector2 GetAimingDirection()
        {
            Vector2 gamepadInput = Vector2.zero;
            Vector2 keyBoardInput = Vector2.zero;

            if (Gamepad.current != null)
            {
                gamepadInput = new Vector2(Gamepad.current.rightStick.x.ReadValue(), Gamepad.current.rightStick.y.ReadValue());
                
                if (Mathf.Abs(gamepadInput.x) <= 0.15f)
                    gamepadInput.x = 0.0f;
        
                if (Mathf.Abs(gamepadInput.y) <= 0.15f)
                    gamepadInput.y = 0.0f;
            }
            
            keyBoardInput.x = Input.GetAxisRaw("Mouse X") * Time.deltaTime;
            keyBoardInput.y = Input.GetAxisRaw("Mouse Y") * Time.deltaTime;

            return (gamepadInput + keyBoardInput).normalized;
        }
        
        public static Vector2 GetAimingDirectionWithSensibility()
        {
            Vector2 gamepad = Vector2.zero;
            Vector2 mouse = Vector2.zero;
            float sensibilityMultiplier = Application.isEditor ? 5.0f : 1.0f;

            if (Gamepad.current != null)
            {
                gamepad = new Vector2(Gamepad.current.rightStick.x.ReadValue(), Gamepad.current.rightStick.y.ReadValue());
                
                if (Mathf.Abs(gamepad.x) <= 0.15f)
                    gamepad.x = 0.0f;
            
                if (Mathf.Abs(gamepad.y) <= 0.15f)
                    gamepad.y = 0.0f;
            
                if (gamepad.magnitude > 0.15f)
                {
                    gamepad.x *= PauseMenu.instance.joystickXSensitivity * sensibilityMultiplier * Time.deltaTime;
                    gamepad.y *= PauseMenu.instance.joystickYSensitivity * sensibilityMultiplier * Time.deltaTime;
                    return gamepad;
                }
            }
            
            mouse.x = Input.GetAxisRaw("Mouse X");
            mouse.y = Input.GetAxisRaw("Mouse Y");

            if (mouse.magnitude > 0.0f)
                mouse *= PauseMenu.instance.mouseSensitivity * sensibilityMultiplier * Time.deltaTime;

            return mouse;
        }

        public static bool GetNorthButton(bool isHeld = false, bool withBuffer = true)
        {
            if (Gamepad.current == null)
                return false;
        
            if (isHeld)
                return Gamepad.current.buttonNorth.isPressed;

            if (withBuffer && Time.time <= throwBufferTimeStamp)
            {
                throwBufferTimeStamp = -1.0f;
                return true;
            }

            return Gamepad.current.buttonNorth.wasPressedThisFrame;
        }
    
        public static bool GetEastButton(bool withBuffer = true)
        {
            if (Gamepad.current == null)
                return false;

            if (withBuffer && Time.time <= dashBufferTimeStamp)
            {
                dashBufferTimeStamp = -1.0f;
                return true;
            }
            
            return Gamepad.current.buttonEast.wasPressedThisFrame;
        }
    
        public static bool GetWestButton(bool withBuffer = true)
        {
            bool gamepad = false;
            bool mouse = false;

            if (withBuffer && Time.time <= attackBufferTimeStamp)
            {
                attackBufferTimeStamp = -1.0f;
                return true;
            }
            
            if (Gamepad.current != null)
                gamepad = Gamepad.current.buttonWest.wasPressedThisFrame;

            mouse = Keyboard.current.rKey.wasPressedThisFrame;
        
            return gamepad || mouse;
        }

        public static bool GetSouthButton(bool isHeld = false, bool withBuffer = true)
        {
            bool gamepad = false;
            bool mouse = false;

            if (withBuffer && Time.time <= jumpBufferTimeStamp)
            {
                jumpBufferTimeStamp = -1.0f;
                return true;
            }
            
            if (Gamepad.current != null)
                gamepad = isHeld ? Gamepad.current.buttonSouth.isPressed || Gamepad.current.leftShoulder.isPressed : Gamepad.current.buttonSouth.wasPressedThisFrame || Gamepad.current.leftShoulder.wasPressedThisFrame;

            mouse = isHeld ? Keyboard.current.spaceKey.isPressed : Keyboard.current.spaceKey.wasPressedThisFrame;

            return gamepad || mouse;
        }

        public static bool GetWestButtonIsPressed()
        {
            if (Gamepad.current == null)
                return false;
        
            return Gamepad.current.buttonWest.isPressed;
        }
    
        public static bool GetWestButtonUp()
        {
            if (Gamepad.current == null)
                return false;
        
            return Gamepad.current.buttonWest.wasReleasedThisFrame;
        }
    
        public static bool GetLeftShoulder(bool isHeld = false, bool withBuffer = true)
        {
            if (Gamepad.current == null)
                return false;
        
            if (isHeld)
                return Gamepad.current.leftShoulder.isPressed;
        
            if (withBuffer && Time.time <= guardBufferTimeStamp)
            {
                guardBufferTimeStamp = -1.0f;
                return true;
            }
        
            return Gamepad.current.leftShoulder.wasPressedThisFrame;
        }
    
        public static bool GetRightShoulder(bool isHeld = false)
        {
            if (Gamepad.current == null)
                return false;
        
            if (isHeld)
                return Gamepad.current.rightShoulder.isPressed;
        
            return Gamepad.current.rightShoulder.wasPressedThisFrame;
        }

        public static bool GetLeftTrigger(bool isHeld = false)
        {
            bool gamepad = false;
            bool mouse = false;

            if (Gamepad.current != null)
                gamepad = isHeld ? Gamepad.current.leftTrigger.isPressed : Gamepad.current.leftTrigger.wasPressedThisFrame;

            mouse = isHeld ? Mouse.current.rightButton.isPressed : Mouse.current.rightButton.wasPressedThisFrame;

            return gamepad || mouse;
        }
    
        public static bool GetRightTrigger(bool isHeld = false, bool withBuffer = true)
        {
            bool gamepad = false;
            bool mouse = false;

            if (withBuffer && Time.time <= shootBufferTimeStamp)
            {
                shootBufferTimeStamp = -1.0f;
                return true;
            }
            
            if (Gamepad.current != null)
                gamepad = isHeld ? Gamepad.current.rightTrigger.isPressed : Gamepad.current.rightTrigger.wasPressedThisFrame;

            mouse = isHeld ? Mouse.current.leftButton.isPressed : Mouse.current.leftButton.wasPressedThisFrame;

            return gamepad || mouse;
        }

        public static bool GetSelectButton()
        {
            if (Gamepad.current == null)
                return false;
        
            return Gamepad.current.selectButton.wasPressedThisFrame;
        }
        
        public static bool GetStartButton()
        {
            bool gamepad = false;
            bool keyBoard = Keyboard.current.tabKey.wasPressedThisFrame || Keyboard.current.escapeKey.wasPressedThisFrame;

            if (Gamepad.current != null)
                gamepad = Gamepad.current.selectButton.wasPressedThisFrame;
        
            return gamepad || keyBoard;
        }

        private static float dashBufferTimeStamp = -1.0f;
        private static float attackBufferTimeStamp = -1.0f;
        private static float jumpBufferTimeStamp = -1.0f;
        private static float guardBufferTimeStamp = -1.0f;
        private static float throwBufferTimeStamp = -1.0f;
        private static float shootBufferTimeStamp = -1.0f;
    
        public static void UpdateInputBuffers()
        {
            if (GetEastButton(false))
                dashBufferTimeStamp = Time.time + 0.2f;

            if (GetWestButton(false))
                attackBufferTimeStamp = Time.time + 0.2f;
        
            if (GetSouthButton(false, false))
                jumpBufferTimeStamp = Time.time + 0.2f;
        
            if (GetLeftShoulder(true, false))
                guardBufferTimeStamp = Time.time + 0.2f;
        
            if (GetNorthButton(false, false))
                throwBufferTimeStamp = Time.time + 0.2f;
            
            if (GetRightTrigger(false, false))
                shootBufferTimeStamp = Time.time + 0.2f;
        }
    }
}
