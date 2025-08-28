using UnityEngine;
using UnityEngine.Events;

namespace Inventory
{
    public class SlotMouseDetection : MonoBehaviour
    {
        private SlotDisplay slotDisplay;
        private RectTransform rectTransform;

        private int slotIndex;

        public static UnityEvent<RectTransform, int, bool> OnSlotMouseOver = new UnityEvent<RectTransform, int, bool>();

        private void Start()
        {
            slotDisplay = GetComponent<SlotDisplay>();
            rectTransform = GetComponent<RectTransform>();
        }

        private void OnMouseEnter()
        {
            if (slotDisplay.IsDisplayed)
                OnSlotMouseOver?.Invoke(rectTransform, slotIndex, slotDisplay.IsToolBeltSLot);
        }

        public void SetSlotIndex(int index)
        {
            slotIndex = index;
        }
    }
}
