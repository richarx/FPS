using UnityEngine;
using UnityEngine.Events;

namespace Player.Scripts
{
    public class PlayerAmmo : MonoBehaviour
    {
        [HideInInspector] public UnityEvent OnRefillAmmo = new UnityEvent();
        
        private PlayerStateMachine player;

        private int currentAmmo;
        public int CurrentAmmo => currentAmmo;
        public bool IsEmpty => currentAmmo < 1;

        private void Start()
        {
            player = GetComponent<PlayerStateMachine>();
            player.playerGun.OnEquipWeapon.AddListener(() => currentAmmo = GetMaxAmmo());
            currentAmmo = GetMaxAmmo();
        }

        private int GetMaxAmmo()
        {
            return player.playerGun.hasWeapon ? player.playerGun.CurrentWeapon.startingAmmo : 0;
        }

        public void ConsumeAmmo()
        {
            currentAmmo -= 1;
        }

        public void RefillAmmo()
        {
            currentAmmo = GetMaxAmmo();
            OnRefillAmmo?.Invoke();
        }
    }
}
