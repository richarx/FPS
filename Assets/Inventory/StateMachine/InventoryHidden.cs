namespace Inventory.StateMachine
{
    public class InventoryHidden : IInventoryBehaviour
    {
        public void StartBehaviour(InventoryStateMachine inventory, InventoryBehaviourType previous)
        {
            inventory.player.ChangeBehaviour(inventory.player.playerRun);
        }

        public void UpdateBehaviour(InventoryStateMachine inventory)
        {
        }

        public void StopBehaviour(InventoryStateMachine inventory, InventoryBehaviourType next)
        {
        }

        public InventoryBehaviourType GetBehaviourType()
        {
            return InventoryBehaviourType.Hidden;
        }
    }
}
