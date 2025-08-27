using Items;
using UnityEngine;
using UnityEngine.Events;

namespace Inventory
{
    public class ToolBelt : MonoBehaviour
    {
        [HideInInspector] public UnityEvent<int> OnEquipNewTool = new UnityEvent<int>();
        
        private ItemData tool_1;
        private ItemData tool_2;
        private ItemData tool_3;
        private ItemData tool_4;

        public void EquipTool(ItemData item, int toolSlot)
        {
            if (toolSlot == 1)
                tool_1 = item;
            else if (toolSlot == 2)
                tool_2 = item;
            else if (toolSlot == 3)
                tool_3 = item;
            else if (toolSlot == 4)
                tool_4 = item;

            OnEquipNewTool.Invoke(toolSlot);
        }

        public ItemData GetTool(int toolSlot)
        {
            if (toolSlot == 1)
                return tool_1;
            else if (toolSlot == 2)
                return tool_2;
            else if (toolSlot == 3)
                return tool_3;
            else if (toolSlot == 4)
                return tool_4;

            return null;
        }
    }
}
