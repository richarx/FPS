using System;
using System.Collections.Generic;
using Inventory.StateMachine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{
    public class ToolBeltDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private Image line;
        [SerializeField] private Image background;
        [SerializeField] private List<SlotDisplay> slots;
        
        private InventoryStateMachine inventory;
        
        private void Start()
        {
            inventory = InventoryStateMachine.instance;
            
            inventory.openBackpack.OnOpenBackpack.AddListener(DisplayToolBelt);
            inventory.closeInventory.OnCloseInventory.AddListener(HideToolBelt);
            inventory.OnEquipItem.AddListener(EquipItem);

            title.gameObject.SetActive(false);
            line.gameObject.SetActive(false);
            background.gameObject.SetActive(false);

            for (int i = 0; i < slots.Count; i++)
            {
                slots[i].Setup(null, SlotDisplay.SlotState.Normal);
                slots[i].HideInstant();
                slots[i].GetComponent<SlotMouseDetection>().SetSlotIndex(i);
            }
        }

        private void EquipItem(int toolBeltSlotIndex)
        {
            slots[toolBeltSlotIndex].Setup(inventory.player.backpackStorage.GetToolBeltSlot(toolBeltSlotIndex).pocketItem ,SlotDisplay.SlotState.Normal);
        }

        private void DisplayToolBelt()
        {
            StopAllCoroutines();
            StartCoroutine(Tools.Fade(title, 0.2f, true));
            StartCoroutine(Tools.Fade(line, 0.2f, true));
            StartCoroutine(Tools.Fade(background, 0.2f, true, 0.2f));
            
            foreach (SlotDisplay slot in slots)
            {
                slot.Display();
            }
        }
        
        private void HideToolBelt()
        {
            StartCoroutine(Tools.Fade(title, 0.2f, false));
            StartCoroutine(Tools.Fade(line, 0.2f, false));
            StartCoroutine(Tools.Fade(background, 0.2f, false, 0.2f));
            
            foreach (SlotDisplay slot in slots)
            {
                slot.Hide();
            }
        }

        public SlotDisplay GetToolBeltSlot(int index)
        {
            return slots[index];
        }
    }
}
