using UI;
using UI.ToolTip;
using UnityEngine;
using UnityEngine.Events;

namespace Items
{
    public class InteractableItem : Interactable
    {
        [SerializeField] private SpriteRenderer sr;
        [SerializeField] private Sprite sprite;
        [SerializeField] private Sprite outlineSprite;

        [HideInInspector] public UnityEvent OnLoot = new UnityEvent();

        private Rigidbody rb;

        private GameObject tooltip;
        
        private void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        public override void Interact()
        {
            OnLoot?.Invoke();
            Destroy(gameObject);
            
            if (tooltip != null)
                Destroy(tooltip);
        }
        
        protected override bool CanInteract()
        {
            return rb.velocity.magnitude <= 1.5f;
        }
        
        protected override void SetItemDisplay(bool isInteractable)
        {
            sr.sprite = isInteractable ? outlineSprite : sprite;

            if (isInteractable)
                tooltip = ToolTipManager.instance.DisplayToolTip("Press $E$ to Equip");
            else if (tooltip != null)
                Destroy(tooltip);
        }
    }
}
