using Player.Scripts;
using UnityEngine;

namespace Items
{
    public class InteractableItem : MonoBehaviour
    {
        public enum ItemType
        {
            Loot,
            Weapon,
            Trigger
        }
        
        [SerializeField] private SpriteRenderer sr;
        [SerializeField] private Sprite sprite;
        [SerializeField] private Sprite outlineSprite;
        [SerializeField] private ItemType itemType;
        public ItemType type => itemType;
        
        private PlayerInteraction playerInteraction;
        
        private bool isPlayerInRange;

        public virtual void Interact()
        {
            
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.attachedRigidbody.CompareTag("Player"))
                OnPlayerEnterRange();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.attachedRigidbody.CompareTag("Player"))
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

        public void DeactivateItem()
        {
            isPlayerInRange = false;
            SetItemDisplay(isPlayerInRange);
        }
        
        private void SetItemDisplay(bool isInteractable)
        {
            sr.sprite = isInteractable ? outlineSprite : sprite;
        }
    }
}
