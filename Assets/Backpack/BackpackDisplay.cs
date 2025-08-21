using System;
using System.Collections;
using System.Collections.Generic;
using Player.Scripts;
using Tools_and_Scripts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Backpack
{
    public class BackpackDisplay : MonoBehaviour
    {
        [SerializeField] private Transform holder;
        [SerializeField] private Transform backpack;
        [SerializeField] private Transform shadow;
        [SerializeField] private Image blackScreen;
        [SerializeField] private List<Transform> lookTargets;

        public UnityEvent<int> OnSwitchPocketTarget = new UnityEvent<int>(); 

        private Animator backpackAnimator;

        private bool isDisplayed;
        private int previousPocket = 1;
        private int currentPocket = 1;
        
        private void Start()
        {
            backpackAnimator = backpack.GetComponent<Animator>();
            
            PlayerStateMachine player = PlayerStateMachine.instance;
            player.playerBackpack.OnOpenBag.AddListener(() =>
            {
                StopAllCoroutines();
                StartCoroutine(DisplayBackpack());
            });
            player.playerBackpack.OnCloseBag.AddListener(() =>
            {
                StopAllCoroutines();
                StartCoroutine(HideBackpack());
            });

            backpack.gameObject.SetActive(false);
            shadow.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (!isDisplayed)
                return;
            
            if (PlayerInputs.GetMenuUp())
                SwitchPocket(1);
            if (PlayerInputs.GetMenuDown())
                SwitchPocket(2);
            if (PlayerInputs.GetMenuLeft())
                SwitchPocket(3);
            if (PlayerInputs.GetMenuRight())
                SwitchPocket(4);
        }
        
        private void SwitchPocket(int next)
        {
            if (next == currentPocket)
                return;
            
            previousPocket = currentPocket;
            currentPocket = next;

            StopAllCoroutines();
            StartCoroutine(UpdatePocketDisplay());
            
            OnSwitchPocketTarget?.Invoke(currentPocket);
        }

        private IEnumerator UpdatePocketDisplay()
        {
            backpackAnimator.Play($"Close_{previousPocket}");
            yield return new WaitForSeconds(0.16f);
            backpackAnimator.Play($"Open_{currentPocket}");
        }

        public float displayDelay;
        public float displayDuration;
        public float shadowDisplayDelay;
        public float squeezeDelay;
        public float blackScreenMaxFade;
        private IEnumerator DisplayBackpack()
        {
            yield return new WaitForSeconds(displayDelay);
            
            previousPocket = 1;
            currentPocket = 1;
            
            backpack.gameObject.SetActive(true);
            shadow.gameObject.SetActive(true);
            
            backpackAnimator.Play("Closed");

            backpack.localPosition = new Vector3(1.5f, 0.4f, 0.0f);
            shadow.localPosition = new Vector3(0.0f, -1.0f, 0.0f);

            StartCoroutine(Tools.TweenLocalPosition(backpack, 0.0f, 0.0f, displayDuration));
            
            yield return new WaitForSeconds(shadowDisplayDelay);
            StartCoroutine(Tools.TweenLocalPosition(shadow, 0.0f, 0.0f, 0.1f));
            shadow.localScale = Vector3.one * 0.8f;
            StartCoroutine(Tools.TweenLocalScale(shadow, 1.0f, 1.0f, 1.0f, 0.1f));
            
            yield return new WaitForSeconds(squeezeDelay);
            backpack.GetComponent<SqueezeAndStretch>().Trigger();

            StartCoroutine(Tools.Fade(blackScreen, 0.3f, true, blackScreenMaxFade));
            
            holder.SetParent(null);
            backpackAnimator.Play("Open_1");
            isDisplayed = true;
        }

        public float hideDelay;
        private IEnumerator HideBackpack()
        {
            isDisplayed = false;
            holder.SetParent(transform);

            backpackAnimator.Play("Close_1");

            StartCoroutine(Tools.Fade(blackScreen, 0.1f, false, blackScreenMaxFade));
            StartCoroutine(Tools.TweenLocalPosition(shadow, 0.0f, -1.0f, 0.05f, true));
            yield return new WaitForSeconds(hideDelay);
            yield return Tools.TweenLocalPosition(backpack, 1.5f, 0.4f, 0.1f, true);
            holder.localPosition = Vector3.zero;
            holder.localRotation = Quaternion.identity;
        }

        public Transform GetCurrentLookTarget()
        {
            return isDisplayed ? lookTargets[currentPocket - 1] : null;
        }
    }
}
