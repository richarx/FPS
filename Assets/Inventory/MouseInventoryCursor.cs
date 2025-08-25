using Player.Scripts;
using Tools_and_Scripts;
using UnityEngine;

namespace Inventory
{
    public class MouseInventoryCursor : MonoBehaviour
    {
        [SerializeField] private RectTransform canvas;
        [SerializeField] private RectTransform pointer;
        
        private PlayerStateMachine player;
        private InventoryCursor inventoryCursor;
        private InventoryDisplay inventoryDisplay;

        private bool isPointerDisplayed;
        
        private void Start()
        {
            inventoryCursor = GetComponent<InventoryCursor>();
            inventoryDisplay = GetComponent<InventoryDisplay>();
            player = PlayerStateMachine.instance;
         
            SlotMouseDetection.OnSlotMouseOver.AddListener(MoveCursorToSlot);
            
            pointer.gameObject.SetActive(false);
        }

        private void MoveCursorToSlot(RectTransform slot, int slotIndex)
        {
            if (!player.isBackpackOpen || player.inputPackage.lastInputType != InputType.Keyboard)
                return;
            
            inventoryCursor.SetTargetPosition(slot, slotIndex);
        }

        private void Update()
        {
            if (!player.isBackpackOpen || player.inputPackage.lastInputType != InputType.Keyboard)
            {
                if (isPointerDisplayed)
                    HidePointer();
                return;
            }

            if (!isPointerDisplayed)
                DisplayPointer();

            pointer.anchoredPosition = CameraScreenPosition.instance.GetMousePosition(canvas);
        }

        private void DisplayPointer()
        {
            isPointerDisplayed = true;
            Cursor.lockState = CursorLockMode.Confined;
            pointer.gameObject.SetActive(true);
        }

        private void HidePointer()
        {
            isPointerDisplayed = false;
            Cursor.lockState = CursorLockMode.Locked;
            pointer.gameObject.SetActive(false);
        }
    }
}
