using Tools_and_Scripts;
using UnityEngine;

namespace Inventory.StateMachine
{
    public class InventoryKeyboardGrab : IInventoryBehaviour
    {
        private int startingSlotIndex;
        private SlotDisplay grabbedSlot;

        public void StartBehaviour(InventoryStateMachine inventory, InventoryBehaviourType previous)
        {
            if (!inventory.keyboardMove.IsPointerDisplayed)
                inventory.keyboardMove.DisplayPointer(inventory);
            
            startingSlotIndex = inventory.inventoryCursor.currentSlotIndex;
            grabbedSlot = inventory.GetCurrentDisplaySlot();
            grabbedSlot.SetState(SlotDisplay.SlotState.Grabbed);
            
            inventory.itemPickedUp.gameObject.SetActive(true);
            inventory.itemPickedUp.sprite = inventory.GetCurrentStorageSlot().item.icon;
        }

        public void UpdateBehaviour(InventoryStateMachine inventory)
        {
            if (inventory.player.inputPackage.lastInputType == InputType.Gamepad)
            {
                inventory.ChangeBehaviour(inventory.gamepadMove);
                return;
            }
            
            if (!inventory.player.inputPackage.leftMouse.isPressed)
            {
                if (startingSlotIndex != inventory.inventoryCursor.currentSlotIndex)
                    SwapItems(inventory);
                
                inventory.ChangeToMovementBehaviour();
                return;
            }

            Vector3 position = CameraScreenPosition.instance.GetMousePosition(inventory.canvas);
            inventory.pointer.anchoredPosition = position;
            inventory.itemPickedUpRect.anchoredPosition = position;
        }
        
        private void SwapItems(InventoryStateMachine inventory)
        {
            (PocketItem first, PocketItem second) = inventory.player.backpackStorage.SwapItems(inventory.currentPocket, startingSlotIndex, inventory.inventoryCursor.currentSlotIndex);
            inventory.inventoryDisplay.SwapItems(inventory.currentPocket, startingSlotIndex, inventory.inventoryCursor.currentSlotIndex, first, second);
        }

        public void StopBehaviour(InventoryStateMachine inventory, InventoryBehaviourType next)
        {
            if (inventory.keyboardMove.IsPointerDisplayed)
                inventory.keyboardMove.HidePointer(inventory);
            
            grabbedSlot.SetState(SlotDisplay.SlotState.Normal);
            
            inventory.itemPickedUp.gameObject.SetActive(false);
        }

        public InventoryBehaviourType GetBehaviourType()
        {
            return InventoryBehaviourType.GrabKeyboard;
        }
    }
}
