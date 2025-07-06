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
            playerData = PlayerStateMachine.instance.playerData;
            
            PlayerStateMachine.instance.playerCrouch.OnStartCrouch.AddListener((fromSlide) =>
            {
                isCrouched = true;
                isSlide = false;
                currentTarget = ComputeTargetPosition();
            });
            PlayerStateMachine.instance.playerSlide.OnStartSLide.AddListener((fromCrouch) =>
            {
                isCrouched = true;
                isSlide = true;
                currentTarget = ComputeTargetPosition();
            });
            PlayerStateMachine.instance.playerCrouch.OnStopCrouch.AddListener((toSlide) =>
            {
                isCrouched = toSlide;
                isSlide = false;
                currentTarget = ComputeTargetPosition();
            });
            PlayerStateMachine.instance.playerSlide.OnStopSlide.AddListener((toCrouch) =>
            {
                isCrouched = toCrouch;
                isSlide = false;
                currentTarget = ComputeTargetPosition();
            });

            currentTarget = ComputeTargetPosition();
        }

        private Vector3 ComputeTargetPosition()
        {
            return new Vector3(0.0f, isCrouched ? playerData.crouchedCameraHeight : playerData.standingCameraHeight, 0.0f);
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
