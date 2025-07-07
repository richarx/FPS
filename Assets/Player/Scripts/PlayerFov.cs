using System;
using System.Collections;
using Pause_Menu;
using UnityEngine;

namespace Player.Scripts
{
    public class PlayerFov : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private float smoothTime;
        
        private PlayerStateMachine player;

        private float currentTarget;
        private float velocity;

        private void Start()
        {
            player = PlayerStateMachine.instance;
            player.playerGun.OnChangeAimState.AddListener((isAiming) => ChangeFov(isAiming ? player.playerData.fovReductionOnAim : 0.0f));
            
            player.playerRun.OnStartSprinting.AddListener(() => ChangeFov(player.playerData.fovReductionOnSprint));
            player.playerRun.OnStopSprinting.AddListener(ResetFov);
            
            player.playerSlide.OnStartSLide.AddListener((fromCrouch) => ChangeFov(player.playerData.fovReductionOnSlide));
            player.playerSlide.OnStopSlide.AddListener((toCrouch) => ResetFov());
            currentTarget = mainCamera.fieldOfView;
        }

        private void ChangeFov(float newValue)
        {
            currentTarget = PauseMenu.instance.currentFov - newValue;
        }
        
        private void ResetFov()
        {
            currentTarget = PauseMenu.instance.currentFov;
        }

        private void Update()
        {
            if (Mathf.Abs(mainCamera.fieldOfView - currentTarget) >= 0.001f)
                mainCamera.fieldOfView = Mathf.SmoothDamp(mainCamera.fieldOfView, currentTarget, ref velocity, smoothTime);
        }
    }
}
