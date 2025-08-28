using Backpack;
using Items;
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
        [HideInInspector] public UnityEvent<int> OnEquipItem = new UnityEvent<int>();


        [HideInInspector] public PlayerStateMachine player;

        public RectTransform canvas;
        public RectTransform pointer;
        public BackpackDisplay backpackDisplay;
        public Image itemPickedUp;
        [HideInInspector] public RectTransform itemPickedUpRect;
        [HideInInspector] public InventoryDisplay inventoryDisplay;
        [HideInInspector] public InventoryCursor inventoryCursor;
        [HideInInspector] public PocketSwitcher pocketSwitcher;
        [HideInInspector] public ToolBeltDisplay toolBeltDisplay;

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
        public InventoryGamepadThrow gamepadThrow = new InventoryGamepadThrow();
        public InventoryKeyboardThrow keyboardThrow = new InventoryKeyboardThrow();
        public InventoryGamepadEquip gamepadEquip = new InventoryGamepadEquip();
        public InventoryKeyboardGrabToolBelt keyboardGrabToolBelt = new InventoryKeyboardGrabToolBelt();
        
        public IInventoryBehaviour currentBehaviour;

        private bool hasSkippedAFrame;
        
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
            toolBeltDisplay = GetComponent<ToolBeltDisplay>();
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

            if (hasSkippedAFrame && player.inputPackage.GetBackpack.wasPressedThisFrame)
            {
                CloseBackpack();
                return;   
            }
            hasSkippedAFrame = true;
            
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
            hasSkippedAFrame = false;
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
        
        public void ChangeToThrowBehaviour()
        {
            ChangeBehaviour(player.inputPackage.lastInputType == InputType.Gamepad ? gamepadThrow : keyboardThrow);
        }

        public Transform GetCurrentLookTarget()
        {
            if (!isDisplayed || currentBehaviour.GetBehaviourType() == InventoryBehaviourType.OpenBackpack)
                return null;

            if (!backpackDisplay.IsDisplayed || currentBehaviour.GetBehaviourType() == InventoryBehaviourType.CloseBackpack)
                return backpackDisplay.lookTargets[0];
            else
                return backpackDisplay.lookTargets[(int)currentPocket];
        }

        public SlotDisplay GetCurrentDisplaySlot()
        {
            return inventoryDisplay.ComputePocket(currentPocket).Slots[inventoryCursor.currentSlotIndex];
        }
        
        public PocketItem GetCurrentStorageSlot()
        {
            return player.backpackStorage.GetPocketStorage(currentPocket).GetPocketItems[inventoryCursor.currentSlotIndex];
        }

        public void ThrowItem(ItemData item)
        {
            Vector3 position = player.playerShootGun.shootingPosition;
            //position += Vector3.up * player.playerData.throwWeaponHeightOffset;

            Rigidbody rb = Instantiate(item.lootPrefab, position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(player.playerShootGun.shootingDirection * 20.0f, ForceMode.Impulse);
        }
        
        public void EquipItem(int startingSlotIndex, int currentToolBeltSlotIndex)
        {
            Debug.Log($"Equip item : {startingSlotIndex} / {currentToolBeltSlotIndex}");
            player.backpackStorage.StoreItemInToolBelt(currentPocket, startingSlotIndex, currentToolBeltSlotIndex);
            OnEquipItem?.Invoke(currentToolBeltSlotIndex);
        }
    }
}
