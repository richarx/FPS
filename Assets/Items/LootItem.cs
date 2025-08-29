using UnityEngine;

namespace Items
{
    public class LootItem : MonoBehaviour
    {
        [SerializeField] private ItemData itemData;
        [SerializeField] private int amountLooted;

        public ItemData GetItemData()
        {
            return itemData;
        }

        public int GetAmountLooted()
        {
            return amountLooted;
        }
    }
}
