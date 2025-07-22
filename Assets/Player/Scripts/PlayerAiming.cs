using System;
using Pause_Menu;
using Tools_and_Scripts;
using UnityEngine;
using UnityEngine.Events;

namespace Player.Scripts
{
    public class PlayerAiming : MonoBehaviour
    {
        [HideInInspector] public UnityEvent<bool> OnChangeAimState = new UnityEvent<bool>();

        [HideInInspector] public bool isAiming;
        
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
