using System.Collections.Generic;
using Player.Scripts;
using Tools_and_Scripts;
using UnityEngine;

namespace Inventory
{
    public class GamepadInventoryCursor : MonoBehaviour
    {
        private PlayerStateMachine player;
        private InventoryCursor inventoryCursor;
        private InventoryDisplay inventoryDisplay;

        private float lastMoveTimestamp = -1.0f;
        private bool canMove => Time.time - lastMoveTimestamp >= 0.3f;
        
        private void Start()
        {
            inventoryCursor = GetComponent<InventoryCursor>();
            inventoryDisplay = GetComponent<InventoryDisplay>();
            player = PlayerStateMachine.instance;
            
            //player.playerBackpack.OnOpenBag.AddListener(DisplayPocket);
            //player.backpackDisplay.OnSwitchPocketTarget.AddListener(SwitchPocket);
            //player.playerBackpack.OnCloseBag.AddListener(HidePocket);
        }

        private void Update()
        {
            if (!player.isBackpackOpen || player.inputPackage.lastInputType != InputType.Gamepad)
                return;

            Vector2 move = player.inputPackage.GetMove;

            if (canMove && move.magnitude > 0.15f)
            {
                MoveCursor(move);
                lastMoveTimestamp = Time.time;
            }
        }

        private void MoveCursor(Vector2 move)
        {
            List<SlotDisplay> slots = inventoryDisplay.CurrentPocket.Slots;
            
            int nextSlot = ComputeNextSlot(move, inventoryCursor.currentSlotIndex, inventoryDisplay.CurrentPocket.Width, slots.Count);
            
            inventoryCursor.SetTargetPosition(slots[nextSlot].GetComponent<RectTransform>(), nextSlot);
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
    }
}
