using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace Tools_and_Scripts
{
    public enum InputType
    {
        Gamepad,
        Keyboard
    }

    public class InputData
    {
        public bool wasPressedThisFrame;
        public bool isPressed;
        public float lastPressTimestamp = -1.0f;

        public bool WasPressedWithBuffer()
        {
            if (Time.time - lastPressTimestamp <= 0.2f)
            {
                lastPressTimestamp = -1.0f;
                return true;
            }

            return false;
        }
    }

    public class InputPackage
    {
        public InputType lastInputType;

        //Gamepad
        public InputData eastButton;
        public InputData northButton;
        public InputData westButton;
        public InputData southButton;
        
        public InputData leftArrowButton;
        public InputData upArrowButton;
        public InputData rightArrowButton;
        public InputData downArrowButton;

        public InputData leftStickButton;
        public InputData rightStickButton;
        
        public InputData leftShoulder;
        public InputData rightShoulder;
        
        public InputData leftTrigger;
        public InputData rightTrigger;

        public InputData startButton;
        public InputData selectButton;
        
        //Keyboard
        public InputData leftMouse;
        public InputData rightMouse;
        public InputData middleMouse;

        public InputData leftKey;
        public InputData upKey;
        public InputData rightKey;        
        public InputData downKey;        
        
        public InputData shiftKey;
        
        public InputData bKey;
        public InputData eKey;
        public InputData fKey;
        public InputData gKey;
        public InputData iKey;
        public InputData xKey;
    }
    
    public class InputPacker
    {
        private InputPackage previousPackage;

        private bool wasGamepadUsed;
        private bool wasKeyboardUsed;
        
        public InputPackage ComputeInputPackage()
        {
            wasGamepadUsed = false;
            wasKeyboardUsed = false;

            InputPackage inputs = new InputPackage();
            
            inputs = ComputeGamepadInput(inputs);
            inputs = ComputeKeyboardInput(inputs);

            inputs.lastInputType = ComputeLastInputTypeUsed();

            previousPackage = inputs;

            return inputs;
        }

        private InputType ComputeLastInputTypeUsed()
        {
            if (wasGamepadUsed)
                return InputType.Gamepad;
            if (wasKeyboardUsed)
                return InputType.Keyboard;
            
            return previousPackage.lastInputType;
        }

        private InputPackage ComputeKeyboardInput(InputPackage inputs)
        {
            inputs.leftMouse = ComputeKeyboardInput(Mouse.current.leftButton, previousPackage.leftMouse.lastPressTimestamp);
            inputs.rightMouse = ComputeKeyboardInput(Mouse.current.rightButton, previousPackage.rightMouse.lastPressTimestamp);
            inputs.middleMouse = ComputeMiddleMouse(previousPackage.middleMouse.lastPressTimestamp);
            
            inputs.leftKey = ComputeDualKeyboardInput(Keyboard.current.aKey, Keyboard.current.qKey, previousPackage.leftKey.lastPressTimestamp);
            inputs.upKey = ComputeDualKeyboardInput(Keyboard.current.zKey, Keyboard.current.wKey, previousPackage.upKey.lastPressTimestamp);
            inputs.rightKey = ComputeKeyboardInput(Keyboard.current.dKey, previousPackage.rightKey.lastPressTimestamp);
            inputs.downKey = ComputeKeyboardInput(Keyboard.current.sKey, previousPackage.downKey.lastPressTimestamp);
            
            inputs.shiftKey = ComputeKeyboardInput(Keyboard.current.leftShiftKey, previousPackage.shiftKey.lastPressTimestamp);
            
            inputs.bKey = ComputeKeyboardInput(Keyboard.current.bKey, previousPackage.bKey.lastPressTimestamp);
            inputs.eKey = ComputeKeyboardInput(Keyboard.current.eKey, previousPackage.eKey.lastPressTimestamp);
            inputs.fKey = ComputeKeyboardInput(Keyboard.current.fKey, previousPackage.fKey.lastPressTimestamp);
            inputs.gKey = ComputeKeyboardInput(Keyboard.current.gKey, previousPackage.gKey.lastPressTimestamp);
            inputs.iKey = ComputeKeyboardInput(Keyboard.current.iKey, previousPackage.iKey.lastPressTimestamp);
            inputs.xKey = ComputeKeyboardInput(Keyboard.current.xKey, previousPackage.xKey.lastPressTimestamp);
            
            return inputs;
        }

        private InputPackage ComputeGamepadInput(InputPackage inputs)
        {
            inputs.eastButton = ComputeGamepadButtonInput(Gamepad.current.buttonEast, previousPackage.eastButton.lastPressTimestamp);
            inputs.northButton = ComputeGamepadButtonInput(Gamepad.current.buttonNorth, previousPackage.northButton.lastPressTimestamp);
            inputs.westButton = ComputeGamepadButtonInput(Gamepad.current.buttonWest, previousPackage.westButton.lastPressTimestamp);
            inputs.southButton = ComputeGamepadButtonInput(Gamepad.current.buttonSouth, previousPackage.southButton.lastPressTimestamp);

            inputs.leftArrowButton = ComputeGamepadButtonInput(Gamepad.current.dpad.left, previousPackage.leftArrowButton.lastPressTimestamp);
            inputs.upArrowButton = ComputeGamepadButtonInput(Gamepad.current.dpad.up, previousPackage.upArrowButton.lastPressTimestamp);
            inputs.rightArrowButton = ComputeGamepadButtonInput(Gamepad.current.dpad.right, previousPackage.rightArrowButton.lastPressTimestamp);
            inputs.downArrowButton = ComputeGamepadButtonInput(Gamepad.current.dpad.down, previousPackage.downArrowButton.lastPressTimestamp);
            
            inputs.leftStickButton = ComputeGamepadButtonInput(Gamepad.current.leftStickButton, previousPackage.leftStickButton.lastPressTimestamp);
            inputs.rightStickButton = ComputeGamepadButtonInput(Gamepad.current.rightStickButton, previousPackage.rightStickButton.lastPressTimestamp);
            
            inputs.leftShoulder = ComputeGamepadButtonInput(Gamepad.current.leftShoulder, previousPackage.leftShoulder.lastPressTimestamp);
            inputs.rightShoulder = ComputeGamepadButtonInput(Gamepad.current.rightShoulder, previousPackage.rightShoulder.lastPressTimestamp);
            
            inputs.leftTrigger = ComputeGamepadButtonInput(Gamepad.current.leftTrigger, previousPackage.leftTrigger.lastPressTimestamp);
            inputs.rightTrigger = ComputeGamepadButtonInput(Gamepad.current.rightTrigger, previousPackage.rightTrigger.lastPressTimestamp);
            
            inputs.startButton = ComputeGamepadButtonInput(Gamepad.current.startButton, previousPackage.startButton.lastPressTimestamp);
            inputs.selectButton = ComputeGamepadButtonInput(Gamepad.current.selectButton, previousPackage.selectButton.lastPressTimestamp);
            
            return inputs;
        }

        private InputData ComputeGamepadButtonInput(ButtonControl button, float lastPressTimestamp)
        {
            return ComputeButtonInput(button, lastPressTimestamp, InputType.Gamepad);
        }
        
        private InputData ComputeKeyboardInput(ButtonControl button, float lastPressTimestamp)
        {
            return ComputeButtonInput(button, lastPressTimestamp, InputType.Keyboard);
        }
        
        private InputData ComputeDualKeyboardInput(ButtonControl button_1, ButtonControl button_2, float lastPressTimestamp)
        {
            InputData input_1 = ComputeButtonInput(button_1, lastPressTimestamp, InputType.Keyboard);
            InputData input_2 = ComputeButtonInput(button_2, lastPressTimestamp, InputType.Keyboard);

            InputData input = new InputData();

            input.wasPressedThisFrame = input_1.wasPressedThisFrame || input_2.wasPressedThisFrame;
            input.isPressed = input_1.isPressed || input_2.isPressed;
            input.lastPressTimestamp = Mathf.Max(input_1.lastPressTimestamp, input_2.lastPressTimestamp);

            return input;
        }

        private InputData ComputeButtonInput(ButtonControl button, float lastPressTimestamp, InputType inputType)
        {
            InputData input = new InputData();
            
            input.wasPressedThisFrame = button.wasPressedThisFrame;
            input.isPressed = button.isPressed;

            input.lastPressTimestamp = input.wasPressedThisFrame ? Time.time : lastPressTimestamp;

            if (input.wasPressedThisFrame)
                RegisterInputType(inputType);
            
            return input;
        }

        private InputData ComputeMiddleMouse(float lastPressTimestamp)
        {
            bool isPressed = Mouse.current.scroll.up.magnitude > 0.0f || Mouse.current.scroll.down.magnitude > 0.0f;
            
            InputData input = new InputData();
            
            input.wasPressedThisFrame = isPressed;
            input.isPressed = isPressed;

            input.lastPressTimestamp = input.wasPressedThisFrame ? Time.time : lastPressTimestamp;
            
            if (input.wasPressedThisFrame)
                RegisterInputType(InputType.Keyboard);

            return input;
        }
        
        private void RegisterInputType(InputType inputType)
        {
            if (inputType == InputType.Gamepad)
                wasGamepadUsed = true;
            else if (inputType == InputType.Keyboard)
                wasKeyboardUsed = true;
        }
    }
}
