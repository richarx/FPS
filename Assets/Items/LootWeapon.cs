using Items.Weapons;
using UnityEngine;

namespace Items
{
    public class LootWeapon : MonoBehaviour
    {
        [SerializeField] private WeaponData weaponData;

        public WeaponData GetWeaponData()
        {
            return weaponData;
        }
    }
}
