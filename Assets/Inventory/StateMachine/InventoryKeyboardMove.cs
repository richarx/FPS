using Tools_and_Scripts;
using UnityEngine;

namespace Inventory.StateMachine
{
    public class InventoryKeyboardMove : IInventoryBehaviour
    {
        private bool isPointerDisplayed;
        
        public InventoryKeyboardMove(InventoryStateMachine inventory)
        {
            SlotMouseDetection.OnSlotMouseOver.AddListener((rect, slotIndex) => MoveCursorToSlot(inventory, rect, slotIndex));
        }
        
        public void StartBehaviour(InventoryStateMachine inventory, InventoryBehaviourType previous)
        {
            DisplayPointer(inventory);
        }

        public void UpdateBehaviour(InventoryStateMachine inventory)
        {
            if (inventory.player.inputPackage.lastInputType == InputType.Gamepad)
            {
                inventory.ChangeBehaviour(inventory.gamepadMove);
                return;
            }
            
            inventory.pointer.anchoredPosition = CameraScreenPosition.instance.GetMousePosition(inventory.canvas);
        }
        
        private void MoveCursorToSlot(InventoryStateMachine inventory, RectTransform slot, int slotIndex)
        {
            if (!inventory.player.isBackpackOpen || inventory.player.inputPackage.lastInputType != InputType.Keyboard)
                return;
            
            inventory.inventoryCursor.SetTargetPosition(slot, slotIndex);
        }
        
        private void DisplayPointer(InventoryStateMachine inventory)
        {
            isPointerDisplayed = true;
            Cursor.lockState = CursorLockMode.Confined;
            inventory.pointer.gameObject.SetActive(true);
        }

        private void HidePointer(InventoryStateMachine inventory)
        {
            isPointerDisplayed = false;
            Cursor.lockState = CursorLockMode.Locked;
            inventory.pointer.gameObject.SetActive(false);
        }

        public void StopBehaviour(InventoryStateMachine inventory, InventoryBehaviourType next)
        {
            if (isPointerDisplayed)
                HidePointer(inventory);
        }

        public InventoryBehaviourType GetBehaviourType()
        {
            return InventoryBehaviourType.MoveKeyboard;
        }
    }
}
