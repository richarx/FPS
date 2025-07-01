using UnityEngine;
using UnityEngine.UI;

namespace Player.Scripts
{
    public class PlayerCrosshair : MonoBehaviour
    {
        [SerializeField] private Image crosshair;
        [SerializeField] private float fadeDuration;
        
        private PlayerStateMachine player;

        private bool isAiming;
        
        private void Start()
        {
            player = PlayerStateMachine.instance;
        }

        private void Update()
        {
            if (isAiming != player.isAiming)
            {
                isAiming = !isAiming;
                
                StopAllCoroutines();
                StartCoroutine(Tools.Fade(crosshair, fadeDuration, !isAiming));
            }
        }
    }
}
