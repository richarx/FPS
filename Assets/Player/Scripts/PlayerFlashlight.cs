using System;
using Tools_and_Scripts;
using UnityEngine;

namespace Player.Scripts
{
    public class PlayerFlashlight : MonoBehaviour
    {
        [SerializeField] private Light flashLight;

        private PlayerStateMachine player;
        
        private bool isTurnedOn;

        private void Start()
        {
            player = PlayerStateMachine.instance;
        }

        private void Update()
        {
            if (!player.isBackpackOpen && PlayerInputs.GetRightArrow())
                ToggleFlashlight();
        }

        private void ToggleFlashlight()
        {
            isTurnedOn = !isTurnedOn;
            
            flashLight.gameObject.SetActive(isTurnedOn);
        }
    }
}
