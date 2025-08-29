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
            
            inventory.itemPickedUpRect.anchoredPosition = CameraScreenPosition.instance.GetMousePosition(inventory.canvas);
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
                if (inventory.inventoryCursor.isToolBelt)
                    inventory.EquipItem(startingSlotIndex, inventory.inventoryCursor.currentSlotIndex);
                else if (CanSwapItems(inventory))
                    SwapItems(inventory);
                
                inventory.ChangeToMovementBehaviour();
                return;
            }

            Vector3 position = CameraScreenPosition.instance.GetMousePosition(inventory.canvas);
            inventory.pointer.anchoredPosition = position;
            inventory.itemPickedUpRect.anchoredPosition = position;
        }

        private bool CanSwapItems(InventoryStateMachine inventory)
        {
            return startingSlotIndex != inventory.inventoryCursor.currentSlotIndex;
        }
        
        private void SwapItems(InventoryStateMachine inventory)
        {
            inventory.player.backpackStorage.SwapItems(inventory.currentPocket, startingSlotIndex, inventory.inventoryCursor.currentSlotIndex);
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
