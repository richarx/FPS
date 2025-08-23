using Items.Weapons;
using Tools_and_Scripts;
using UnityEngine;
using UnityEngine.Events;

namespace Player.Scripts
{
    public class PlayerGun : MonoBehaviour
    {
        [HideInInspector] public UnityEvent<GameObject> OnSwapWeapon = new UnityEvent<GameObject>();
        [HideInInspector] public UnityEvent OnDropWeapon = new UnityEvent();

        private PlayerStateMachine player;
        
        public bool hasWeapon => CurrentWeapon != null;
        public bool hasSecondaryWeapon => secondaryWeapon != null;
        
        public WeaponData CurrentWeapon => primaryWeapon;

        private WeaponData primaryWeapon;
        private WeaponData secondaryWeapon;

        private void Start()
        {
            player = PlayerStateMachine.instance;
            
            player.playerArms.OnResetArms.AddListener(() =>
            {
                if (hasWeapon)
                    SwapWeaponsVisuals(primaryWeapon);
            });
        }
        
        private void Update()
        {
            if (player.isBackpackOpen)
                return;

            if (hasWeapon && player.playerArms.currentArmType == PlayerArms.ArmType.Throw && PlayerInputs.GetNorthButton())
                SwapWeaponsVisuals(primaryWeapon);
            
            if (hasSecondaryWeapon && PlayerInputs.GetNorthButton())
                SwapWeapons();

            if (hasWeapon && PlayerInputs.GetLeftArrow())
                DropCurrentWeapon();
        }
        
        public void EquipNewWeapon(WeaponData weapon)
        {
            if (hasSecondaryWeapon)
                DropGun(primaryWeapon);
            else if (hasWeapon)
                secondaryWeapon = primaryWeapon;

            primaryWeapon = weapon;
            SwapWeaponsVisuals(weapon);
        }
        
        private void SwapWeapons()
        {
            (secondaryWeapon, primaryWeapon) = (primaryWeapon, secondaryWeapon);
            SwapWeaponsVisuals(primaryWeapon);
        }

        private void SwapWeaponsVisuals(WeaponData weaponData)
        {
            GameObject newWeapon = player.playerArms.EquipWeapon(weaponData).gameObject;
            OnSwapWeapon?.Invoke(newWeapon);
        }
        
        private void DropCurrentWeapon()
        {
            DropGun(primaryWeapon);
            if (hasSecondaryWeapon)
            {
                primaryWeapon = secondaryWeapon;
                secondaryWeapon = null;
                SwapWeaponsVisuals(primaryWeapon);
            }
            else
                primaryWeapon = null;
        }

        private void DropGun(WeaponData weaponData)
        {
            OnDropWeapon?.Invoke();
            SpawnGunLoot(weaponData);
            
            if (player.playerArms.currentArmType == PlayerArms.ArmType.Weapon)
                player.playerArms.ClearPivot();
        }

        private void SpawnGunLoot(WeaponData weaponData)
        {
            Vector3 position = player.playerShootGun.shootingPosition;
            position += Vector3.up * player.playerData.throwWeaponHeightOffset;
            position += player.playerShootGun.rightDirection * player.playerData.throwWeaponSideOffset;

            Rigidbody rb = Instantiate(weaponData.lootPrefab, position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(player.playerShootGun.shootingDirection * player.playerData.throwWeaponForce, ForceMode.Impulse);
        }

        public string ComputeTooltipText()
        {
            if (hasWeapon && hasSecondaryWeapon)
                return "Press $E$ to swap weapons";
            return "Press $E$ to equip weapon";
        }
    }
}
