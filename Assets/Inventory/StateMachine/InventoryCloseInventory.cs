namespace Inventory.StateMachine
{
    public class InventoryCloseInventory : IInventoryBehaviour
    {
        public void StartBehaviour(InventoryStateMachine inventory, InventoryBehaviourType previous)
        {
            inventory.inventoryDisplay.HidePocket();
        }

        public void UpdateBehaviour(InventoryStateMachine inventory)
        {
            inventory.ChangeBehaviour(inventory.closeBackpack);
        }

        public void StopBehaviour(InventoryStateMachine inventory, InventoryBehaviourType next)
        {
        }

        public InventoryBehaviourType GetBehaviourType()
        {
            return InventoryBehaviourType.CloseInventory;
        }
    }
}
