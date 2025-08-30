using System;
using System.Collections;
using System.Collections.Generic;
using Inventory.StateMachine;
using Player.Scripts;
using TMPro;
using Tools_and_Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{
    public class InventoryInputsDisplay : MonoBehaviour
    {
        public enum InventoryInputState
        {
            Hidden,
            GamepadMove,
            KeyboardMove,
            GamepadGrab,
            KeyboardGrab,
            GamepadToolBelt,
            KeyboardToolBelt
        }
        
        [SerializeField] private RectTransform inputDisplay;
        
        [Space]
        [SerializeField] private Image background;
        [SerializeField] private List<Image> icons;
        [SerializeField] private List<TextMeshProUGUI> texts;
        
        [Space]
        [SerializeField] private Sprite gamepadPocket;
        [SerializeField] private Sprite gamepadGrab;
        [SerializeField] private Sprite gamepadEquip;
        [SerializeField] private Sprite gamepadThrowAway;
        [SerializeField] private Sprite gamepadCancel;
        
        [Space]
        [SerializeField] private Sprite keyboardPocket;
        [SerializeField] private Sprite keyboardGrab;
        [SerializeField] private Sprite keyboardThrowAway;
        [SerializeField] private Sprite keyboardCancel;

        [Space] 
        [SerializeField] private float fadeDuration;

        private PlayerStateMachine player;
        private InventoryStateMachine inventory;

        private Vector3 leftCornerPosition = new Vector3(-730.0f, -340.0f, 0.0f);
        private Vector3 rightCornerPosition = new Vector3(730.0f, -340.0f, 0.0f);

        private InventoryInputState currentState = InventoryInputState.Hidden;
        private bool isRightCorner;

        private Vector3 targetPosition => isRightCorner ? rightCornerPosition : leftCornerPosition;
        
        private Vector3 velocity;
        
        private void Start()
        {
            player = PlayerStateMachine.instance;
            inventory = InventoryStateMachine.instance;
            
            inventory.openBackpack.OnOpenBackpack.AddListener(DisplayInputs);
            inventory.OnSwitchPocketTarget.AddListener(SwitchPocket);
            inventory.closeInventory.OnCloseInventory.AddListener(HideInputs);
            //InputPacker.OnChangeInputType.AddListener(SetCorrectInputType);
            
            inputDisplay.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (!player.isBackpackOpen)
                return;

            InventoryInputState state = ComputeState();

            if (state != currentState)
                SwitchState(state);

            currentState = state;
            
            inputDisplay.localPosition = Vector3.SmoothDamp(inputDisplay.localPosition, targetPosition, ref velocity, 0.15f);
        }

        private void SwitchState(InventoryInputState state)
        {
            StopAllCoroutines();
            StartCoroutine(SwitchStateCoroutine(state));
        }

        private IEnumerator SwitchStateCoroutine(InventoryInputState newState)
        {
            if (currentState != InventoryInputState.Hidden)
                yield return HideState(currentState);

            yield return DisplayState(newState);
        }

        private IEnumerator DisplayState(InventoryInputState state)
        {
            switch (state)
            {
                case InventoryInputState.Hidden:
                    break;
                case InventoryInputState.GamepadMove:
                    DisplayLine(0, gamepadPocket, "Swap Pocket");
                    DisplayLine(1, gamepadGrab, "Grab");
                    DisplayLine(2, gamepadEquip, "Equip");
                    DisplayLine(3, gamepadThrowAway, "Throw Away");
                    break;
                case InventoryInputState.KeyboardMove:
                    DisplayLine(0, keyboardPocket, "Swap Pocket");
                    DisplayLine(1, keyboardGrab, "Grab");
                    DisplayLine(2, keyboardThrowAway, "Throw Away");
                    break;
                case InventoryInputState.GamepadGrab:
                    DisplayLine(0, gamepadPocket, "Swap Pocket");
                    DisplayLine(1, gamepadGrab, "Drop");
                    DisplayLine(2, gamepadCancel, "Cancel");
                    break;
                case InventoryInputState.KeyboardGrab:
                    DisplayLine(0, keyboardPocket, "Swap Pocket");
                    DisplayLine(1, keyboardGrab, "Drop");
                    DisplayLine(2, keyboardCancel, "Cancel");
                    break;
                case InventoryInputState.GamepadToolBelt:
                    DisplayLine(0, gamepadPocket, "Swap Pocket");
                    DisplayLine(1, gamepadGrab, "Grab");
                    DisplayLine(2, gamepadThrowAway, "Unequip");
                    break;
                case InventoryInputState.KeyboardToolBelt:
                    DisplayLine(0, keyboardPocket, "Swap Pocket");
                    DisplayLine(1, keyboardGrab, "Grab");
                    DisplayLine(2, keyboardThrowAway, "Unequip");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }

            yield return new WaitForSeconds(fadeDuration);
        }
        
        private IEnumerator HideState(InventoryInputState state)
        {
            switch (state)
            {
                case InventoryInputState.Hidden:
                    break;
                case InventoryInputState.GamepadMove:
                    HideLine(0);
                    HideLine(1);
                    HideLine(2);
                    HideLine(3);
                    break;
                case InventoryInputState.KeyboardMove:
                    HideLine(0);
                    HideLine(1);
                    HideLine(2);
                    break;
                case InventoryInputState.GamepadGrab:
                    HideLine(0);
                    HideLine(1);
                    HideLine(2);
                    break;
                case InventoryInputState.KeyboardGrab:
                    HideLine(0);
                    HideLine(1);
                    HideLine(2);
                    break;
                case InventoryInputState.GamepadToolBelt:
                    HideLine(0);
                    HideLine(1);
                    HideLine(2);
                    break;
                case InventoryInputState.KeyboardToolBelt:
                    HideLine(0);
                    HideLine(1);
                    HideLine(2);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }

            yield return new WaitForSeconds(fadeDuration + 0.01f);
            yield return null;
        }

        private void DisplayLine(int index, Sprite icon, string text)
        {
            icons[index].sprite = icon;
            texts[index].text = text;

            StartCoroutine(Tools.Fade(icons[index], fadeDuration, true));
            StartCoroutine(Tools.Fade(texts[index], fadeDuration, true));
        }

        private void HideLine(int index)
        {
            StartCoroutine(Tools.Fade(icons[index], fadeDuration, false));
            StartCoroutine(Tools.Fade(texts[index], fadeDuration, false));
        }

        private InventoryInputState ComputeState()
        {
            bool isGamepad = player.inputPackage.lastInputType == InputType.Gamepad;

            InventoryBehaviourType currentBehaviour = inventory.currentBehaviour.GetBehaviourType();

            if (currentBehaviour == InventoryBehaviourType.GrabGamepad || currentBehaviour == InventoryBehaviourType.GrabKeyboard)
                return isGamepad ? InventoryInputState.GamepadGrab : InventoryInputState.KeyboardGrab;

            if (inventory.inventoryCursor.isToolBelt)
                return isGamepad ? InventoryInputState.GamepadToolBelt : InventoryInputState.KeyboardToolBelt;

            return isGamepad ? InventoryInputState.GamepadMove : InventoryInputState.KeyboardMove;
        }

        private void SwitchPocket(BackpackStorage.Pocket pocket)
        {
            if (isRightCorner && pocket == BackpackStorage.Pocket.ammo)
                isRightCorner = false;
            else if (!isRightCorner && pocket == BackpackStorage.Pocket.medicine)
                isRightCorner = true;
        }

        private void DisplayInputs()
        {
            inputDisplay.gameObject.SetActive(true);

            InventoryInputState state = ComputeState();
            SwitchState(state);
            
            StartCoroutine(Tools.Fade(background, fadeDuration, true, 0.15f));
            
            currentState = state;
        }

        private void HideInputs()
        {
            SwitchState(InventoryInputState.Hidden);
            StartCoroutine(Tools.Fade(background, fadeDuration, false, 0.15f));

            currentState = InventoryInputState.Hidden;
        }
    }
}
