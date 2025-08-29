using System;
using Inventory;
using Items;
using UnityEngine;
using UnityEngine.Events;

namespace Player.Scripts
{
    public class PlayerTools : MonoBehaviour
    {
        public enum ToolUsage
        {
            Scanner,
            FlashLight,
            Throw,
            Equip,
            None
        }

        [HideInInspector] public UnityEvent OnThrowItem = new UnityEvent();

        private int lastToolUsed = -1;
        
        private PlayerStateMachine player;
        private PlayerFlashlight flashlight;

        private Transform currentTool;
        private ItemData currentItemData;

        private bool isInputReset;
        private float lastThrowTimestamp;
        
        private void Start()
        {
            player = PlayerStateMachine.instance;
            flashlight = GetComponent<PlayerFlashlight>();
        }

        private void Update()
        {
            if (player.isLocked || player.isScanning || player.isBackpackOpen)
                return;

            CheckToolInput();
            
            if (CanThrow() && player.inputPackage.GetShoot.isPressed)
            {
                ThrowItem();
                isInputReset = false;
            }
            
            if (!isInputReset && !player.inputPackage.GetShoot.isPressed)
                isInputReset = true;
        }

        private void CheckToolInput()
        {
            int direction = -1;
            
            if (player.inputPackage.GetToolLeft.WasPressedWithBuffer())
                direction = 0;
            if (player.inputPackage.GetToolUp.WasPressedWithBuffer())
                direction = 1;
            if (player.inputPackage.GetToolRight.WasPressedWithBuffer())
                direction = 2;
            if (player.inputPackage.GetToolDown.WasPressedWithBuffer())
                direction = 3;

            if (direction >= 0)
            {
                EquipToolInSlot(direction);
            }
        }

        private void EquipToolInSlot(int index)
        {
            PocketItem pocketItem = player.backpackStorage.GetItem(BackpackStorage.Pocket.toolBelt, index);

            if (pocketItem.isEmpty)
                return;

            bool isSameToolAsLastTime = index == lastToolUsed;
            
            switch (pocketItem.item.toolUsage)
            {
                case ToolUsage.Scanner:
                    player.scanner.TriggerScanner();
                    break;
                case ToolUsage.FlashLight:
                    flashlight.ToggleFlashlight();
                    break;
                case ToolUsage.Throw:
                    if (isSameToolAsLastTime)
                        UnEquipTool();
                    else
                        EquipTool(pocketItem.item, index);
                    break;
                case ToolUsage.Equip:
                    if (isSameToolAsLastTime)
                        UnEquipTool();
                    else
                        EquipTool(pocketItem.item, index);
                    break;
                case ToolUsage.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ThrowItem()
        {
            Vector3 direction = player.playerShootGun.shootingDirection.normalized;
            Vector3 finalPosition = player.playerShootGun.shootingPosition + direction;
            GameObject item = Instantiate(currentItemData.thrownPrefab, finalPosition, Quaternion.identity);
            item.GetComponent<ThrowableItem>().Setup(direction);
            
            lastThrowTimestamp = Time.time;
            OnThrowItem?.Invoke();
        }

        private bool CanThrow()
        {
            if (player.playerArms.currentArmType != PlayerArms.ArmType.Throw)
                return false;

            if (lastThrowTimestamp > 0.0f && Time.time - lastThrowTimestamp <= 1.0f)
                return false;
            
            return isInputReset;
        }

        private void EquipTool(ItemData item, int index)
        {
            currentItemData = item;
            currentTool = player.playerArms.EquipThrowTool(item);
            lastToolUsed = index;
        }

        private void UnEquipTool()
        {
            currentItemData = null;
            player.playerArms.UnEquipThrowTool();
            lastToolUsed = -1;
        }
    }
}
