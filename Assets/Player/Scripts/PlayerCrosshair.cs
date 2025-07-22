using UnityEngine;
using UnityEngine.UI;

namespace Player.Scripts
{
    public class PlayerCrosshair : MonoBehaviour
    {
        [SerializeField] private Image crosshair;
        [SerializeField] private float fadeDuration;
        
        private PlayerStateMachine player;
        
        private void Start()
        {
            player = PlayerStateMachine.instance;
            player.playerAiming.OnChangeAimState.AddListener((isAiming) =>
            {
                StopAllCoroutines();
                StartCoroutine(Tools.Fade(crosshair, fadeDuration, !isAiming));
            });
        }
    }
}
