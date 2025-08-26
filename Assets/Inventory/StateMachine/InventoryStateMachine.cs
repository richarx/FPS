using Backpack;
using Player.Scripts;
using Tools_and_Scripts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static Inventory.BackpackStorage;

namespace Inventory.StateMachine
{
    public class InventoryStateMachine : MonoBehaviour
    {
        public static InventoryStateMachine instance;
        
        [HideInInspector] public UnityEvent<Pocket> OnSwitchPocketTarget = new UnityEvent<Pocket>();

        [HideInInspector] public PlayerStateMachine player;

        public RectTransform canvas;
        public RectTransform pointer;
        public BackpackDisplay backpackDisplay;
        public Image itemPickedUp;
        [HideInInspector] public RectTransform itemPickedUpRect;
        [HideInInspector] public InventoryDisplay inventoryDisplay;
        [HideInInspector] public InventoryCursor inventoryCursor;
        [HideInInspector] public PocketSwitcher pocketSwitcher;

        [HideInInspector] public Pocket currentPocket = Pocket.tools;

        public bool isDisplayed => currentBehaviour.GetBehaviourType() != InventoryBehaviourType.Hidden;

        public InventoryHidden hidden = new InventoryHidden();
        public InventoryOpenBackpack openBackpack = new InventoryOpenBackpack();
        public InventoryCloseBackpack closeBackpack = new InventoryCloseBackpack();
        public InventoryOpenInventory openInventory = new InventoryOpenInventory();
        public InventoryCloseInventory closeInventory = new InventoryCloseInventory();
        public InventoryGamepadMove gamepadMove = new InventoryGamepadMove();
        public InventoryKeyboardMove keyboardMove;
        public InventoryGamepadGrab gamepadGrab = new InventoryGamepadGrab();
        public InventoryKeyboardGrab keyboardGrab = new InventoryKeyboardGrab();
        
        public IInventoryBehaviour currentBehaviour;

        private void Awake()
        {
            instance = this;
        }
        
        private void Start()
        {
            player = PlayerStateMachine.instance;
            
            keyboardMove = new InventoryKeyboardMove(instance);
            
            inventoryDisplay = GetComponent<InventoryDisplay>();
            inventoryCursor = GetComponent<InventoryCursor>();
            pocketSwitcher = new PocketSwitcher();
            itemPickedUpRect = itemPickedUp.GetComponent<RectTransform>();
            
            currentBehaviour = hidden;
            currentBehaviour.StartBehaviour(this, InventoryBehaviourType.Hidden);
            
            pointer.gameObject.SetActive(false);
        }
        
        private void Update()
        {
            if (!isDisplayed)
                return;
            
            (bool isPocketChangeRequested, Pocket targetPocket) = pocketSwitcher.CheckForPocketInput();

            if (isPocketChangeRequested)
            {
                SwitchPocket(targetPocket);

                if (currentBehaviour.GetBehaviourType() != InventoryBehaviourType.MoveGamepad)
                {
                    ChangeToMovementBehaviour();
                    return;
                }
            }
            
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
        
        public void ChangeToGrabBehaviour()
        {
            ChangeBehaviour(player.inputPackage.lastInputType == InputType.Gamepad ? gamepadGrab : keyboardGrab);
        }
        
        public Transform GetCurrentLookTarget()
        {
            return isDisplayed ? backpackDisplay.lookTargets[(int)currentPocket] : null;
        }

        public SlotDisplay GetCurrentDisplaySlot()
        {
            return inventoryDisplay.ComputePocket(currentPocket).Slots[inventoryCursor.currentSlotIndex];
        }
        
        public PocketItem GetCurrentStorageSlot()
        {
            return player.backpackStorage.GetPocketStorage(currentPocket).GetPocketItems[inventoryCursor.currentSlotIndex];
        }
    }
}
