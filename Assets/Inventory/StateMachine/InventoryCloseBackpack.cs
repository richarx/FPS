using UnityEngine;
using UnityEngine.Events;

namespace Inventory.StateMachine
{
    public class InventoryCloseBackpack : IInventoryBehaviour
    {
        public UnityEvent OnCloseBackpack = new UnityEvent();

        private float closeTimestamp;
        private bool isClosing;
        
        public void StartBehaviour(InventoryStateMachine inventory, InventoryBehaviourType previous)
        {
            isClosing = false;
            closeTimestamp = Time.time;
        }

        public void UpdateBehaviour(InventoryStateMachine inventory)
        {
            if (!isClosing && Time.time - closeTimestamp >= 0.3f)
            {
                isClosing = true;
                inventory.backpackDisplay.CloseBackpack(inventory.currentPocket);
            }
            
            if (!inventory.backpackDisplay.IsDisplayed)
                inventory.ChangeBehaviour(inventory.hidden);
        }

        public void StopBehaviour(InventoryStateMachine inventory, InventoryBehaviourType next)
        {
            OnCloseBackpack?.Invoke();
        }

        public InventoryBehaviourType GetBehaviourType()
        {
            return InventoryBehaviourType.CloseBackpack;
        }
    }
}
