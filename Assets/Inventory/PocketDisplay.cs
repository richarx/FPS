using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{
    public class PocketDisplay : MonoBehaviour
    {
        [SerializeField] private List<SlotDisplay> slots;
        [SerializeField] private Image background;
        [SerializeField] private float fadeDuration;
        [SerializeField] private int width;
        [SerializeField] private int height;

        public List<SlotDisplay> Slots => slots;
        public int Width => width;
        public int Height => height;
        
        public void Setup(IReadOnlyCollection<PocketItem> items)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                slots[i].Setup(i < items.Count ? items.ElementAt(i) : null, SlotDisplay.SlotState.Normal);
                slots[i].GetComponent<SlotMouseDetection>().SetSlotIndex(i);
            }
        }
        
        public void Display()
        {
            StopAllCoroutines();
            StartCoroutine(Tools.Fade(background, fadeDuration, true, 0.2f));
            
            foreach (SlotDisplay slot in slots)
                slot.Display();
        }
        
        public void Hide()
        {
            StopAllCoroutines();
            StartCoroutine(Tools.Fade(background, fadeDuration, false, 0.2f));
            
            foreach (SlotDisplay slot in slots)
                slot.Hide();
        }
        
        public void HideInstant()
        {
            StopAllCoroutines();
            background.gameObject.SetActive(false);
            
            foreach (SlotDisplay slot in slots)
                slot.HideInstant();
        }

        public void SwapItems(int firstIndex, int secondIndex, PocketItem firstData, PocketItem secondData)
        {
            slots[firstIndex].Setup(firstData, SlotDisplay.SlotState.Normal);
            slots[secondIndex].Setup(secondData, SlotDisplay.SlotState.Normal);
        }
    }
}
