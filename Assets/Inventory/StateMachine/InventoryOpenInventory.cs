namespace Inventory.StateMachine
{
    public class InventoryOpenInventory : IInventoryBehaviour
    {
        public void StartBehaviour(InventoryStateMachine inventory, InventoryBehaviourType previous)
        {
            inventory.inventoryDisplay.DisplayPocket(inventory.currentPocket);
        }

        public void UpdateBehaviour(InventoryStateMachine inventory)
        {
            if (inventory.inventoryDisplay.IsDisplayed)
                inventory.ChangeToMovementBehaviour();
        }

        public void StopBehaviour(InventoryStateMachine inventory, InventoryBehaviourType next)
        {
        }

        public InventoryBehaviourType GetBehaviourType()
        {
            return InventoryBehaviourType.OpenInventory;
        }
    }
}
