using System;
using Pause_Menu;
using Tools_and_Scripts;
using UnityEngine;
using UnityEngine.Events;

namespace Player.Scripts
{
    public class PlayerAmmo : MonoBehaviour
    {
        [HideInInspector] public UnityEvent OnRefillAmmo = new UnityEvent();
        [HideInInspector] public UnityEvent OnStartReloading = new UnityEvent();
        [HideInInspector] public UnityEvent OnStopReloading = new UnityEvent();
        
        private PlayerStateMachine player;

        private int currentAmmo;
        public int CurrentAmmo => currentAmmo;
        public bool IsEmpty => currentAmmo < 1;
        
        private float startReloadTimestamp;
        [HideInInspector] public bool isReloading;

        private void Start()
        {
            player = GetComponent<PlayerStateMachine>();
            player.playerGun.OnSwapWeapon.AddListener((_) => currentAmmo = GetMaxAmmo());
            player.playerShootGun.OnShootEmptyMag.AddListener(() =>
            {
                if (player.playerGun.CurrentWeapon.isReloadingOnEmptyMag)
                    ReloadGun();
            });
            currentAmmo = GetMaxAmmo();
        }

        private void Update()
        {
            if (PauseMenu.instance.IsPaused)
                return;
            
            if (isReloading && Time.time - startReloadTimestamp >= player.playerGun.CurrentWeapon.reloadDuration)
            {
                isReloading = false;
                RefillAmmo();
                OnStopReloading?.Invoke();
            }
            
            if (isReloading || !player.playerGun.hasWeapon)
                return;
            
            if (PlayerInputs.GetWestButton())
                ReloadGun();
        }
        
        private void ReloadGun()
        {
            isReloading = true;
            startReloadTimestamp = Time.time;
            OnStartReloading?.Invoke();
        }

        private int GetMaxAmmo()
        {
            return player.playerGun.hasWeapon ? player.playerGun.CurrentWeapon.startingAmmo : 0;
        }

        public void ConsumeAmmo()
        {
            currentAmmo -= 1;
        }

        public void RefillAmmo()
        {
            currentAmmo = GetMaxAmmo();
            OnRefillAmmo?.Invoke();
        }
    }
}
