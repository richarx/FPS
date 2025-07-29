using Dialog_System;
using UnityEngine;
using UnityEngine.UI;

namespace Player.Scripts
{
    public class PlayerCrosshair : MonoBehaviour
    {
        [SerializeField] private Image crosshair;
        [SerializeField] private float fadeDuration;

        private bool isDisplayed = true;
        
        private PlayerStateMachine player;
        
        private void Start()
        {
            player = PlayerStateMachine.instance;
            player.playerAiming.OnChangeAimState.AddListener((isAiming) => SetCrosshairState(!isAiming && !DialogManager.instance.IsDialogDisplayed));
            DialogManager.OnDisplayDialog.AddListener((_) => SetCrosshairState(false));
            DialogManager.OnHideDialog.AddListener(() => SetCrosshairState(!player.isAiming));
        }

        private void SetCrosshairState(bool state)
        {
            if (state == isDisplayed)
                return;
            
            isDisplayed = state;
            StopAllCoroutines();
            StartCoroutine(Tools.Fade(crosshair, fadeDuration, isDisplayed));
        }
    }
}
