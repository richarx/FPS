using UnityEngine;

namespace Items.Weapons
{
    public class WeaponAmmo : MonoBehaviour
    {
        private int currentAmmo;
        public int CurrentAmmo => currentAmmo;
        public bool IsEmpty => currentAmmo < 1;

        public void ConsumeAmmo()
        {
            currentAmmo -= 1;
        }

        public void RefillAmmo(int ammoCount)
        {
            currentAmmo = ammoCount;
        }
    }
}
