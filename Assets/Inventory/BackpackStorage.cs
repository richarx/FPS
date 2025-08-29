using System;
using System.Collections.Generic;
using System.Linq;
using Items;
using UnityEngine;
using UnityEngine.Events;

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

        public (bool, int) StoreItem(ItemData newItem, int count)
        {
            Debug.Log($"Store Item : {newItem.itemName}");
            
            if (newItem.canBeStacked)
            {
                PocketItem pocketItem = pocketItems.Find((i) => i.item == newItem);
                if (pocketItem != null)
                {
                    Debug.Log("Store Item : item incremented");
                    pocketItem.count += count;
                    return (true, pocketItems.IndexOf(pocketItem));
                }
            }

            if (IsFull())
            {
                Debug.Log("Store Item : Bag is full");
                return (false, -1);
            }

            PocketItem item = pocketItems.Find((p) => p.isEmpty);
            item.item = newItem;
            item.count = count;
            
            Debug.Log("Store Item : item added");
            return (true, pocketItems.IndexOf(item));
        }

        public void StoreItemAtIndex(ItemData newItem, int count, int index)
        {
            PocketItem item = pocketItems[index];
            item.item = newItem;
            item.count = count;
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

    public class BackpackStorage : MonoBehaviour
    {
        public enum Pocket
        {
            tools,
            component,
            ammo,
            medicine,
            toolBelt
        }

        public static UnityEvent<Pocket, int> OnUpdateSlot = new UnityEvent<Pocket, int>();
        
        private PocketStorage tools;
        private PocketStorage components;
        private PocketStorage ammo;
        private PocketStorage medicine;
        private PocketStorage toolBelt;

        private void Start()
        {
            tools = new PocketStorage(8);
            components = new PocketStorage(6);
            ammo = new PocketStorage(6);
            medicine = new PocketStorage(6);
            toolBelt = new PocketStorage(4);
        }

        public bool CanStoreItem(ItemData item)
        {
            return GetPocketStorage(item.pocket).CanStoreItem(item);
        }

        public void StoreItem(ItemData item, int count)
        {
            (bool wasItemStored, int slotIndex) = GetPocketStorage(item.pocket).StoreItem(item, count);
            
            if (wasItemStored)
                OnUpdateSlot?.Invoke(item.pocket, slotIndex);
        }
        
        public void SwapItems(Pocket pocket, int first, int second)
        {
            GetPocketStorage(pocket).SwapItems(first, second);
            OnUpdateSlot?.Invoke(pocket, first);
            OnUpdateSlot?.Invoke(pocket, second);
        }

        public void RemoveItem(Pocket pocket, int slotIndex)
        {
            GetPocketStorage(pocket).RemoveItem(slotIndex);
            OnUpdateSlot?.Invoke(pocket, slotIndex);
        }

        public void StoreItemInToolBelt(Pocket pocket, int pocketSlotIndex, int toolBeltSlotIndex)
        {
            PocketItem previousEquippedItem = GetItem(Pocket.toolBelt, toolBeltSlotIndex);
            if (previousEquippedItem != null && !previousEquippedItem.isEmpty)
                StoreItem(previousEquippedItem.item, previousEquippedItem.count);
            
            PocketItem item = GetItem(pocket, pocketSlotIndex);
            GetPocketStorage(Pocket.toolBelt).StoreItemAtIndex(item.item, item.count, toolBeltSlotIndex);
            OnUpdateSlot?.Invoke(Pocket.toolBelt, toolBeltSlotIndex);
            
            RemoveItem(pocket, pocketSlotIndex);
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
                case Pocket.toolBelt:
                    return toolBelt;
                default:
                    throw new ArgumentOutOfRangeException(nameof(pocket), pocket, null);
            }
        }

        public PocketItem GetItem(Pocket pocket, int slotIndex)
        {
            return GetPocketStorage(pocket).GetPocketItems[slotIndex];
        }
    }
}
