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
            Throw,
            LedgeGrab,
            Loot
        }
        
        [SerializeField] private Transform armsPivot;
        [SerializeField] private GameObject throwArmsPrefab;
        [SerializeField] private GameObject ledgeGrabArmsPrefab;
        [SerializeField] private GameObject lootArmsPrefab;

        [HideInInspector] public UnityEvent OnUnEquipTool = new UnityEvent();
        [HideInInspector] public UnityEvent OnResetArms = new UnityEvent();
        
        private Transform currentArms;
        public Transform CurrentArms => currentArms;

        private ArmType previousArmType = ArmType.Empty;
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
            SetArmType(ArmType.Throw);

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
            SetArmType(ArmType.Weapon);

            return newWeapon;
        }
        
        public void ClearPivot()
        {
            StopAllCoroutines();
            for (int i = armsPivot.childCount - 1; i >= 0; i--)
            {
                Destroy(armsPivot.GetChild(i).gameObject);
            }

            SetArmType(ArmType.Empty);
        }

        public void DisplayLedgeGrabArms()
        {
            StopAllCoroutines();
            ClearPivot();
            
            Transform newArms = Instantiate(ledgeGrabArmsPrefab, Vector3.zero, Quaternion.identity, armsPivot).transform;
            newArms.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            
            currentArms = newArms;
            SetArmType(ArmType.LedgeGrab);
        }

        public void RemoveLedgeGrabArms()
        {
            if (currentArmType != ArmType.LedgeGrab)
                return;
            
            ClearPivot();
            OnResetArms?.Invoke();
        }
        
        public void DisplayLootArms()
        {
            StopAllCoroutines();
            ClearPivot();
            
            Transform newArms = Instantiate(lootArmsPrefab, Vector3.zero, Quaternion.identity, armsPivot).transform;
            newArms.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            
            currentArms = newArms;
            SetArmType(ArmType.Loot);
        }

        public void RemoveLootArms()
        {
            if (currentArmType != ArmType.Loot)
                return;
            
            ClearPivot();
            OnResetArms?.Invoke();
        }

        private void SetArmType(ArmType newType)
        {
            previousArmType = currentArmType;
            armType = newType;
        }
    }
}
