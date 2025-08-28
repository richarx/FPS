using Tools_and_Scripts;
using UnityEngine;

namespace Inventory.StateMachine
{
    public class InventoryKeyboardGrabToolBelt : IInventoryBehaviour
    {
        private int startingSlotIndex;

        private SlotDisplay grabbedSlot;
        
        public void StartBehaviour(InventoryStateMachine inventory, InventoryBehaviourType previous)
        {
            if (!inventory.keyboardMove.IsPointerDisplayed)
                inventory.keyboardMove.DisplayPointer(inventory);

            startingSlotIndex = inventory.inventoryCursor.currentSlotIndex;
            grabbedSlot = inventory.toolBeltDisplay.GetToolBeltSlot(startingSlotIndex);
            grabbedSlot.SetState(SlotDisplay.SlotState.Grabbed);
            
            inventory.itemPickedUpRect.anchoredPosition = CameraScreenPosition.instance.GetMousePosition(inventory.canvas);
            inventory.itemPickedUp.gameObject.SetActive(true);
            inventory.itemPickedUp.sprite = inventory.player.backpackStorage.GetToolBeltSlot(startingSlotIndex).pocketItem.item.icon;
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
                if (inventory.inventoryCursor.isToolBelt && startingSlotIndex != inventory.inventoryCursor.currentSlotIndex)
                    inventory.SwapToolBeltSlots(startingSlotIndex);
                
                inventory.ChangeToMovementBehaviour();
                return;
            }

            Vector3 position = CameraScreenPosition.instance.GetMousePosition(inventory.canvas);
            inventory.pointer.anchoredPosition = position;
            inventory.itemPickedUpRect.anchoredPosition = position;
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
            return InventoryBehaviourType.EquipKeyboard;
        }
    }
}
