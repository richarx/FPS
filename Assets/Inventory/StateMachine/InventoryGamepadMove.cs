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

            bool slotHasItem = !inventory.IsCurrentSlotEmpty();

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
            
            if (!inventory.inventoryCursor.isToolBelt && slotHasItem && inventory.player.inputPackage.westButton.wasPressedThisFrame)
            {
                inventory.ChangeBehaviour(inventory.gamepadEquip);
                return;
            }
            
            CheckPlayerMove(inventory);
        }

        public void CheckPlayerMove(InventoryStateMachine inventory)
        {
            Vector2 move = inventory.player.inputPackage.GetMove;
            Vector2 moveToolBelt = inventory.player.inputPackage.GetLook;
            bool isCurrentlyInToolBelt = inventory.inventoryCursor.isToolBelt;

            if (canMove && moveToolBelt.magnitude > 0.2f)
            {
                MoveCursorToToolBelt(inventory, moveToolBelt, isCurrentlyInToolBelt);
                lastMoveTimestamp = Time.time;
            }
            else if (canMove && move.magnitude > 0.15f)
            {
                MoveCursor(inventory, move, isCurrentlyInToolBelt);
                lastMoveTimestamp = Time.time;
            }
        }

        private void MoveCursorToToolBelt(InventoryStateMachine inventory, Vector2 move, bool isCurrentlyInToolBelt)
        {
            bool isToolBeltOnTheRight = inventory.toolBeltDisplay.IsRightCorner;
            bool isMovingRight = move.x > 0.2f;
            bool isMovingLeft = move.x < -0.2f;
            bool isMovingTowardToolBelt = (isMovingRight && isToolBeltOnTheRight) || (isMovingLeft && !isToolBeltOnTheRight);
            
            if (!isCurrentlyInToolBelt && isMovingTowardToolBelt)
            {
                inventory.inventoryCursor.SetTargetPosition(inventory.toolBeltDisplay.GetToolBeltSlot(0).GetComponent<RectTransform>(), 0, true);
            }
            else if (isCurrentlyInToolBelt && !isMovingTowardToolBelt)
            {
                inventory.inventoryCursor.SetTargetPosition(inventory.inventoryDisplay.CurrentPocket.Slots[0].GetComponent<RectTransform>(), 0, false);
            }
        }

        private void MoveCursor(InventoryStateMachine inventory, Vector2 move, bool isCurrentlyInToolBelt)
        {
            List<SlotDisplay> slots = inventory.inventoryDisplay.CurrentPocket.Slots;

            int currentSlot = inventory.inventoryCursor.currentSlotIndex;
            int pocketWidth = isCurrentlyInToolBelt ? 4 : inventory.inventoryDisplay.CurrentPocket.Width;
            int pocketSlotCount = isCurrentlyInToolBelt ? 4 : slots.Count;
            
            int nextSlot = ComputeNextSlot(move, currentSlot, pocketWidth, pocketSlotCount);

            RectTransform target = (isCurrentlyInToolBelt ? inventory.toolBeltDisplay.GetToolBeltSlot(nextSlot) : slots[nextSlot]).GetComponent<RectTransform>();
            
            inventory.inventoryCursor.SetTargetPosition(target, nextSlot, isCurrentlyInToolBelt);
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
