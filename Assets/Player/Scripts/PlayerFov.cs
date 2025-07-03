using System.Collections;
using Pause_Menu;
using UnityEngine;

namespace Player.Scripts
{
    public class PlayerFov : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private float smoothTime;
        
        private PlayerStateMachine player;

        private float velocity;

        private void Start()
        {
            player = PlayerStateMachine.instance;
            player.playerGun.OnChangeAimState.AddListener((isAiming) =>
            {
                StopAllCoroutines();
                StartCoroutine(UpdateFov(isAiming));
            });
        }

        private IEnumerator UpdateFov(bool isAiming)
        {
            float currentFov = PauseMenu.instance.currentFov;
            float target = isAiming ? currentFov - player.playerData.fovReductionOnAim : currentFov;

            while (Mathf.Abs(mainCamera.fieldOfView - target) >= 0.1f)
            {
                mainCamera.fieldOfView = Mathf.SmoothDamp(mainCamera.fieldOfView, target, ref velocity, smoothTime);
                yield return null;
            }

            mainCamera.fieldOfView = target;
        }
    }
}
