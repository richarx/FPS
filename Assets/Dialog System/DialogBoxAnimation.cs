using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dialog_System
{
    public class DialogBoxAnimation : MonoBehaviour
    {
        [SerializeField] private RectTransform dialogBox;
        [SerializeField] private TextMeshProUGUI textMeshPro;
        
        [Space]
        [SerializeField, Range(0.0f, 1.0f)] private float boxOpacity;

        [Space]
        [SerializeField, Range(0.0f, 1.0f)] private float boxHeightScreenPercent;
        [SerializeField] private float animationDistance;
        [SerializeField] private float animationDuration;
        
        private Image boxImage;
        private float dialogBoxPosition;
        
        private void Start()
        {
            boxImage = dialogBox.GetComponent<Image>();
            ComputePositions();
        }
        
        private void ComputePositions()
        {
            float halfScreen = Screen.height / 2.0f;
            float downScreen = 0.0f;

            dialogBoxPosition = downScreen + (halfScreen * boxHeightScreenPercent);
        }
        
        public IEnumerator DisplayDialogBox()
        {
            ComputePositions();
            textMeshPro.text = "";
            Tools.SetTextColor(textMeshPro);
            dialogBox.gameObject.SetActive(true);
            StartCoroutine(Tools.Fade(boxImage, animationDuration, true, boxOpacity));
            
            Vector3 position = dialogBox.position;
            position.y = dialogBoxPosition - animationDistance;
            dialogBox.position = position;
            yield return Tools.TweenPosition(dialogBox, dialogBox.position.x, dialogBoxPosition, animationDuration);
        }

        public IEnumerator HideDialogBox()
        {
            ComputePositions();
            StartCoroutine(Tools.Fade(boxImage, animationDuration, false, boxOpacity));
            StartCoroutine(Tools.Fade(textMeshPro, Mathf.Max(0.2f, animationDuration - 0.2f), false));
            yield return Tools.TweenPosition(dialogBox, dialogBox.position.x, dialogBoxPosition - animationDistance, animationDuration);
            
            Vector3 position = dialogBox.position;
            position.y = dialogBoxPosition;
            dialogBox.position = position;
            dialogBox.gameObject.SetActive(false);
        }
    }
}
