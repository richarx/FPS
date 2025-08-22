using UnityEngine;

namespace Items
{
    public class LootItem : MonoBehaviour
    {
        [SerializeField] private ItemData itemData;

        public ItemData GetItemData()
        {
            return itemData;
        }
    }
}
