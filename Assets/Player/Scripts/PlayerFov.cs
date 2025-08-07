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
            player.playerAiming.OnChangeAimState.AddListener((isAiming) =>
            {
                if (player.isScanning)
                    ChangeFov(GetAimFov(isAiming) + player.playerData.fovReductionOnScanner);
                else
                    ChangeFov(GetAimFov(isAiming));
            });
            
            player.playerRun.OnStartSprinting.AddListener(() => ChangeFov(player.playerData.fovReductionOnSprint));
            player.playerRun.OnStopSprinting.AddListener(ResetFov);
            
            player.playerSlide.OnStartSlide.AddListener((fromCrouch) => ChangeFov(player.playerData.fovReductionOnSlide));
            player.playerSlide.OnStopSlide.AddListener((toCrouch) => ResetFov());
            
            player.scanner.OnScannerVisorAppear.AddListener(() =>
            {
                if (player.isAiming)
                    ChangeFov(GetAimFov(true) + player.playerData.fovReductionOnScanner);
                else
                    ChangeFov(player.playerData.fovReductionOnScanner);
            });
            player.scanner.OnScannerVisorDisappear.AddListener(() =>
            {
                if (player.isAiming)
                    ChangeFov(GetAimFov(true));
                else
                    ResetFov();
            });
            
            currentTarget = mainCamera.fieldOfView;
        }

        private float GetAimFov(bool isAiming)
        {
            if (!isAiming)
                return 0.0f;
            
            return player.playerGun.hasWeapon
                ? player.playerGun.CurrentWeapon.fovReductionOnAim
                : player.playerData.fovReductionOnAim;
        }
        
        private float GetSprintFov()
        {
            return player.playerData.fovReductionOnSprint;
        }
        
        private float GetSlideFov()
        {
            return player.playerData.fovReductionOnSlide;
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
