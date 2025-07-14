using System;
using Tools_and_Scripts;
using UnityEngine;

namespace Player.Scripts
{
    public class PlayerFlashlight : MonoBehaviour
    {
        [SerializeField] private Light light;

        private bool isTurnedOn;
        
        private void Update()
        {
            if (PlayerInputs.GetUpArrow())
                ToggleFlashlight();
        }

        private void ToggleFlashlight()
        {
            isTurnedOn = !isTurnedOn;
            
            light.gameObject.SetActive(isTurnedOn);
        }
    }
}
