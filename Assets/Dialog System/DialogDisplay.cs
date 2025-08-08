using System;
using Febucci.UI.Core;
using Febucci.UI.Core.Parsing;
using Tools_and_Scripts;
using UnityEngine;
using UnityEngine.Events;

namespace Dialog_System
{
    public class DialogDisplay : MonoBehaviour
    {
        [SerializeField] private TypewriterCore typewriter;
        [SerializeField] private GameObject continueIcon;
        
        [HideInInspector] public UnityEvent OnReachDialogEnd = new UnityEvent();

        private string[] dialoguesLines;
        private bool hasLines => dialoguesLines.Length > 0;
        
        int dialogueIndex = 0;
        int dialogueLength;
        bool currentLineShown;
        
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
            typewriter.onMessage.AddListener(OnMessage);
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

        public void InitializeDisplay()
        {
            continueIcon.SetActive(false);
        }

        public void StopDisplay()
        {
            typewriter.StartDisappearingText();
            CurrentLineShown = false;
            dialoguesLines = Array.Empty<string>();
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
