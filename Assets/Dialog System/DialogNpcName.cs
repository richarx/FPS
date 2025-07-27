using System;
using System.Collections;
using System.ComponentModel;
using TMPro;
using UnityEngine;

namespace Dialog_System
{
    public class DialogNpcName : MonoBehaviour
    {
        [SerializeField] private float transitionDelay;
        [SerializeField] private float transitionInDuration;
        [SerializeField] private float transitionOutDuration;
        [SerializeField] private float textHeight;
        [SerializeField] private TextMeshProUGUI text;

        private RectTransform textTransform;

        private void Start()
        {
            textTransform = text.GetComponent<RectTransform>();
            DialogManager.OnDisplayDialog.AddListener(() =>
            {
                StopAllCoroutines();
                StartCoroutine(DisplayName());
            });
            DialogManager.OnHideDialog.AddListener(HideName);
        }

        private IEnumerator DisplayName()
        {
            yield return new WaitForSeconds(transitionDelay);
            text.text = DialogManager.instance.npcName;
            yield return Tools.TweenPosition(textTransform, textTransform.position.x, textHeight, transitionInDuration);
        }
        
        private void HideName()
        {
            StopAllCoroutines();
            StartCoroutine(Tools.TweenPosition(textTransform, textTransform.position.x, -500.0f, transitionOutDuration, true));
        }
    }
}
