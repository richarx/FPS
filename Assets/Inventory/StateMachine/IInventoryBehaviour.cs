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
        public void StartBehaviour(InventoryStateMachine player, InventoryBehaviourType previous);
        public void UpdateBehaviour(InventoryStateMachine player);
        public void StopBehaviour(InventoryStateMachine player, InventoryBehaviourType next);
        public InventoryBehaviourType GetBehaviourType();
    }
}
