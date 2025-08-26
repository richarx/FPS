namespace Inventory.StateMachine
{
    public class InventoryKeyboardMove : IInventoryBehaviour
    {
        public void StartBehaviour(InventoryStateMachine inventory, InventoryBehaviourType previous)
        {
        }

        public void UpdateBehaviour(InventoryStateMachine inventory)
        {
            
        }

        public void StopBehaviour(InventoryStateMachine inventory, InventoryBehaviourType next)
        {
        }

        public InventoryBehaviourType GetBehaviourType()
        {
            return InventoryBehaviourType.MoveKeyboard;
        }
    }
}
