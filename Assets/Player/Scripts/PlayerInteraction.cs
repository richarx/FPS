using System;
using Items;
using Tools_and_Scripts;
using UnityEngine;

namespace Player.Scripts
{
    public class PlayerInteraction : MonoBehaviour
    {
        private InteractableItem registeredItem;

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
                case InteractableItem.ItemType.Loot:
                    break;
                case InteractableItem.ItemType.Weapon:
                    break;
                case InteractableItem.ItemType.Trigger:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public bool TryRegisterItem(InteractableItem item)
        {
            if (registeredItem != null)
                registeredItem.DeactivateItem();
            
            registeredItem = item;
            return true;
        }

        public void UnregisterItem(InteractableItem item)
        {
            if (registeredItem == item)
                registeredItem = null;
        }
    }
}
