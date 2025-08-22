using System;
using Backpack;
using Items;
using Pause_Menu;
using Tools_and_Scripts;
using UnityEngine;

namespace Player.Scripts
{
    public class PlayerInteraction : MonoBehaviour
    {
        private PlayerGun playerGun;
        private BackpackStorage backpackStorage;
        
        private Interactable registeredItem;

        private void Start()
        {
            playerGun = GetComponent<PlayerGun>();
            backpackStorage = GetComponent<BackpackStorage>();
        }

        private void Update()
        {
            if (PauseMenu.instance.IsPaused)
                return;
            
            if (PlayerInputs.GetRightShoulder() && registeredItem != null)
                InteractWithItem();
        }

        private void InteractWithItem()
        {
            if (registeredItem == null)
                return;
            
            Debug.Log($"Interact with item : {registeredItem.gameObject.name}, of type : {registeredItem.type}");

            switch (registeredItem.type)
            {
                case Interactable.ItemType.Loot:
                    LootItem();
                    break;
                case Interactable.ItemType.Weapon:
                    playerGun.EquipNewWeapon(registeredItem.GetComponent<LootWeapon>().GetWeaponData());
                    registeredItem.Interact();
                    break;
                case Interactable.ItemType.Trigger:
                    registeredItem.Interact();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void LootItem()
        {
            ItemData item = registeredItem.GetComponent<LootItem>().GetItemData();
            if (backpackStorage.CanStoreItem(item))
            {
                backpackStorage.StoreItem(item);
                registeredItem.Interact();
            }
        }

        public bool TryRegisterItem(Interactable item)
        {
            if (registeredItem != null)
                registeredItem.DeactivateItem();
            
            registeredItem = item;
            return true;
        }

        public void UnregisterItem(Interactable item)
        {
            if (registeredItem == item)
                registeredItem = null;
        }
    }
}
