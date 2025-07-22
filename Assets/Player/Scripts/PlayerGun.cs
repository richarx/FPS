using Items.Weapons;
using Pause_Menu;
using UnityEngine;
using UnityEngine.Events;

namespace Player.Scripts
{
    public class PlayerGun : MonoBehaviour
    {
        [SerializeField] private Transform gunPivot;
        
        [HideInInspector] public UnityEvent OnEquipWeapon = new UnityEvent();
        [HideInInspector] public UnityEvent<GameObject> OnSwapWeapon = new UnityEvent<GameObject>();
        
        [HideInInspector] public bool isEquippingWeapon;
       
        public bool hasWeapon => currentWeapon != null;

        private float startEquipWeaponTimestamp;
        private bool hasWeaponBeenSwapped;

        private WeaponData currentWeapon;
        public WeaponData CurrentWeapon => currentWeapon;

        private void Update()
        {
            if (PauseMenu.instance.IsPaused)
                return;
            
            if (isEquippingWeapon && !hasWeaponBeenSwapped && Time.time - startEquipWeaponTimestamp >= 0.5f)
                SwapWeaponsVisuals();
            
            if (isEquippingWeapon && Time.time - startEquipWeaponTimestamp >= 1.0f)
                isEquippingWeapon = false;
        }

        public void EquipNewWeapon(WeaponData weapon)
        {
            isEquippingWeapon = true;
            startEquipWeaponTimestamp = Time.time;
            hasWeaponBeenSwapped = false;
            currentWeapon = weapon;
            OnEquipWeapon?.Invoke();
        }

        private void SwapWeaponsVisuals()
        {
            if (gunPivot.childCount > 0)
                Destroy(gunPivot.GetChild(0).gameObject);

            Transform newWeapon = Instantiate(currentWeapon.weaponPrefab, Vector3.zero, Quaternion.identity, gunPivot).transform;
            newWeapon.localPosition = new Vector3(0.0f, -600.0f, 0.0f);
            hasWeaponBeenSwapped = true;
            OnSwapWeapon?.Invoke(newWeapon.gameObject);
        }
    }
}
