using Items.Weapons;
using Pause_Menu;
using Tools_and_Scripts;
using UnityEngine;
using UnityEngine.Events;

namespace Player.Scripts
{
    public class PlayerAmmo : MonoBehaviour
    {
        [HideInInspector] public UnityEvent<bool> OnRefillAmmo = new UnityEvent<bool>();
        [HideInInspector] public UnityEvent OnStartReloading = new UnityEvent();
        [HideInInspector] public UnityEvent<bool> OnStopReloading = new UnityEvent<bool>();
        
        private PlayerStateMachine player;

        private WeaponAmmo weaponAmmo;
        
        private float startReloadTimestamp;

        public bool isReloading;

        private void Start()
        {
            player = GetComponent<PlayerStateMachine>();
            player.playerGun.OnSwapWeapon.AddListener(RegisterNewWeapon);
            player.playerGun.OnDropWeapon.AddListener(() => weaponAmmo = null);

            player.playerShootGun.OnShoot.AddListener(ConsumeAmmo);
            player.playerShootGun.OnShootEmptyMag.AddListener(() =>
            {
                if (player.playerGun.CurrentWeapon.isReloadingOnEmptyMag)
                    ReloadGun();
            });
        }

        private void RegisterNewWeapon(GameObject weapon)
        {
            weaponAmmo = weapon.GetComponent<WeaponAmmo>();
            weaponAmmo.RefillAmmo(GetMaxAmmo());
        }
        
        public int GetCurrentAmmo()
        {
            return weaponAmmo.CurrentAmmo;
        }

        public bool IsGunEmpty()
        {
            return weaponAmmo.IsEmpty;
        }

        private void Update()
        {
            if (PauseMenu.instance.IsPaused)
                return;
            
            CheckEndOfReload();
            
            if (!CanReload())
                return;
            
            if (player.playerGun.hasWeapon && player.inputPackage.GetReload.WasPressedWithBuffer())
                ReloadGun();
        }

        private bool CanReload()
        {
            if (!player.playerGun.hasWeapon || player.playerArms.currentArmType != PlayerArms.ArmType.Weapon)
                return false;
            return !isReloading;
        }

        private void CheckEndOfReload()
        {
            if (isReloading && Time.time - startReloadTimestamp >= player.playerGun.CurrentWeapon.reloadDuration)
            {
                isReloading = false;
                RefillAmmo(true);
                OnStopReloading?.Invoke(true);
            }
        }
        
        private void ReloadGun()
        {
            startReloadTimestamp = Time.time;
            isReloading = true;
            
            OnStartReloading?.Invoke();
        }

        private int GetMaxAmmo()
        {
            return player.playerGun.hasWeapon ? player.playerGun.CurrentWeapon.startingAmmo : 0;
        }

        public void ConsumeAmmo()
        {
            weaponAmmo.ConsumeAmmo();
        }

        public void RefillAmmo(bool mainGun)
        {
            if (mainGun)
                weaponAmmo.RefillAmmo(GetMaxAmmo());
            
            OnRefillAmmo?.Invoke(mainGun);
        }
    }
}
