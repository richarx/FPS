using System;
using System.Collections.Generic;
using System.Linq;
using Items;
using UnityEngine;
using static Inventory.BackpackStorage;

namespace Inventory
{
    public class PocketItem
    {
        public ItemData item;
        public int count;
        public bool isEmpty => item == null;

        public PocketItem(ItemData data)
        {
            item = data;
            count = isEmpty ? 0 : 1;
        }
    }
    
    public class PocketStorage
    {
        private List<PocketItem> pocketItems;

        public List<PocketItem> GetPocketItems => pocketItems;


        public bool CanStoreItem(ItemData newItem)
        {
            if (!IsFull())
                return true;
            
            if (newItem.canBeStacked)
            {
                PocketItem pocketItem = pocketItems.Find((i) => i.item == newItem);
                if (pocketItem != null)
                    return true;
            }

            return false;
        }

        private bool IsFull()
        {
            return ComputeItemCount() >= pocketItems.Count;
        }

        private int ComputeItemCount()
        {
            return pocketItems.Count((p) => p.isEmpty == false);
        }

        public void StoreItem(ItemData newItem)
        {
            Debug.Log($"Store Item : {newItem.itemName}");
            
            if (newItem.canBeStacked)
            {
                PocketItem pocketItem = pocketItems.Find((i) => i.item == newItem);
                if (pocketItem != null)
                {
                    Debug.Log("Store Item : item incremented");
                    pocketItem.count += 1;
                    return;
                }
            }

            if (IsFull())
            {
                Debug.Log("Store Item : Bag is full");
                return;
            }

            PocketItem item = pocketItems.Find((p) => p.isEmpty);
            item.item = newItem;
            item.count = 1;
            
            Debug.Log("Store Item : item added");
        }

        public PocketStorage(int slotCount)
        {
            pocketItems = new List<PocketItem>();

            for (int i = 0; i < slotCount; i++)
            {
                pocketItems.Add(new PocketItem(null));
            }
        }

        public (PocketItem, PocketItem) SwapItems(int first, int second)
        {
            (pocketItems[first], pocketItems[second]) = (pocketItems[second], pocketItems[first]);
            return (pocketItems[first], pocketItems[second]);
        }

        public void RemoveItem(int slotIndex)
        {
            pocketItems[slotIndex].item = null;
            pocketItems[slotIndex].count = 0;
        }
    }

    public class ToolBeltSlot
    {
        public PocketItem pocketItem;
        public bool hasItem => pocketItem != null;
    }
    
    public class BackpackStorage : MonoBehaviour
    {
        public enum Pocket
        {
            tools,
            component,
            ammo,
            medicine
        }
        
        private PocketStorage tools;
        private PocketStorage components;
        private PocketStorage ammo;
        private PocketStorage medicine;

        private ToolBeltSlot slot_1 = new ToolBeltSlot();
        private ToolBeltSlot slot_2 = new ToolBeltSlot();
        private ToolBeltSlot slot_3 = new ToolBeltSlot();
        private ToolBeltSlot slot_4 = new ToolBeltSlot();

        private void Start()
        {
            tools = new PocketStorage(8);
            components = new PocketStorage(6);
            ammo = new PocketStorage(6);
            medicine = new PocketStorage(6);
        }

        public bool CanStoreItem(ItemData item)
        {
            return GetPocketStorage(item.pocket).CanStoreItem(item);
        }

        public void StoreItem(ItemData item)
        {
            GetPocketStorage(item.pocket).StoreItem(item);
        }
        
        public (PocketItem, PocketItem) SwapItems(Pocket pocket, int first, int second)
        {
            return GetPocketStorage(pocket).SwapItems(first, second);
        }

        public void StoreItemInToolBelt(Pocket pocket, int itemSlotIndex, int toolBeltIndex)
        {
            GetToolBeltSlot(toolBeltIndex).pocketItem = GetPocketStorage(pocket).GetPocketItems[itemSlotIndex];
        }

        public void RemoveItem(Pocket pocket, int slotIndex)
        {
            GetPocketStorage(pocket).RemoveItem(slotIndex);
        }

        public PocketStorage GetPocketStorage(Pocket pocket)
        {
            switch (pocket)
            {
                case Pocket.tools:
                    return tools;
                case Pocket.component:
                    return components;
                case Pocket.ammo:
                    return ammo;
                case Pocket.medicine:
                    return medicine;
                default:
                    throw new ArgumentOutOfRangeException(nameof(pocket), pocket, null);
            }
        }

        public ToolBeltSlot GetToolBeltSlot(int slotIndex)
        {
            if (slotIndex == 1)
                return slot_1;
            else if (slotIndex == 2)
                return slot_2;
            else if (slotIndex == 3)
                return slot_3;
            else if (slotIndex == 4)
                return slot_4;

            return slot_1;
        }

        public ItemData GetItem(Pocket pocket, int slotIndex)
        {
            return GetPocketStorage(pocket).GetPocketItems[slotIndex].item;
        }
    }
}
