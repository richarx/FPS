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
        
        public override void Interact()
        {
            OnLoot?.Invoke();
        }
        
        protected override void SetItemDisplay(bool isInteractable)
        {
            sr.sprite = isInteractable ? outlineSprite : sprite;
        }
    }
}
