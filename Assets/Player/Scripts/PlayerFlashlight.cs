using Tools_and_Scripts;
using UnityEngine;

namespace Player.Scripts
{
    public class PlayerFlashlight : MonoBehaviour
    {
        [SerializeField] private Light flashLight;

        private bool isTurnedOn;
        
        private void Update()
        {
            if (PlayerInputs.GetRightArrow())
                ToggleFlashlight();
        }

        private void ToggleFlashlight()
        {
            isTurnedOn = !isTurnedOn;
            
            flashLight.gameObject.SetActive(isTurnedOn);
        }
    }
}
