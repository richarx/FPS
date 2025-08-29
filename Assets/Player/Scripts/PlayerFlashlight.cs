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

        public void ToggleFlashlight()
        {
            isTurnedOn = !isTurnedOn;
            
            flashLight.gameObject.SetActive(isTurnedOn);
        }
    }
}
