using Items;
using Items.Weapons;
using UnityEngine;
using Weapons.Throw;

namespace Player.Scripts
{
    public class PlayerArms : MonoBehaviour
    {
        public enum ArmType
        {
            Empty,
            Weapon,
            Throw
        }
        
        [SerializeField] private Transform armsPivot;
        [SerializeField] private GameObject throwArmsPrefab;

        private Transform currentArms;
        public Transform CurrentArms => currentArms;

        private ArmType armType = ArmType.Empty;
        public ArmType currentArmType => armType;

        public Transform EquipThrowTool(ItemData itemData)
        {
            ClearPivot();
            
            Transform newArms = Instantiate(throwArmsPrefab, Vector3.zero, Quaternion.identity, armsPivot).transform;
            newArms.localPosition = new Vector3(0.0f, 0.0f, 0.0f);

            Transform newTool = newArms.GetComponent<HandHoldItem>().EquipItem(itemData);

            currentArms = newArms;
            armType = ArmType.Throw;

            return newTool;
        }

        public Transform EquipWeapon(WeaponData weaponData)
        {
            ClearPivot();
            
            Transform newWeapon = Instantiate(weaponData.weaponPrefab, Vector3.zero, Quaternion.identity, armsPivot).transform;
            newWeapon.localPosition = new Vector3(0.0f, 0.0f, 0.0f);

            currentArms = newWeapon;
            armType = ArmType.Weapon;

            return newWeapon;
        }
        
        public void ClearPivot()
        {
            for (int i = armsPivot.childCount - 1; i >= 0; i--)
            {
                Destroy(armsPivot.GetChild(i).gameObject);
            }

            armType = ArmType.Empty;
        }
    }
}
