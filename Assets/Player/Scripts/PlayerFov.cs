using Pause_Menu;
using UnityEngine;

namespace Player.Scripts
{
    public class PlayerFov : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private float fovReduction;
        [SerializeField] private float smoothTime;
        
        private PlayerStateMachine player;

        private float velocity;

        private void Start()
        {
            player = PlayerStateMachine.instance;
        }

        private void LateUpdate()
        {
            float currentFov = PauseMenu.instance.currentFov;
            float target = player.isAiming ? currentFov - fovReduction : currentFov;
            mainCamera.fieldOfView = Mathf.SmoothDamp(mainCamera.fieldOfView, target, ref velocity, smoothTime);
        }
    }
}
