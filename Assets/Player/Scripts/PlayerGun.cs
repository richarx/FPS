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
        [HideInInspector] public UnityEvent<GameObject> OnDropWeapon = new UnityEvent<GameObject>();
        [HideInInspector] public UnityEvent<GameObject> OnEquipAkimboWeapon = new UnityEvent<GameObject>();
        [HideInInspector] public UnityEvent<GameObject> OnDropAkimboWeapon = new UnityEvent<GameObject>();

        private PlayerStateMachine player;
        
        public bool hasWeapon => CurrentWeapon != null;
        public bool hasSecondaryWeapon => secondaryWeapon != null;
        
        public WeaponData CurrentWeapon => primaryWeapon;

        private WeaponData primaryWeapon;
        private WeaponData secondaryWeapon;

        private bool hasAkimbo;
        public bool HasAkimbo => hasAkimbo;
        
        private void Start()
        {
            player = PlayerStateMachine.instance;
        }
        
        private void Update()
        {
            if (player.isBackpackOpen)
                return;
            
            if (hasSecondaryWeapon && PlayerInputs.GetNorthButton())
                SwapWeapons();

            if (hasWeapon && PlayerInputs.GetLeftArrow())
                DropCurrentWeapon();
        }
        
        public void EquipNewWeapon(WeaponData weapon)
        {
            if (hasAkimbo)
                DropAkimboGun();
            
            if (hasWeapon && weapon.weaponPrefab.name == primaryWeapon.weaponPrefab.name)
            {
                hasAkimbo = true;
                DisplayAkimboVisuals(weapon);
                return;
            }
            else if (hasSecondaryWeapon)
                DropGun(primaryWeapon);
            else if (hasWeapon)
                secondaryWeapon = primaryWeapon;

            primaryWeapon = weapon;
            SwapWeaponsVisuals(weapon);
        }
        
        private void DropCurrentWeapon()
        {
            if (hasAkimbo)
            {
                DropAkimboGun();
                return;
            }
            
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
            OnDropWeapon?.Invoke(gunPivot.GetChild(0).gameObject);
            SpawnGunLoot(weaponData);
            ClearGunPivot();
        }

        private void SpawnGunLoot(WeaponData weaponData)
        {
            Vector3 position = player.playerShootGun.shootingPosition;
            position += Vector3.up * player.playerData.throwWeaponHeightOffset;
            position += player.playerShootGun.rightDirection * player.playerData.throwWeaponSideOffset;

            Rigidbody rb = Instantiate(weaponData.lootPrefab, position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(player.playerShootGun.shootingDirection * player.playerData.throwWeaponForce, ForceMode.Impulse);
        }
        
        private void DropAkimboGun()
        {
            SpawnGunLoot(primaryWeapon);
            hasAkimbo = false;

            GameObject gun = gunPivot.GetChild(1).gameObject;
            
            OnDropAkimboWeapon?.Invoke(gun);
            
            if (gunPivot.childCount > 1)
                Destroy(gun);
        }

        private void SwapWeapons()
        {
            if (hasAkimbo)
                DropAkimboGun();
            
            (secondaryWeapon, primaryWeapon) = (primaryWeapon, secondaryWeapon);
            SwapWeaponsVisuals(primaryWeapon);
        }

        private void SwapWeaponsVisuals(WeaponData weaponData)
        {
            ClearGunPivot();
            
            Transform newWeapon = Instantiate(weaponData.weaponPrefab, Vector3.zero, Quaternion.identity, gunPivot).transform;
            newWeapon.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            OnSwapWeapon?.Invoke(newWeapon.gameObject);
        }
        
        private void DisplayAkimboVisuals(WeaponData weaponData)
        {
            Transform newWeapon = Instantiate(weaponData.weaponPrefab, Vector3.zero, Quaternion.identity, gunPivot).transform;
            newWeapon.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            newWeapon.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            newWeapon.GetComponent<AnimateGun>().isAkimbo = true;
            OnEquipAkimboWeapon?.Invoke(newWeapon.gameObject);
        }

        private void ClearGunPivot()
        {
            for (int i = gunPivot.childCount - 1; i >= 0; i--)
            {
                Destroy(gunPivot.GetChild(i).gameObject);
            }
        }

        public string ComputeTooltipText()
        {
            if (hasWeapon && hasSecondaryWeapon)
                return "Press $E$ to swap weapons";
            return "Press $E$ to equip weapon";
        }
    }
}
