using Data;
using Items.Weapons;
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
        
        public bool hasWeapon => currentWeapon != null;

        private WeaponData currentWeapon;
        public WeaponData CurrentWeapon => currentWeapon;

        private void Start()
        {
            playerShootGun = PlayerStateMachine.instance.playerShootGun;
            playerData = PlayerStateMachine.instance.playerData;
        }

        public void EquipNewWeapon(WeaponData weapon)
        {
            if (currentWeapon != null)
                DropGun(currentWeapon);
            
            currentWeapon = weapon;
            SwapWeaponsVisuals(weapon);
        }

        private void DropGun(WeaponData weaponData)
        {
            Vector3 position = playerShootGun.shootingPosition;
            position += Vector3.up * playerData.throwWeaponHeightOffset;
            position += playerShootGun.rightDirection * playerData.throwWeaponSideOffset;

            Rigidbody rb = Instantiate(weaponData.lootPrefab, position, Quaternion.identity).GetComponent<Rigidbody>();
            
            rb.AddForce(playerShootGun.shootingDirection * playerData.throwWeaponForce, ForceMode.Impulse);
        }

        private void SwapWeaponsVisuals(WeaponData weaponData)
        {
            if (gunPivot.childCount > 0)
                Destroy(gunPivot.GetChild(0).gameObject);

            Transform newWeapon = Instantiate(weaponData.weaponPrefab, Vector3.zero, Quaternion.identity, gunPivot).transform;
            newWeapon.localPosition = new Vector3(0.0f, -600.0f, 0.0f);
            OnSwapWeapon?.Invoke(newWeapon.gameObject);
        }
    }
}
