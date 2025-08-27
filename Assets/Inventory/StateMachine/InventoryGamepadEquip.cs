using System.Collections.Generic;
using Tools_and_Scripts;
using UnityEngine;

namespace Inventory.StateMachine
{
    public class InventoryGamepadEquip : IInventoryBehaviour
    {
        private int startingSlotIndex;
        private SlotDisplay grabbedSlot;

        private int currentToolBeltSlotIndex;
        
        private bool hasSkippedAFrame;
        
        private float lastMoveTimestamp = -1.0f;
        private bool canMove => Time.time - lastMoveTimestamp >= 0.3f;
        
        public void StartBehaviour(InventoryStateMachine inventory, InventoryBehaviourType previous)
        {
            startingSlotIndex = inventory.inventoryCursor.currentSlotIndex;
            grabbedSlot = inventory.GetCurrentDisplaySlot();
            grabbedSlot.SetState(SlotDisplay.SlotState.Grabbed);

            currentToolBeltSlotIndex = 0;
            
            inventory.inventoryCursor.SetTargetPosition(inventory.toolBeltDisplay.GetToolBeltSlot(currentToolBeltSlotIndex).GetComponent<RectTransform>(), startingSlotIndex);
            
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
                EquipItem(inventory);
                inventory.ChangeToMovementBehaviour();
                return;
            }

            CheckPlayerMove(inventory);

                hasSkippedAFrame = true;
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
            currentToolBeltSlotIndex = ComputeNextSlot(move, currentToolBeltSlotIndex);
            inventory.inventoryCursor.SetTargetPosition(inventory.toolBeltDisplay.GetToolBeltSlot(currentToolBeltSlotIndex).GetComponent<RectTransform>(), startingSlotIndex);
        }

        private int ComputeNextSlot(Vector2 move, int currentIndex)
        {
            if (move.x > 0.2f)
            {
                currentIndex += 1;

                if (currentIndex > 3)
                    currentIndex = 0;
            }
            else if (move.x < -0.2f)
            {
                currentIndex -= 1;

                if (currentIndex < 0)
                    currentIndex = 3;
            }
            else if (move.y > 0.2f)
            {
                
            }
            else if (move.y < -0.2f)
            {
               
            }
            
            return currentIndex;
        }

        private void EquipItem(InventoryStateMachine inventory)
        {
            Debug.Log($"Equip item : {startingSlotIndex} / {currentToolBeltSlotIndex}");
        }

        public void StopBehaviour(InventoryStateMachine inventory, InventoryBehaviourType next)
        {
            grabbedSlot.SetState(SlotDisplay.SlotState.Normal);
        }

        public InventoryBehaviourType GetBehaviourType()
        {
            return InventoryBehaviourType.EquipGamepad;
        }
    }
}
