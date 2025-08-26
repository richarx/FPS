using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Inventory.BackpackStorage;

namespace Backpack
{
    public class BackpackDisplay : MonoBehaviour
    {
        [SerializeField] private Transform holder;
        [SerializeField] private Transform backpack;
        [SerializeField] private Transform shadow;
        [SerializeField] private Image blackScreen;
        public List<Transform> lookTargets;
        
        private Animator backpackAnimator;

        private bool isDisplayed;
        public bool IsDisplayed => isDisplayed;

        private void Start()
        {
            backpackAnimator = backpack.GetComponent<Animator>();
            
            backpack.gameObject.SetActive(false);
            shadow.gameObject.SetActive(false);
        }
        
        public void OpenBackpack()
        {
            StopAllCoroutines();
            StartCoroutine(DisplayBackpack());
        }
        
        public void CloseBackpack()
        {
            StopAllCoroutines();
            StartCoroutine(HideBackpack());
        }
        
        public void SwitchPocket(Pocket current, Pocket next)
        {
            StopAllCoroutines();
            StartCoroutine(UpdatePocketDisplay(current, next));
        }

        private IEnumerator UpdatePocketDisplay(Pocket current, Pocket next)
        {
            backpackAnimator.Play($"Close_{(int)current + 1}");
            yield return new WaitForSeconds(0.16f);
            backpackAnimator.Play($"Open_{(int)next + 1}");
        }

        public float displayDelay;
        public float displayDuration;
        public float shadowDisplayDelay;
        public float squeezeDelay;
        public float blackScreenMaxFade;
        private IEnumerator DisplayBackpack()
        {
            yield return new WaitForSeconds(displayDelay);
            
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
            holder.SetParent(transform);

            backpackAnimator.Play("Close_1");

            StartCoroutine(Tools.Fade(blackScreen, 0.1f, false, blackScreenMaxFade));
            StartCoroutine(Tools.TweenLocalPosition(shadow, 0.0f, -1.0f, 0.05f, true));
            yield return new WaitForSeconds(hideDelay);
            yield return Tools.TweenLocalPosition(backpack, 1.5f, 0.4f, 0.1f, true);
            holder.localPosition = Vector3.zero;
            holder.localRotation = Quaternion.identity;
            isDisplayed = false;
        }
    }
}
