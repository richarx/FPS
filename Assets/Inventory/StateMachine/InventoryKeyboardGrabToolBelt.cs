namespace Inventory.StateMachine
{
    public class InventoryKeyboardGrabToolBelt : IInventoryBehaviour
    {
        private int currentToolBeltSlotIndex;
        
        public void StartBehaviour(InventoryStateMachine inventory, InventoryBehaviourType previous)
        {
            currentToolBeltSlotIndex = inventory.inventoryCursor.currentSlotIndex;
        }

        public void UpdateBehaviour(InventoryStateMachine inventory)
        {
        }

        public void StopBehaviour(InventoryStateMachine inventory, InventoryBehaviourType next)
        {
        }

        public InventoryBehaviourType GetBehaviourType()
        {
            return InventoryBehaviourType.EquipKeyboard;
        }
    }
}
