using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{
    public class SlotDeleteAnimation : MonoBehaviour
    {
        [SerializeField] private Image deletionAnimationImage;

        private bool isFillingUp;
        private bool isComplete;
        
        private float currentFill => deletionAnimationImage.fillAmount;

        private void Update()
        {
            if (isFillingUp)
                FillUp();
            else if (!isComplete && currentFill >= 0.0f)
                FillDown();
        }
        
        private void FillUp()
        {
            deletionAnimationImage.fillAmount += Time.deltaTime;
        }

        private void FillDown()
        {
            deletionAnimationImage.fillAmount -= Time.deltaTime;
        }

        public void StartAnimation()
        {
            StopAllCoroutines();
            
            deletionAnimationImage.gameObject.SetActive(true);
            Tools.SetImageColor(deletionAnimationImage);
            deletionAnimationImage.fillAmount = 0.0f;
            
            isFillingUp = true;
            isComplete = false;
        }

        public void StopAnimation()
        {
            StopAllCoroutines();
            if (currentFill >= 1.0f)
                StartCoroutine(AnimateCompletion());

            isFillingUp = false;
        }

        private IEnumerator AnimateCompletion()
        {
            isComplete = true;

            yield return Tools.Fade(deletionAnimationImage, 0.5f, false);
            
            deletionAnimationImage.fillAmount = 0.0f;
            isComplete = false;
        }
    }
}
