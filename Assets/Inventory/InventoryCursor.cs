using System.Collections;
using Player.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{
    public class InventoryCursor : MonoBehaviour
    {
        [SerializeField] private RectTransform cursor;
        [SerializeField] private float smoothTime;

        [HideInInspector] public int currentSlotIndex; 
        
        private InventoryDisplay inventoryDisplay;

        private Image cursorImage;
        
        private RectTransform targetPosition;
        private bool hasTarget => targetPosition != null;
        
        private Vector3 velocity;

        private int pocketSwitchCount;

        private void Start()
        {
            inventoryDisplay = GetComponent<InventoryDisplay>();
            cursorImage = cursor.GetComponent<Image>();
            
            PlayerStateMachine player = PlayerStateMachine.instance;
            
            player.playerBackpack.OnOpenBag.AddListener(DisplayCursor);
            inventoryDisplay.OnDisplayNewPocket.AddListener(SwitchPocket);
            player.playerBackpack.OnCloseBag.AddListener(HideCursor);
            
            cursor.gameObject.SetActive(false);
        }
        
        private void Update()
        {
            if (hasTarget)
                cursor.position = Vector3.SmoothDamp(cursor.position, targetPosition.position, ref velocity, smoothTime);
        }
        
        private void SwitchPocket()
        {
            pocketSwitchCount += 1;
            currentSlotIndex = 0;
            targetPosition = inventoryDisplay.CurrentPocket.Slots[0].GetComponent<RectTransform>();
            
            if (pocketSwitchCount <= 1)
                cursor.position = targetPosition.position;
        }
        private void DisplayCursor()
        {
            StopAllCoroutines();
            StartCoroutine(DisplayCursorCoroutine());
        }

        private IEnumerator DisplayCursorCoroutine()
        {
            pocketSwitchCount = 0;
            yield return new WaitForSeconds(0.5f);
            yield return Tools.Fade(cursorImage, 0.2f, true);
        }

        private void HideCursor()
        {
            StopAllCoroutines();
            StartCoroutine(Tools.Fade(cursorImage, 0.2f, false));
        }

        public void SetTargetPosition(RectTransform newPosition, int newSlotIndex)
        {
            targetPosition = newPosition;
            currentSlotIndex = newSlotIndex;
        }
    }
}
