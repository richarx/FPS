using UnityEngine.Events;

namespace Inventory.StateMachine
{
    public class InventoryOpenBackpack : IInventoryBehaviour
    {
        public UnityEvent OnOpenBackpack = new UnityEvent();
        
        public void StartBehaviour(InventoryStateMachine inventory, InventoryBehaviourType previous)
        {
            inventory.backpackDisplay.OpenBackpack();
            OnOpenBackpack?.Invoke();
        }

        public void UpdateBehaviour(InventoryStateMachine inventory)
        {
            if (inventory.backpackDisplay.IsDisplayed)
                inventory.ChangeBehaviour(inventory.openInventory);
        }

        public void StopBehaviour(InventoryStateMachine inventory, InventoryBehaviourType next)
        {
        }

        public InventoryBehaviourType GetBehaviourType()
        {
            return InventoryBehaviourType.OpenBackpack;
        }
    }
}
