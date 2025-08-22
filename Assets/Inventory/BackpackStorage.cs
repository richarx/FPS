using System;
using System.Collections.Generic;
using Items;
using UnityEngine;

namespace Inventory
{
    public class PocketItem
    {
        public ItemData item;
        public int count;

        public PocketItem(ItemData data)
        {
            item = data;
            count = 1;
        }
    }
    
    public class PocketStorage
    {
        private List<PocketItem> pocketItems;
        private int maxSlotCount;

        public IReadOnlyCollection<PocketItem> GetPocketItems => pocketItems.AsReadOnly();

        public bool isFull => pocketItems.Count + 1 >= maxSlotCount;

        public bool CanStoreItem(ItemData newItem)
        {
            if (!isFull)
                return true;
            
            if (newItem.canBeStacked)
            {
                PocketItem pocketItem = pocketItems.Find((i) => i.item == newItem);
                if (pocketItem != null)
                    return true;
            }

            return false;
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
            
            if (isFull)
                return;
            
            pocketItems.Add(new PocketItem(newItem));
            Debug.Log("Store Item : item added");
        }

        public PocketStorage(int slotCount)
        {
            pocketItems = new List<PocketItem>();
            maxSlotCount = slotCount;
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
    }
}
