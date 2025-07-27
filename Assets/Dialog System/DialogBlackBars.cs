using UnityEngine;

namespace Dialog_System
{
    public class DialogBlackBars : MonoBehaviour
    {
        [SerializeField] private float barsSize;
        [SerializeField] private float transitionInDuration;
        [SerializeField] private float transitionOutDuration;
        [SerializeField] private RectTransform blackBarUp;
        [SerializeField] private RectTransform blackBarDown;

        private float upperLimit = 1380.0f;
        private float downerLimit = -380.0f;
        
        private void Start()
        {
            DialogManager.OnDisplayDialog.AddListener(ActivateBlackBars);
            DialogManager.OnHideDialog.AddListener(HideBlackBars);
            blackBarUp.position = new Vector3(blackBarUp.position.x, upperLimit, 0.0f);
            blackBarDown.position = new Vector3(blackBarDown.position.x, downerLimit, 0.0f);
        }

        private void ActivateBlackBars()
        {
            StopAllCoroutines();
            StartCoroutine(Tools.TweenPosition(blackBarUp, blackBarUp.position.x, 1080 - barsSize, transitionInDuration));
            StartCoroutine(Tools.TweenPosition(blackBarDown, blackBarDown.position.x, barsSize, transitionInDuration));
        }
        
        private void HideBlackBars()
        {
            StopAllCoroutines();
            StartCoroutine(Tools.TweenPosition(blackBarUp, blackBarUp.position.x, upperLimit, transitionOutDuration, true));
            StartCoroutine(Tools.TweenPosition(blackBarDown, blackBarDown.position.x, downerLimit, transitionOutDuration, true));
        }
    }
}
