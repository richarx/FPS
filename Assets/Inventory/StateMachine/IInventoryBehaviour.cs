namespace Inventory.StateMachine
{
    public enum InventoryBehaviourType 
    {
        Hidden,
        OpenBackpack,
        CloseBackpack,
        OpenInventory,
        CloseInventory,
        MoveGamepad,
        MoveKeyboard,
        GrabGamepad,
        GrabKeyboard,
        EquipGamepad,
        EquipKeyboard,
        ThrowAwayGamepad,
        ThrowAwayKeyboard,
    }
    
    public interface IInventoryBehaviour
    {
        public void StartBehaviour(InventoryStateMachine inventory, InventoryBehaviourType previous);
        public void UpdateBehaviour(InventoryStateMachine inventory);
        public void StopBehaviour(InventoryStateMachine inventory, InventoryBehaviourType next);
        public InventoryBehaviourType GetBehaviourType();
    }
}
