using UnityEngine;

namespace Items
{
    public class InteractableItem : Interactable
    {
        [SerializeField] private SpriteRenderer sr;
        [SerializeField] private Sprite sprite;
        [SerializeField] private Sprite outlineSprite;

        protected override void SetItemDisplay(bool isInteractable)
        {
            sr.sprite = isInteractable ? outlineSprite : sprite;
        }
    }
}
