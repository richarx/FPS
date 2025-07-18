using System;
using Items;
using Tools_and_Scripts;
using UnityEngine;

namespace Player.Scripts
{
    public class PlayerInteraction : MonoBehaviour
    {
        private PlayerGun playerGun;
        
        private Interactable registeredItem;

        private void Start()
        {
            playerGun = GetComponent<PlayerGun>();
        }

        private void Update()
        {
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
