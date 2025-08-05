using Data;
using Items.Weapons;
using Tools_and_Scripts;
using UnityEngine;
using UnityEngine.Events;

namespace Player.Scripts
{
    public class PlayerGun : MonoBehaviour
    {
        [SerializeField] private Transform gunPivot;
        
        [HideInInspector] public UnityEvent<GameObject> OnSwapWeapon = new UnityEvent<GameObject>();

        private PlayerShootGun playerShootGun;
        private PlayerData playerData;
        
        public bool hasWeapon => CurrentWeapon != null;
        public bool hasSecondaryWeapon => secondaryWeapon != null;
        
        public WeaponData CurrentWeapon => primaryWeapon;

        private WeaponData primaryWeapon;
        private WeaponData secondaryWeapon;
        
        private void Start()
        {
            playerShootGun = PlayerStateMachine.instance.playerShootGun;
            playerData = PlayerStateMachine.instance.playerData;
        }

        private bool hasBeenCalledThisFrame;
        private bool hasBeenCalledLastFrame;
        private void Update()
        {
            if (hasSecondaryWeapon && PlayerInputs.GetNorthButton())
                SwapWeapons();

            if (hasWeapon && PlayerInputs.GetDownArrow())
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

        private void DropGun(WeaponData weaponData)
        {
            Vector3 position = playerShootGun.shootingPosition;
            position += Vector3.up * playerData.throwWeaponHeightOffset;
            position += playerShootGun.rightDirection * playerData.throwWeaponSideOffset;

            Rigidbody rb = Instantiate(weaponData.lootPrefab, position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(playerShootGun.shootingDirection * playerData.throwWeaponForce, ForceMode.Impulse);
            
            if (gunPivot.childCount > 0)
                Destroy(gunPivot.GetChild(0).gameObject);
        }

        private void SwapWeapons()
        {
            (secondaryWeapon, primaryWeapon) = (primaryWeapon, secondaryWeapon);
            SwapWeaponsVisuals(primaryWeapon);
        }

        private void SwapWeaponsVisuals(WeaponData weaponData)
        {
            if (gunPivot.childCount > 0)
                Destroy(gunPivot.GetChild(0).gameObject);
            
            Transform newWeapon = Instantiate(weaponData.weaponPrefab, Vector3.zero, Quaternion.identity, gunPivot).transform;
            newWeapon.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            OnSwapWeapon?.Invoke(newWeapon.gameObject);
        }

        public string ComputeTooltipText()
        {
            if (hasWeapon && hasSecondaryWeapon)
                return "Press $E$ to swap weapons";
            return "Press $E$ to equip weapon";
        }
    }
}
