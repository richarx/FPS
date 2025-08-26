using UnityEngine.Events;

namespace Inventory.StateMachine
{
    public class InventoryCloseBackpack : IInventoryBehaviour
    {
        public UnityEvent OnCloseBackpack = new UnityEvent();
        
        public void StartBehaviour(InventoryStateMachine inventory, InventoryBehaviourType previous)
        {
            inventory.backpackDisplay.CloseBackpack();
            
            OnCloseBackpack?.Invoke();
        }

        public void UpdateBehaviour(InventoryStateMachine inventory)
        {
            if (!inventory.backpackDisplay.IsDisplayed)
                inventory.ChangeBehaviour(inventory.hidden);
        }

        public void StopBehaviour(InventoryStateMachine inventory, InventoryBehaviourType next)
        {
        }

        public InventoryBehaviourType GetBehaviourType()
        {
            return InventoryBehaviourType.CloseBackpack;
        }
    }
}
