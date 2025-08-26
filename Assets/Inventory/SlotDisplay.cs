using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{
    public class SlotDisplay : MonoBehaviour
    {
        public enum SlotState
        {
            Normal,
            Grabbed,
            Deleting
        }
        
        [SerializeField] private Image icon; 
        [SerializeField] private Image background; 
        [SerializeField] private TextMeshProUGUI text; 
        [SerializeField] private float fadeDuration;

        private SlotState currentState = SlotState.Normal;
        
        private bool isDisplayed;
        private bool hasItem;

        public bool IsDisplayed => isDisplayed;
        public bool HasItem => hasItem;

        public void Setup(PocketItem item, SlotState newState)
        {
            hasItem = item != null && !item.isEmpty;
            bool hasCount = hasItem && item!.item.canBeStacked && item.count > 1;
            
            text.text = hasCount ? item.count.ToString() : "";
            icon.sprite = hasItem ? item!.item.icon : null;
            
            SetState(newState);
        }
        
        public void Display()
        {
            StopAllCoroutines();

            isDisplayed = true;
            StartCoroutine(Tools.Fade(background, fadeDuration, true, 0.8f));
            StartCoroutine(Tools.Fade(icon, fadeDuration, true, 0.8f));
            StartCoroutine(Tools.Fade(text, fadeDuration, true, 0.8f));
        }

        public void Hide()
        {
            StopAllCoroutines();
         
            isDisplayed = false;
            StartCoroutine(Tools.Fade(background, fadeDuration, false, 0.8f));
            StartCoroutine(Tools.Fade(icon, fadeDuration, false, 0.8f));
            StartCoroutine(Tools.Fade(text, fadeDuration, false, 0.8f));
        }

        public void HideInstant()
        {
            StopAllCoroutines();
            
            isDisplayed = false;
            background.gameObject.SetActive(false);
            icon.gameObject.SetActive(false);
            text.gameObject.SetActive(false);
        }

        public void SetState(SlotState newState)
        {
            if (newState == currentState)
                return;

            currentState = newState;
            DisplaySlotFromState(currentState);
        }

        private void DisplaySlotFromState(SlotState slotState)
        {
            switch (slotState)
            {
                case SlotState.Normal:
                    icon = Tools.SetImageColor(icon, 0.8f);
                    text = Tools.SetTextColor(text, 0.8f);
                    break;
                case SlotState.Grabbed:
                    icon = Tools.SetImageColor(icon, 0.3f);
                    text = Tools.SetTextColor(text, 0.3f);
                    break;
                case SlotState.Deleting:
                    icon = Tools.SetImageColor(icon, 0.8f);
                    text = Tools.SetTextColor(text, 0.8f);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(slotState), slotState, null);
            }
        }
    }
}
