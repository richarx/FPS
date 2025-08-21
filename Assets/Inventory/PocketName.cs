using System.Collections;
using Player.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{
    public class PocketName : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Image line;
        [SerializeField] private float fadeDuration;

        private void Start()
        {
            PlayerStateMachine player = PlayerStateMachine.instance;
            
            player.playerBackpack.OnOpenBag.AddListener(DisplayPocketName);
            player.backpackDisplay.OnSwitchPocketTarget.AddListener(UpdatePocketName);
            player.playerBackpack.OnCloseBag.AddListener(HidePocketName);
            
            text.gameObject.SetActive(false);
            line.gameObject.SetActive(false);
        }

        private void DisplayPocketName()
        {
            text.gameObject.SetActive(true);
            line.gameObject.SetActive(true);
            
            text.text = ComputePocketName(1);
            StopAllCoroutines();
            StartCoroutine(Tools.Fade(text, fadeDuration, true));
            StartCoroutine(Tools.Fade(line, fadeDuration, true));
        }

        private void UpdatePocketName(int pocketIndex)
        {
            StopAllCoroutines();
            StartCoroutine(UpdatePocketNameCoroutine(pocketIndex));
        }

        private IEnumerator UpdatePocketNameCoroutine(int pocketIndex)
        {
            yield return Tools.Fade(text, fadeDuration, false);
            text.text = ComputePocketName(pocketIndex);
            yield return Tools.Fade(text, fadeDuration, true);
        }
        
        private void HidePocketName()
        {
            StopAllCoroutines();
            StartCoroutine(Tools.Fade(text, fadeDuration, false));
            StartCoroutine(Tools.Fade(line, fadeDuration, false));
        }

        private string ComputePocketName(int pocketIndex)
        {
            if (pocketIndex == 1)
                return "Components Pocket";
            if (pocketIndex == 2)
                return "Tools Pocket";
            if (pocketIndex == 3)
                return "Ammo Pocket";
            if (pocketIndex == 4)
                return "Medicine Pocket";

            return $"Error wrong index : {pocketIndex}";
        }
    }
}
