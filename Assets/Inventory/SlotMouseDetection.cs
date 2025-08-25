using UnityEngine;
using UnityEngine.Events;

namespace Inventory
{
    public class SlotMouseDetection : MonoBehaviour
    {
        private SlotDisplay slotDisplay;
        private RectTransform rectTransform;
        
        private int slotIndex;

        public static UnityEvent<RectTransform, int> OnSlotMouseOver = new UnityEvent<RectTransform, int>();

        private void Start()
        {
            slotDisplay = GetComponent<SlotDisplay>();
            rectTransform = GetComponent<RectTransform>();
        }

        private void OnMouseEnter()
        {
            if (slotDisplay.IsDisplayed)
                OnSlotMouseOver?.Invoke(rectTransform, slotIndex);
        }

        public void SetSlotIndex(int index)
        {
            slotIndex = index;
        }
    }
}
