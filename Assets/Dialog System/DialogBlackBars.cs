using UnityEngine;

namespace Dialog_System
{
    public class DialogBlackBars : MonoBehaviour
    {
        [SerializeField] [Range(0.0f, 1.0f)] private float barsScreenPercent;
        [SerializeField] private float transitionInDuration;
        [SerializeField] private float transitionOutDuration;
        [SerializeField] private RectTransform blackBarUp;
        [SerializeField] private RectTransform blackBarDown;

        private float upPosition;
        private float upperLimit;

        private float downPosition;
        private float downerLimit;
        
        private void Start()
        {
            DialogManager.OnDisplayDialog.AddListener(ActivateBlackBars);
            DialogManager.OnHideDialog.AddListener(HideBlackBars);

            ComputePositions();
            blackBarUp.position = new Vector3(blackBarUp.position.x, upperLimit, 0.0f);
            blackBarDown.position = new Vector3(blackBarDown.position.x, downerLimit, 0.0f);
        }

        private void ComputePositions()
        {
            float halfScreen = Screen.height / 2.0f;
            float upScreen = Screen.height;
            float downScreen = 0.0f;

            upPosition = upScreen - (halfScreen * barsScreenPercent);
            upperLimit = upScreen + 150.0f;

            downPosition = downScreen + (halfScreen * barsScreenPercent);
            downerLimit = downScreen - 150.0f;
        }

        private void ActivateBlackBars()
        {
            ComputePositions();
            StopAllCoroutines();
            StartCoroutine(Tools.TweenPosition(blackBarUp, blackBarUp.position.x, upPosition, transitionInDuration));
            StartCoroutine(Tools.TweenPosition(blackBarDown, blackBarDown.position.x, downPosition, transitionInDuration));
        }
        
        private void HideBlackBars()
        {
            ComputePositions();
            StopAllCoroutines();
            StartCoroutine(Tools.TweenPosition(blackBarUp, blackBarUp.position.x, upperLimit, transitionOutDuration, true));
            StartCoroutine(Tools.TweenPosition(blackBarDown, blackBarDown.position.x, downerLimit, transitionOutDuration, true));
        }
    }
}
