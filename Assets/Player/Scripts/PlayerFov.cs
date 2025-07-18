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
            player.playerGun.OnChangeAimState.AddListener((isAiming) => ChangeFov(GetAimFov(isAiming)));
            
            player.playerRun.OnStartSprinting.AddListener(() => ChangeFov(GetSprintFov()));
            player.playerRun.OnStopSprinting.AddListener(ResetFov);
            
            player.playerSlide.OnStartSlide.AddListener((fromCrouch) => ChangeFov(GetSlideFov()));
            player.playerSlide.OnStopSlide.AddListener((toCrouch) => ResetFov());
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
            return player.playerGun.hasWeapon
                ? player.playerGun.CurrentWeapon.fovReductionOnSprint
                : player.playerData.fovReductionOnSprint;
        }
        
        private float GetSlideFov()
        {
            return player.playerGun.hasWeapon
                ? player.playerGun.CurrentWeapon.fovReductionOnSlide
                : player.playerData.fovReductionOnSlide;
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
