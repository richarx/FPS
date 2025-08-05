using Items.Weapons;
using Pause_Menu;
using Tools_and_Scripts;
using UnityEngine;
using UnityEngine.Events;

namespace Player.Scripts
{
    public class PlayerAmmo : MonoBehaviour
    {
        [HideInInspector] public UnityEvent<bool, bool> OnRefillAmmo = new UnityEvent<bool, bool>();
        [HideInInspector] public UnityEvent<bool, bool> OnStartReloading = new UnityEvent<bool, bool>();
        [HideInInspector] public UnityEvent<bool, bool> OnStopReloading = new UnityEvent<bool, bool>();
        
        private PlayerStateMachine player;

        private WeaponAmmo mainWeaponAmmo;
        private WeaponAmmo akimboAmmo;
        
        private float startMainReloadTimestamp;
        private float startAkimboReloadTimestamp;

        [HideInInspector] public bool isMainReloading;
        [HideInInspector] public bool isAkimboReloading;
        
        public bool isReloading => isMainReloading || isAkimboReloading;

        private void Start()
        {
            player = GetComponent<PlayerStateMachine>();
            player.playerGun.OnSwapWeapon.AddListener((weapon) => RegisterNewWeapon(weapon, false));
            player.playerGun.OnEquipAkimboWeapon.AddListener((weapon) => RegisterNewWeapon(weapon, true));
            player.playerGun.OnDropWeapon.AddListener((weapon) => mainWeaponAmmo = null);
            player.playerGun.OnDropAkimboWeapon.AddListener((weapon) => akimboAmmo = null);

            player.playerShootGun.OnShoot.AddListener(() =>ConsumeAmmo(false));
            player.playerShootGun.OnShootAkimbo.AddListener(() =>ConsumeAmmo(true));
            player.playerShootGun.OnShootEmptyMag.AddListener(() =>
            {
                if (player.playerGun.CurrentWeapon.isReloadingOnEmptyMag)
                    ReloadGun(true, false);
            });
            player.playerShootGun.OnShootAkimboEmptyMag.AddListener(() =>
            {
                if (player.playerGun.CurrentWeapon.isReloadingOnEmptyMag)
                    ReloadGun(false, true);
            });
        }

        private void RegisterNewWeapon(GameObject weapon, bool isAkimbo)
        {
            if (isAkimbo)
            {
                akimboAmmo = weapon.GetComponent<WeaponAmmo>();
                akimboAmmo.RefillAmmo(GetMaxAmmo());
            }
            else
            {
                mainWeaponAmmo = weapon.GetComponent<WeaponAmmo>();
                mainWeaponAmmo.RefillAmmo(GetMaxAmmo());
            }
        }
        
        public int GetCurrentAmmo(bool isAkimbo)
        {
            return isAkimbo ? akimboAmmo.CurrentAmmo : mainWeaponAmmo.CurrentAmmo;
        }

        public bool IsGunEmpty(bool isAkimbo)
        {
            return isAkimbo ? akimboAmmo.IsEmpty : mainWeaponAmmo.IsEmpty;
        }

        private void Update()
        {
            if (PauseMenu.instance.IsPaused)
                return;
            
            CheckEndOfReload();
            
            if (!CanReload())
                return;
            
            if (PlayerInputs.GetWestButton())
                ReloadGun(player.playerGun.hasWeapon, player.playerGun.HasAkimbo);
        }

        private bool CanReload()
        {
            if (!player.playerGun.hasWeapon)
                return false;

            if (!player.playerGun.HasAkimbo)
                return !isMainReloading;

            return !isMainReloading || !isAkimboReloading;
        }

        private void CheckEndOfReload()
        {
            if (isMainReloading && Time.time - startMainReloadTimestamp >= player.playerGun.CurrentWeapon.reloadDuration)
            {
                isMainReloading = false;
                RefillAmmo(true, false);
                OnStopReloading?.Invoke(true, false);
            }
            
            if (isAkimboReloading && Time.time - startAkimboReloadTimestamp >= player.playerGun.CurrentWeapon.reloadDuration)
            {
                isAkimboReloading = false;
                RefillAmmo(false, true);
                OnStopReloading?.Invoke(false, true);
            }
        }
        
        private void ReloadGun(bool mainGun, bool akimbo)
        {
            if (mainGun)
            {
                startMainReloadTimestamp = Time.time;
                isMainReloading = true;
            }
            if (akimbo)
            {
                startAkimboReloadTimestamp = Time.time;
                isAkimboReloading = true;
            }
            OnStartReloading?.Invoke(mainGun, akimbo);
        }

        private int GetMaxAmmo()
        {
            return player.playerGun.hasWeapon ? player.playerGun.CurrentWeapon.startingAmmo : 0;
        }

        public void ConsumeAmmo(bool isAkimbo)
        {
            if (isAkimbo)
                akimboAmmo.ConsumeAmmo();
            else
                mainWeaponAmmo.ConsumeAmmo();
        }

        public void RefillAmmo(bool mainGun, bool akimbo)
        {
            if (mainGun)
                mainWeaponAmmo.RefillAmmo(GetMaxAmmo());
            if (akimbo)
                akimboAmmo.RefillAmmo(GetMaxAmmo());
            
            OnRefillAmmo?.Invoke(mainGun, akimbo);
        }
    }
}
