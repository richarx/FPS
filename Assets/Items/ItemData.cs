using Inventory;
using UnityEngine;

namespace Items
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObjects/ItemData")]
    public class ItemData : ScriptableObject
    {
        public string itemName;
        [TextArea(10, 15)] public string description;
        public Sprite icon;
        public PocketDisplay.Pocket pocket;
        public bool canBeStacked;
    }
}
