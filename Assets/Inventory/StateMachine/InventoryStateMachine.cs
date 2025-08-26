using Backpack;
using Player.Scripts;
using Tools_and_Scripts;
using UnityEngine;
using UnityEngine.Events;
using static Inventory.BackpackStorage;

namespace Inventory.StateMachine
{
    public class InventoryStateMachine : MonoBehaviour
    {
        public static InventoryStateMachine instance;
        
        [HideInInspector] public UnityEvent<Pocket> OnSwitchPocketTarget = new UnityEvent<Pocket>();

        [HideInInspector] public PlayerStateMachine player;

        public BackpackDisplay backpackDisplay;
        [HideInInspector] public InventoryDisplay inventoryDisplay;
        [HideInInspector] public PocketSwitcher pocketSwitcher;

        public Pocket currentPocket = Pocket.tools;

        public bool isDisplayed => currentBehaviour.GetBehaviourType() != InventoryBehaviourType.Hidden;

        public InventoryHidden hidden = new InventoryHidden();
        public InventoryOpenBackpack openBackpack = new InventoryOpenBackpack();
        public InventoryCloseBackpack closeBackpack = new InventoryCloseBackpack();
        public InventoryOpenInventory openInventory = new InventoryOpenInventory();
        public InventoryCloseInventory closeInventory = new InventoryCloseInventory();
        public InventoryGamepadMove gamepadMove = new InventoryGamepadMove();
        public InventoryKeyboardMove keyboardMove = new InventoryKeyboardMove();
        
        public IInventoryBehaviour currentBehaviour;

        private void Awake()
        {
            instance = this;
        }
        
        private void Start()
        {
            player = PlayerStateMachine.instance;
            
            inventoryDisplay = GetComponent<InventoryDisplay>();
            pocketSwitcher = new PocketSwitcher();
            
            currentBehaviour = hidden;
            currentBehaviour.StartBehaviour(this, InventoryBehaviourType.Hidden);
        }
        
        private void Update()
        {
            if (!isDisplayed)
                return;
            
            (bool isPocketChangeRequested, Pocket targetPocket) = pocketSwitcher.CheckForPocketInput();
            
            if (isPocketChangeRequested)
                SwitchPocket(targetPocket);
            
            currentBehaviour.UpdateBehaviour(this);
        }
        
        public void ChangeBehaviour(IInventoryBehaviour newBehaviour)
        {
            if (newBehaviour == null || newBehaviour == currentBehaviour)
                return;

            InventoryBehaviourType previous = currentBehaviour.GetBehaviourType();
            currentBehaviour.StopBehaviour(this, newBehaviour.GetBehaviourType());
            currentBehaviour = newBehaviour;
            
            currentBehaviour.StartBehaviour(this, previous);
        }

        public void OpenBackpack()
        {
            ChangeBehaviour(openBackpack);
        }

        public void CloseBackpack()
        {
            ChangeBehaviour(closeInventory);
        }

        public void SwitchPocket(Pocket next)
        {
            if (next == currentPocket)
                return;

            backpackDisplay.SwitchPocket(currentPocket, next);
            currentPocket = next;
            OnSwitchPocketTarget?.Invoke(currentPocket);
        }

        public void ChangeToMovementBehaviour()
        {
            ChangeBehaviour(player.inputPackage.lastInputType == InputType.Gamepad ? gamepadMove : keyboardMove);
        }
        
        public Transform GetCurrentLookTarget()
        {
            return isDisplayed ? backpackDisplay.lookTargets[(int)currentPocket] : null;
        }
    }
}
