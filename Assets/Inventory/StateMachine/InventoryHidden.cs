namespace Inventory.StateMachine
{
    public class InventoryHidden : IInventoryBehaviour
    {
        public void StartBehaviour(InventoryStateMachine player, InventoryBehaviourType previous)
        {
        }

        public void UpdateBehaviour(InventoryStateMachine player)
        {
        }

        public void StopBehaviour(InventoryStateMachine player, InventoryBehaviourType next)
        {
        }

        public InventoryBehaviourType GetBehaviourType()
        {
            return InventoryBehaviourType.Hidden;
        }
    }
}
