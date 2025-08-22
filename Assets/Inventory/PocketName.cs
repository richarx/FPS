using System;
using System.Collections;
using Player.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Inventory.BackpackStorage;

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
            
            text.text = ComputePocketName(Pocket.tools);
            StopAllCoroutines();
            StartCoroutine(Tools.Fade(text, fadeDuration, true));
            StartCoroutine(Tools.Fade(line, fadeDuration, true));
        }

        private void UpdatePocketName(Pocket pocket)
        {
            StopAllCoroutines();
            StartCoroutine(UpdatePocketNameCoroutine(pocket));
        }

        private IEnumerator UpdatePocketNameCoroutine(Pocket pocket)
        {
            yield return Tools.Fade(text, fadeDuration, false);
            text.text = ComputePocketName(pocket);
            yield return Tools.Fade(text, fadeDuration, true);
        }
        
        private void HidePocketName()
        {
            StopAllCoroutines();
            StartCoroutine(Tools.Fade(text, fadeDuration, false));
            StartCoroutine(Tools.Fade(line, fadeDuration, false));
        }

        private string ComputePocketName(Pocket pocket)
        {
            switch (pocket)
            {
                case Pocket.component:
                    return "Components Pocket";
                case Pocket.tools:
                    return "Tools Pocket";
                case Pocket.ammo:
                    return "Ammo Pocket";
                case Pocket.medicine:
                    return "Medicine Pocket";
                default:
                    throw new ArgumentOutOfRangeException(nameof(pocket), pocket, null);
            }
        }
    }
}
