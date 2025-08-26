using Inventory.StateMachine;
using Player.Scripts;
using Tools_and_Scripts;
using UnityEngine;

namespace Inventory
{
    public class InventoryInputsDisplay : MonoBehaviour
    {
        [SerializeField] private RectTransform inputDisplay;
        [SerializeField] private GameObject gamepadDisplay;
        [SerializeField] private GameObject keyboardDisplay;
        
        private PlayerStateMachine player;
        private InventoryStateMachine inventory;

        private Vector3 leftCornerPosition = new Vector3(-730.0f, -340.0f, 0.0f);
        private Vector3 rightCornerPosition = new Vector3(730.0f, -340.0f, 0.0f);
        
        private bool isRightCorner;

        private Vector3 targetPosition => isRightCorner ? rightCornerPosition : leftCornerPosition;
        
        private Vector3 velocity;
        
        private void Start()
        {
            player = PlayerStateMachine.instance;
            inventory = InventoryStateMachine.instance;
            
            inventory.openBackpack.OnOpenBackpack.AddListener(DisplayInputs);
            inventory.OnSwitchPocketTarget.AddListener(SwitchPocket);
            inventory.closeBackpack.OnCloseBackpack.AddListener(HideInputs);
            InputPacker.OnChangeInputType.AddListener(SetCorrectInputType);
            
            inputDisplay.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (!player.isBackpackOpen)
                return;

            inputDisplay.localPosition = Vector3.SmoothDamp(inputDisplay.localPosition, targetPosition, ref velocity, 0.15f);
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
            SetCorrectInputType(player.inputPackage.lastInputType);
        }

        private void HideInputs()
        {
            inputDisplay.gameObject.SetActive(false);
        }

        private void SetCorrectInputType(InputType lastInput)
        {
            gamepadDisplay.SetActive(lastInput == InputType.Gamepad);
            keyboardDisplay.SetActive(lastInput == InputType.Keyboard);
        }
    }
}
