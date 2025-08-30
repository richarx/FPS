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
        [SerializeField] private Transform toolBeltPivot;
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private Image line;
        [SerializeField] private Image background;
        [SerializeField] private List<SlotDisplay> slots;
        
        private Vector3 leftCornerPosition = new Vector3(-700.0f, 350.0f, 0.0f);
        private Vector3 rightCornerPosition = new Vector3(700.0f, 350.0f, 0.0f);
        private bool isRightCorner = true;
        public bool IsRightCorner => isRightCorner;
        private Vector3 targetPosition => isRightCorner ? rightCornerPosition : leftCornerPosition;
        private Vector3 velocity;
        
        private InventoryStateMachine inventory;
        
        private void Start()
        {
            inventory = InventoryStateMachine.instance;
            
            inventory.openBackpack.OnOpenBackpack.AddListener(DisplayToolBelt);
            inventory.closeInventory.OnCloseInventory.AddListener(HideToolBelt);
            BackpackStorage.OnUpdateSlot.AddListener(UpdateSlot);
            inventory.OnSwitchPocketTarget.AddListener(SwitchPocket);

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

        private void Update()
        {
            if (!inventory.player.isBackpackOpen)
                return;
            
            toolBeltPivot.localPosition = Vector3.SmoothDamp(toolBeltPivot.localPosition, targetPosition, ref velocity, 0.15f);
        }

        private void SwitchPocket(BackpackStorage.Pocket newPocket)
        {
            if (isRightCorner && newPocket == BackpackStorage.Pocket.ammo)
                isRightCorner = false;
            else if (!isRightCorner && newPocket == BackpackStorage.Pocket.medicine)
                isRightCorner = true;
        }

        private void UpdateSlot(BackpackStorage.Pocket pocket, int index)
        {
            if (!inventory.player.isBackpackOpen)
                return;
            
            if (pocket == BackpackStorage.Pocket.toolBelt)
                slots[index].Setup(inventory.player.backpackStorage.GetItem(BackpackStorage.Pocket.toolBelt, index), SlotDisplay.SlotState.Normal);
        }

        private void DisplayToolBelt()
        {
            StopAllCoroutines();
            StartCoroutine(Tools.Fade(title, 0.5f, true));
            StartCoroutine(Tools.Fade(line, 0.8f, true));
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
