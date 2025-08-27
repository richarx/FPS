using System.Collections.Generic;
using Tools_and_Scripts;
using UnityEngine;

namespace Inventory.StateMachine
{
    public class InventoryGamepadMove : IInventoryBehaviour
    {
        private float lastMoveTimestamp = -1.0f;
        private bool canMove => Time.time - lastMoveTimestamp >= 0.3f;
        
        public void StartBehaviour(InventoryStateMachine inventory, InventoryBehaviourType previous)
        {
        }

        public void UpdateBehaviour(InventoryStateMachine inventory)
        {
            if (inventory.player.inputPackage.lastInputType == InputType.Keyboard)
            {
                inventory.ChangeBehaviour(inventory.keyboardMove);
                return;
            }

            bool slotHasItem = inventory.GetCurrentDisplaySlot().HasItem;

            if (slotHasItem && inventory.player.inputPackage.southButton.wasPressedThisFrame)
            {
                inventory.ChangeToGrabBehaviour();
                return;
            }
            
            if (slotHasItem && inventory.player.inputPackage.northButton.wasPressedThisFrame)
            {
                inventory.ChangeToThrowBehaviour();
                return;
            }
            
            CheckPlayerMove(inventory);
        }

        public void CheckPlayerMove(InventoryStateMachine inventory)
        {
            Vector2 move = inventory.player.inputPackage.GetMove;

            if (canMove && move.magnitude > 0.15f)
            {
                MoveCursor(inventory, move);
                lastMoveTimestamp = Time.time;
            }
        }
        
        private void MoveCursor(InventoryStateMachine inventory, Vector2 move)
        {
            List<SlotDisplay> slots = inventory.inventoryDisplay.CurrentPocket.Slots;
            
            int nextSlot = ComputeNextSlot(move, inventory.inventoryCursor.currentSlotIndex, inventory.inventoryDisplay.CurrentPocket.Width, slots.Count);
            
            inventory.inventoryCursor.SetTargetPosition(slots[nextSlot].GetComponent<RectTransform>(), nextSlot);
        }

        private int ComputeNextSlot(Vector2 move, int currentIndex, int width, int slotCount)
        {
            move = move.normalized;
            
            if (move.x > 0.2f)
            {
                currentIndex += 1;

                if (currentIndex % width == 0.0f)
                    currentIndex -= width;
            }
            else if (move.x < -0.2f)
            {
                currentIndex -= 1;

                if (currentIndex < 0 || currentIndex % width == width - 1)
                    currentIndex += width;
            }
            else if (move.y > 0.2f)
            {
                currentIndex -= width;

                if (currentIndex < 0)
                    currentIndex += slotCount;
            }
            else if (move.y < -0.2f)
            {
                currentIndex += width;

                if (currentIndex >= slotCount)
                    currentIndex -= slotCount;
            }
            
            return currentIndex;
        }

        public void StopBehaviour(InventoryStateMachine inventory, InventoryBehaviourType next)
        {
        }

        public InventoryBehaviourType GetBehaviourType()
        {
            return InventoryBehaviourType.MoveGamepad;
        }
    }
}
