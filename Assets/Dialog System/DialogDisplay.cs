using System;
using System.Collections;
using Febucci.UI.Core;
using Febucci.UI.Core.Parsing;
using TMPro;
using Tools_and_Scripts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Dialog_System
{
    public class DialogDisplay : MonoBehaviour
    {
        [SerializeField] private TypewriterCore typewriter;
        [SerializeField] private RectTransform dialogBox;
        [SerializeField] private TextMeshProUGUI textMeshPro;
        [SerializeField] private GameObject continueIcon;
        
        [HideInInspector] public UnityEvent OnReachDialogEnd = new UnityEvent();

        private Image boxImage;
        
        private string[] dialoguesLines;
        private bool hasLines => dialoguesLines.Length > 0;
        
        int dialogueIndex = 0;
        int dialogueLength;
        bool currentLineShown;

        private float dialogBoxPosition;

        private void Awake()
        {
            typewriter.onTextShowed.AddListener(() =>
            {
                if (hasLines)
                    CurrentLineShown = true;
            });
        }
    
        private void Start()
        {
            boxImage = dialogBox.GetComponent<Image>();
            textMeshPro = typewriter.GetComponent<TextMeshProUGUI>();
            typewriter.onMessage.AddListener(OnMessage);
            dialogBoxPosition = dialogBox.position.y;
        }

        public void DisplayNewDialog(string[] newDialog)
        {
            dialoguesLines = newDialog;
            
            dialogueIndex = 0;
            dialogueLength = dialoguesLines.Length;
            CurrentLineShown = false;
            typewriter.ShowText(dialoguesLines[dialogueIndex]);
        }

        public void StopDialog()
        {
            typewriter.StopShowingText();
        }
        
        private void Update()
        {
            if (CheckContinueInput() && CurrentLineShown)
                ContinueSequence();
        }

        private bool CheckContinueInput()
        {
            if (PlayerInputs.GetRightTrigger(isHeld: false, withBuffer: false))
                return true;

            if (PlayerInputs.GetSouthButton(isHeld: false, withBuffer: false))
                return true;

            return false;
        }
        
        private bool CurrentLineShown
        {
            get => currentLineShown;
            set
            {
                currentLineShown = value;
                continueIcon.SetActive(value);
            }
        }

        private void ContinueSequence()
        {
            CurrentLineShown = false;
            dialogueIndex += 1;
            
            if (dialogueIndex < dialogueLength)
                typewriter.ShowText(dialoguesLines[dialogueIndex]);
            else
            {
                typewriter.StartDisappearingText();
                OnReachDialogEnd?.Invoke();
            }
        }

        public IEnumerator DisplayDialogBox()
        {
            textMeshPro.text = "";
            Tools.RestoreTextColor(textMeshPro);
            dialogBox.gameObject.SetActive(true);
            continueIcon.SetActive(false);
            StartCoroutine(Tools.Fade(boxImage, 0.5f, true, 0.1f));
            
            Vector3 position = dialogBox.position;
            position.y = dialogBoxPosition - 100.0f;
            dialogBox.position = position;
            yield return Tools.TweenPosition(dialogBox, dialogBox.position.x, dialogBoxPosition, 0.5f);
        }

        public IEnumerator HideDialogBox()
        {
            typewriter.StartDisappearingText();
            CurrentLineShown = false;
            dialoguesLines = Array.Empty<string>();
            
            StartCoroutine(Tools.Fade(boxImage, 0.5f, false, 0.1f));
            StartCoroutine(Tools.Fade(textMeshPro, 0.3f, false));
            yield return Tools.TweenPosition(dialogBox, dialogBox.position.x, dialogBoxPosition - 100.0f, 0.5f);
            
            Vector3 position = dialogBox.position;
            position.y = dialogBoxPosition;
            dialogBox.position = position;
            dialogBox.gameObject.SetActive(false);
        }

        private void OnMessage(EventMarker eventData)
        {
            switch (eventData.name)
            {
                case "face":
                    if (eventData.parameters.Length <= 0)
                    {
                        Debug.LogWarning($"You need to specify a sprite index! Dialogue: {dialogueIndex}");
                        return;
                    }

                    if (TryGetInt(eventData.parameters[0], out int spriteIndex))
                    {
                    }
                    break;
                
                case "crate":
                    break;
            }
        }

        private bool TryGetInt(string parameter, out int result)
        {
            if (FormatUtils.TryGetFloat(parameter, 0, out float resultFloat))
            {
                result = (int)resultFloat;
                return true;
            }

            result = -1;
            return false;
        }
        
        private void OnDestroy()
        {
            if (typewriter) 
                typewriter.onMessage.RemoveListener(OnMessage);
        }
    }
}
