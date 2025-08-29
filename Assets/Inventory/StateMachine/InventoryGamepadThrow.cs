using Items;
using Tools_and_Scripts;
using UnityEngine;

namespace Inventory.StateMachine
{
    public class InventoryGamepadThrow : IInventoryBehaviour
    {
        private int startingSlotIndex;
        private SlotDisplay grabbedSlot;

        private float timeToThrowAway = 1.05f;
        private float startThrowAwayTimestamp;
        
        public void StartBehaviour(InventoryStateMachine inventory, InventoryBehaviourType previous)
        {
            startingSlotIndex = inventory.inventoryCursor.currentSlotIndex;
            grabbedSlot = inventory.GetCurrentDisplaySlot();
            grabbedSlot.SetState(SlotDisplay.SlotState.Deleting);
            startThrowAwayTimestamp = Time.time;
        }

        public void UpdateBehaviour(InventoryStateMachine inventory)
        {
            if (inventory.player.inputPackage.lastInputType == InputType.Keyboard)
            {
                inventory.ChangeToMovementBehaviour();
                return;
            }

            if (!inventory.player.inputPackage.northButton.isPressed)
            {
                inventory.ChangeToMovementBehaviour();
                return;
            }

            if (Time.time - startThrowAwayTimestamp >= timeToThrowAway)
            {
                ThrowItemAway(inventory);
                inventory.ChangeToMovementBehaviour();
                return;
            }
        }

        private void ThrowItemAway(InventoryStateMachine inventory)
        {
            ItemData item = inventory.player.backpackStorage.GetItem(inventory.currentPocket, startingSlotIndex).item;
            inventory.ThrowItem(item);
            inventory.player.backpackStorage.RemoveItem(inventory.currentPocket, startingSlotIndex);
        }

        public void StopBehaviour(InventoryStateMachine inventory, InventoryBehaviourType next)
        {
            grabbedSlot.SetState(SlotDisplay.SlotState.Normal);
        }

        public InventoryBehaviourType GetBehaviourType()
        {
            return InventoryBehaviourType.ThrowAwayGamepad;
        }
    }
}
