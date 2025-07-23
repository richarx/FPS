using Items.Triggers;
using Player.Scripts;
using UnityEngine;

namespace Items
{
    public class Interactable : Trigger
    {
        public enum ItemType
        {
            Loot,
            Weapon,
            Trigger
        }
        
        [SerializeField] private ItemType itemType;
        public ItemType type => itemType;
        
        private PlayerInteraction playerInteraction;
        
        private bool isPlayerInRange;

        public virtual void Interact()
        {
            
        }

        protected virtual bool CanInteract()
        {
            return true;
        }
    
        private void OnTriggerStay(Collider other)
        {
            if (!isPlayerInRange && CanInteract() && other.attachedRigidbody != null && other.attachedRigidbody.CompareTag("Player"))
                OnPlayerEnterRange();
        }

        private void OnTriggerExit(Collider other)
        {
            if (isPlayerInRange && other.attachedRigidbody != null && other.attachedRigidbody.CompareTag("Player"))
                OnPlayerExitRange();
        }
    
        private void OnPlayerEnterRange()
        {
            if (playerInteraction == null)
                playerInteraction = PlayerStateMachine.instance.GetComponent<PlayerInteraction>();

            if (playerInteraction.TryRegisterItem(this))
            {
                isPlayerInRange = true;
                SetItemDisplay(isPlayerInRange);
            }
        }

        private void OnPlayerExitRange()
        {
            if (playerInteraction == null)
                playerInteraction = PlayerStateMachine.instance.GetComponent<PlayerInteraction>();
            
            playerInteraction.UnregisterItem(this);
            DeactivateItem();
        }

        public virtual void DeactivateItem()
        {
            isPlayerInRange = false;
            SetItemDisplay(isPlayerInRange);
        }
        
        protected virtual void SetItemDisplay(bool isInteractable)
        {
        }
    }
}
