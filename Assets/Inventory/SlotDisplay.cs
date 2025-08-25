using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{
    public class SlotDisplay : MonoBehaviour
    {
        [SerializeField] private Image icon; 
        [SerializeField] private Image background; 
        [SerializeField] private TextMeshProUGUI text; 
        [SerializeField] private float fadeDuration;

        private bool isDisplayed;
        private bool hasItem;

        public bool IsDisplayed => isDisplayed;

        public void Setup(PocketItem item)
        {
            hasItem = item != null;
            bool hasCount = hasItem && item!.item.canBeStacked && item.count > 1;
            
            text.text = hasCount ? item.count.ToString() : "";
            icon.sprite = hasItem ? item!.item.icon : null;
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
    }
}
