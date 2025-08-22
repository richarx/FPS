using System.Collections.Generic;
using System.Linq;
using Backpack;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{
    public class PocketDisplay : MonoBehaviour
    {
        [SerializeField] private List<SlotDisplay> slots;
        [SerializeField] private Image background;
        [SerializeField] private float fadeDuration;

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
