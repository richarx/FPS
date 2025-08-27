using Backpack;
using Inventory;
using UnityEngine;

namespace Items
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObjects/ItemData")]
    public class ItemData : ScriptableObject
    {
        public string itemName;
        [TextArea(10, 15)] public string description;
        public GameObject inHandPrefab;
        public GameObject thrownPrefab;
        public GameObject lootPrefab;
        public Sprite icon;
        public BackpackStorage.Pocket pocket;
        public bool canBeStacked;
    }
}
