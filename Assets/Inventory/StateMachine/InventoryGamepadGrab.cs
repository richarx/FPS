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
        }

        private void SwapItems(InventoryStateMachine inventory)
        {
            (PocketItem first, PocketItem second) = inventory.player.backpackStorage.SwapItems(inventory.currentPocket, startingSlotIndex, inventory.inventoryCursor.currentSlotIndex);
            inventory.inventoryDisplay.SwapItems(inventory.currentPocket, startingSlotIndex, inventory.inventoryCursor.currentSlotIndex, first, second);
        }

        public void StopBehaviour(InventoryStateMachine inventory, InventoryBehaviourType next)
        {
            grabbedSlot.SetState(SlotDisplay.SlotState.Normal);
        }

        public InventoryBehaviourType GetBehaviourType()
        {
            return InventoryBehaviourType.GrabGamepad;
        }
    }
}
