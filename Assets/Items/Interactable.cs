using Items.Triggers;
using Player.Scripts;
using UI.ToolTip;
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

        [SerializeField] private string toolTipText;
        [SerializeField] private ItemType itemType;
        public ItemType type => itemType;
        
        private PlayerInteraction playerInteraction;
        
        protected bool isPlayerInRange;
        
        protected GameObject tooltip;

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
    
        protected virtual  void OnPlayerEnterRange()
        {
            if (playerInteraction == null)
                playerInteraction = PlayerStateMachine.instance.GetComponent<PlayerInteraction>();

            ActivateItem();
        }

        protected virtual void OnPlayerExitRange()
        {
            if (playerInteraction == null)
                playerInteraction = PlayerStateMachine.instance.GetComponent<PlayerInteraction>();
            
            playerInteraction.UnregisterItem(this);
            DeactivateItem();
        }

        public virtual void ActivateItem()
        {
            if (playerInteraction.TryRegisterItem(this))
            {
                isPlayerInRange = true;
                SetItemDisplay(isPlayerInRange);
            }
        }

        public virtual void DeactivateItem()
        {
            isPlayerInRange = false;
            SetItemDisplay(isPlayerInRange);
        }
        
        protected virtual void SetItemDisplay(bool isInteractable)
        {
            Debug.Log($"SetItemDisplay : {isInteractable}");
            if (isInteractable)
                CreateTooltip();
            else
                DestroyTooltip();
        }

        protected void CreateTooltip()
        {
            Debug.Log("Create Tooltip");
            if (tooltip == null && !string.IsNullOrEmpty(toolTipText))
                tooltip = ToolTipManager.instance.DisplayToolTip(toolTipText);
        }

        protected void DestroyTooltip()
        {
            if (tooltip != null)
                Destroy(tooltip);
            tooltip = null;
        }
    }
}
