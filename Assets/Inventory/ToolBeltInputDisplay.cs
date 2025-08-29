using System.Collections;
using Inventory.StateMachine;
using Player.Scripts;
using Tools_and_Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{
    public class ToolBeltInputDisplay : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private Image square;
        [SerializeField] private Image background;
    
        [Space]
        [SerializeField] private Sprite gamepadSprite;
        [SerializeField] private Sprite keyboardSprite;

        private PlayerStateMachine player;
        private InputType currentInputType => player.inputPackage.lastInputType;
        
        private void Start()
        {
            player = PlayerStateMachine.instance;
            InventoryStateMachine inventory = InventoryStateMachine.instance;
            
            inventory.openBackpack.OnOpenBackpack.AddListener(DisplayToolBelt);
            inventory.closeInventory.OnCloseInventory.AddListener(HideToolBelt);
            InputPacker.OnChangeInputType.AddListener(UpdateSprite);
            
            icon.gameObject.SetActive(false);
            square.gameObject.SetActive(false);
            background.gameObject.SetActive(false);
        }

        private void DisplayToolBelt()
        {
            StopAllCoroutines();
            StartCoroutine(DisplayCoroutine(currentInputType));
        }

        private IEnumerator DisplayCoroutine(InputType inputType)
        {
            icon.sprite = ComputeSprite(inputType);
            if (inputType == InputType.Gamepad)
                StartCoroutine(Tools.Fade(square, 0.2f, true));
            else
                square.gameObject.SetActive(false);
            
            StartCoroutine(Tools.Fade(background, 0.2f, true, 0.8f));
            yield return Tools.Fade(icon, 0.2f, true);
        }
        
        private void HideToolBelt()
        {
            StopAllCoroutines();
            StartCoroutine(HideCoroutine());
        }

        private IEnumerator HideCoroutine()
        {
            if (square.gameObject.activeSelf)
                StartCoroutine(Tools.Fade(square, 0.2f, false));
            
            StartCoroutine(Tools.Fade(background, 0.2f, false, 0.8f));
            yield return Tools.Fade(icon, 0.2f, false);
        }
        
        private void UpdateSprite(InputType inputType)
        {
            if (!player.isBackpackOpen)
                return;
            
            StopAllCoroutines();
            StartCoroutine(UpdateSpriteCoroutine(inputType));
        }

        private IEnumerator UpdateSpriteCoroutine(InputType inputType)
        {
            yield return HideCoroutine();
            yield return DisplayCoroutine(inputType);
        }

        private Sprite ComputeSprite(InputType inputType)
        {
            return inputType == InputType.Gamepad ? gamepadSprite : keyboardSprite;
        }
    }
}
