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
            Debug.Log("Setup !");
            for (int i = 0; i < slots.Count; i++)
            {
                string tmp = i < items.Count ? items.ElementAt(i).item.itemName : "empty";
                Debug.Log($"Setup : {tmp}");
                slots[i].Setup(i < items.Count ? items.ElementAt(i) : null);
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
    }
}
