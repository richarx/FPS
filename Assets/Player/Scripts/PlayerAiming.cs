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
        public bool IsAiming => isAiming;

        private PlayerStateMachine player;
        private PlayerGun playerGun;

        private void Start()
        {
            player = PlayerStateMachine.instance;
            playerGun = GetComponent<PlayerGun>();
        }

        private void Update()
        {
            if (PauseMenu.instance.IsPaused)
                return;
            
            if (isAiming != PlayerInputs.GetLeftTrigger(isHeld: true))
            {
                isAiming = !isAiming;
                OnChangeAimState?.Invoke(isAiming);
            }
        }
    }
}
