using System;
using System.Collections;
using Player.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{
    public class PocketDisplay : MonoBehaviour
    {
        public enum Pocket
        {
            tools,
            component,
            ammo,
            medicine
        }
        
        [SerializeField] private Transform componentPocket;
        [SerializeField] private Transform toolsPocket;
        [SerializeField] private Transform ammoPocket;
        [SerializeField] private Transform medicinePocket;
        [SerializeField] private float fadeDuration;
        [SerializeField] private float displayDelay;

        private Transform currentPocket;
        
        private void Start()
        {
            PlayerStateMachine player = PlayerStateMachine.instance;
            
            player.playerBackpack.OnOpenBag.AddListener(DisplayPocket);
            player.backpackDisplay.OnSwitchPocketTarget.AddListener(UpdatePocket);
            player.playerBackpack.OnCloseBag.AddListener(HidePocketName);
            
            componentPocket.gameObject.SetActive(false);
            toolsPocket.gameObject.SetActive(false);
            ammoPocket.gameObject.SetActive(false);
            medicinePocket.gameObject.SetActive(false);
        }

        private void DisplayPocket()
        {
            StopAllCoroutines();
            StartCoroutine(DisplayPocketCoroutine());
        }

        private IEnumerator DisplayPocketCoroutine()
        {
            currentPocket = ComputePocket(Pocket.tools);

            yield return new WaitForSeconds(displayDelay);
            
            currentPocket.gameObject.SetActive(true);
            FadePocket(currentPocket, true);
        }

        private void UpdatePocket(Pocket pocket)
        {
            StopAllCoroutines();
            StartCoroutine(UpdatePocketCoroutine(pocket));
        }

        private IEnumerator UpdatePocketCoroutine(Pocket pocket)
        {
            FadePocket(currentPocket, false);
            yield return new WaitForSeconds(fadeDuration);
            
            currentPocket.gameObject.SetActive(false);
            currentPocket = ComputePocket(pocket);
            currentPocket.gameObject.SetActive(true);
            
            FadePocket(currentPocket, true);
        }
        
        private void HidePocketName()
        {
            StopAllCoroutines();
            FadePocket(currentPocket, false);
        }

        private void FadePocket(Transform pocket, bool fade)
        {
            for (int i = 0; i < pocket.childCount; i++)
            {
                StartCoroutine(Tools.Fade(pocket.GetChild(i).GetComponent<Image>(), fadeDuration, fade, maxFade: i == 0 ? 0.2f : 0.8f));
            }    
        }
        
        private Transform ComputePocket(Pocket pocket)
        {
            switch (pocket)
            {
                case Pocket.component:
                    return componentPocket;
                case Pocket.tools:
                    return toolsPocket;
                case Pocket.ammo:
                    return ammoPocket;
                case Pocket.medicine:
                    return medicinePocket;
                default:
                    throw new ArgumentOutOfRangeException(nameof(pocket), pocket, null);
            }
        }
    }
}
