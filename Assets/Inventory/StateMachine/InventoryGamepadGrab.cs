using Tools_and_Scripts;

namespace Inventory.StateMachine
{
    public class InventoryGamepadGrab : IInventoryBehaviour
    {
        private int startingSlotIndex;
        private SlotDisplay grabbedSlot;

        private bool hasSkippedAFrame;
        
        public void StartBehaviour(InventoryStateMachine inventory, InventoryBehaviourType previous)
        {
            startingSlotIndex = inventory.inventoryCursor.currentSlotIndex;
            grabbedSlot = inventory.GetCurrentDisplaySlot();
            grabbedSlot.SetState(SlotDisplay.SlotState.Grabbed);
            
            inventory.itemPickedUpRect.localPosition = inventory.inventoryCursor.Cursor.localPosition;
            inventory.itemPickedUp.gameObject.SetActive(true);
            inventory.itemPickedUp.sprite = inventory.GetCurrentStorageSlot().item.icon;

            hasSkippedAFrame = false;
        }

        public void UpdateBehaviour(InventoryStateMachine inventory)
        {
            if (inventory.player.inputPackage.lastInputType == InputType.Keyboard)
            {
                inventory.ChangeBehaviour(inventory.keyboardMove);
                return;
            }

            if (hasSkippedAFrame && inventory.player.inputPackage.eastButton.wasPressedThisFrame)
            {
                inventory.ChangeToMovementBehaviour();
                return;
            }

            if (hasSkippedAFrame && inventory.player.inputPackage.southButton.wasPressedThisFrame)
            {
                if (startingSlotIndex != inventory.inventoryCursor.currentSlotIndex)
                    SwapItems(inventory);
                
                inventory.ChangeToMovementBehaviour();
                return;
            }

            hasSkippedAFrame = true;
            
            inventory.gamepadMove.CheckPlayerMove(inventory);
            inventory.itemPickedUpRect.localPosition = inventory.inventoryCursor.Cursor.localPosition;
        }

        private void SwapItems(InventoryStateMachine inventory)
        {
            inventory.player.backpackStorage.SwapItems(inventory.currentPocket, startingSlotIndex, inventory.inventoryCursor.currentSlotIndex);
        }

        public void StopBehaviour(InventoryStateMachine inventory, InventoryBehaviourType next)
        {
            grabbedSlot.SetState(SlotDisplay.SlotState.Normal);
            inventory.itemPickedUp.gameObject.SetActive(false);
        }

        public InventoryBehaviourType GetBehaviourType()
        {
            return InventoryBehaviourType.GrabGamepad;
        }
    }
}
