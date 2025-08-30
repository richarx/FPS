using Items;
using Tools_and_Scripts;
using UnityEngine;

namespace Inventory.StateMachine
{
    public class InventoryKeyboardThrow : IInventoryBehaviour
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
            if (inventory.player.inputPackage.lastInputType == InputType.Gamepad)
            {
                inventory.ChangeToMovementBehaviour();
                return;
            }

            if (!inventory.player.inputPackage.fKey.isPressed)
            {
                inventory.ChangeToMovementBehaviour();
                return;
            }

            if (Time.time - startThrowAwayTimestamp >= timeToThrowAway)
            {
                ThrowItemAway(inventory, startingSlotIndex);
                inventory.ChangeToMovementBehaviour();
                return;
            }
            
            inventory.keyboardMove.MakePointerFollowCursor(inventory);
        }
        
        public void ThrowItemAway(InventoryStateMachine inventory, int toolBeltIndex)
        {
            bool isCursorOnToolBelt = inventory.inventoryCursor.isToolBelt;
            BackpackStorage.Pocket pocket = isCursorOnToolBelt ? BackpackStorage.Pocket.toolBelt : inventory.currentPocket;
            PocketItem item = inventory.player.backpackStorage.GetItem(pocket, toolBeltIndex);
            
            if (isCursorOnToolBelt)
                inventory.player.backpackStorage.StoreItem(item.item, item.count);
            else
                inventory.ThrowItem(item.item);
            inventory.player.backpackStorage.RemoveItem(pocket, toolBeltIndex);
        }

        public void StopBehaviour(InventoryStateMachine inventory, InventoryBehaviourType next)
        {
            grabbedSlot.SetState(SlotDisplay.SlotState.Normal);
        }

        public InventoryBehaviourType GetBehaviourType()
        {
            return InventoryBehaviourType.ThrowAwayKeyboard;
        }
    }
}
