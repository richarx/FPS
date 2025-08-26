using System;
using System.Collections.Generic;
using System.Linq;
using Items;
using UnityEngine;

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

        public IReadOnlyCollection<PocketItem> GetPocketItems => pocketItems.AsReadOnly();


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

        public (PocketItem, PocketItem) SwapItems(Pocket pocket, int first, int second)
        {
            return GetPocketStorage(pocket).SwapItems(first, second);
        }
    }
}
