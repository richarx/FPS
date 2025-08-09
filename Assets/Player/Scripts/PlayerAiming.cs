using Pause_Menu;
using Tools_and_Scripts;
using UnityEngine;
using UnityEngine.Events;

namespace Player.Scripts
{
    public class PlayerAiming : MonoBehaviour
    {
        [HideInInspector] public UnityEvent<bool> OnChangeAimState = new UnityEvent<bool>();

        private bool isAiming;
        public bool IsAiming => isAiming && !playerGun.HasAkimbo;

        private PlayerStateMachine player;
        private PlayerGun playerGun;

        private void Start()
        {
            player = PlayerStateMachine.instance;
            playerGun = GetComponent<PlayerGun>();
            playerGun.OnEquipAkimboWeapon.AddListener((_) =>
            {
                if (isAiming)
                {
                    isAiming = false;
                    OnChangeAimState?.Invoke(isAiming);
                }
            });
        }

        private void Update()
        {
            if (PauseMenu.instance.IsPaused)
                return;
            
            if (playerGun.HasAkimbo && !player.isScanning)
                return;
            
            if (isAiming != PlayerInputs.GetLeftTrigger(isHeld: true))
            {
                isAiming = !isAiming;
                OnChangeAimState?.Invoke(isAiming);
            }
        }
    }
}
