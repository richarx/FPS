using System.Collections;
using Items;
using Items.Weapons;
using UnityEngine;
using UnityEngine.Events;
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

        [HideInInspector] public UnityEvent OnUnEquipTool = new UnityEvent();
        [HideInInspector] public UnityEvent OnResetArms = new UnityEvent();
        
        private Transform currentArms;
        public Transform CurrentArms => currentArms;

        private ArmType armType = ArmType.Empty;
        public ArmType currentArmType => armType;

        public Transform EquipThrowTool(ItemData itemData)
        {
            StopAllCoroutines();
            ClearPivot();
            
            Transform newArms = Instantiate(throwArmsPrefab, Vector3.zero, Quaternion.identity, armsPivot).transform;
            newArms.localPosition = new Vector3(0.0f, 0.0f, 0.0f);

            Transform newTool = newArms.GetComponent<HandHoldItem>().EquipItem(itemData);

            currentArms = newArms;
            armType = ArmType.Throw;

            return newTool;
        }
        
        public void UnEquipThrowTool()
        {
            if (currentArmType != ArmType.Throw)
                return;
            
            StopAllCoroutines();
            StartCoroutine(SwapFromToolToWeaponCoroutine());
        }

        private IEnumerator SwapFromToolToWeaponCoroutine()
        {
            OnUnEquipTool?.Invoke();
            yield return new WaitForSeconds(0.35f);
            ClearPivot();
            OnResetArms?.Invoke();
        }

        public Transform EquipWeapon(WeaponData weaponData)
        {
            StopAllCoroutines();
            ClearPivot();
            
            Transform newWeapon = Instantiate(weaponData.weaponPrefab, Vector3.zero, Quaternion.identity, armsPivot).transform;
            newWeapon.localPosition = new Vector3(0.0f, 0.0f, 0.0f);

            currentArms = newWeapon;
            armType = ArmType.Weapon;

            return newWeapon;
        }
        
        public void ClearPivot()
        {
            StopAllCoroutines();
            for (int i = armsPivot.childCount - 1; i >= 0; i--)
            {
                Destroy(armsPivot.GetChild(i).gameObject);
            }

            armType = ArmType.Empty;
        }
    }
}
