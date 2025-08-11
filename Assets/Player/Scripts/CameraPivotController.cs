using Data;
using UnityEngine;

namespace Player.Scripts
{
    public class CameraPivotController : MonoBehaviour
    {
        [SerializeField] private Transform cameraPivot;
        
        private PlayerData playerData;
        
        private Vector3 currentTarget;
        private Vector3 velocity;

        private bool isCrouched;
        private bool isSlide;
        
        private void Start()
        {
            PlayerStateMachine player = PlayerStateMachine.instance;
            playerData = player.playerData;
            
            player.playerBackpack.OnOpenBag.AddListener(() =>
            {
                isCrouched = true;
                isSlide = false;
                currentTarget = ComputeTargetPosition();
            });
            player.playerBackpack.OnCloseBag.AddListener(() =>
            {
                isCrouched = false;
                isSlide = false;
                currentTarget = ComputeTargetPosition();
            });
            
            player.playerCrouch.OnStartCrouch.AddListener((fromSlide) =>
            {
                isCrouched = true;
                isSlide = false;
                currentTarget = ComputeTargetPosition();
            });
            player.playerSlide.OnStartSlide.AddListener((fromCrouch) =>
            {
                isCrouched = true;
                isSlide = true;
                currentTarget = ComputeTargetPosition();
            });
            
            player.playerCrouch.OnStopCrouch.AddListener((toSlide) =>
            {
                isCrouched = toSlide;
                isSlide = false;
                currentTarget = ComputeTargetPosition();
            });
            player.playerSlide.OnStopSlide.AddListener((toCrouch) =>
            {
                isCrouched = toCrouch;
                isSlide = false;
                currentTarget = ComputeTargetPosition();
            });

            currentTarget = ComputeTargetPosition();
        }

        private Vector3 ComputeTargetPosition()
        {
            float height = playerData.standingCameraHeight;

            if (isSlide)
                height = playerData.slideCameraHeight;
            else if (isCrouched)
                height = playerData.crouchedCameraHeight;
            
            return new Vector3(0.0f, height, 0.0f);
        }

        private float ComputeTransitionSpeed()
        {
            return isSlide ? playerData.slideTransitionSpeed : playerData.crouchTransitionSpeed;
        }

        private void Update()
        {
            if (Vector3.Distance(cameraPivot.localPosition, currentTarget) >= 0.001f)
            {
                cameraPivot.localPosition = Vector3.SmoothDamp(cameraPivot.localPosition, currentTarget, ref velocity, ComputeTransitionSpeed());
            }
        }
    }
}
