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

        private void Start()
        {
            playerData = PlayerStateMachine.instance.playerData;
            
            PlayerStateMachine.instance.playerCrouch.OnStartCrouch.AddListener(() => currentTarget = ComputeTargetPosition(true));
            PlayerStateMachine.instance.playerCrouch.OnStopCrouch.AddListener(() => currentTarget = ComputeTargetPosition(false));

            currentTarget = ComputeTargetPosition(false);
        }

        private Vector3 ComputeTargetPosition(bool isCrouched)
        {
            return new Vector3(0.0f, isCrouched ? playerData.crouchedCameraHeight : playerData.standingCameraHeight, 0.0f);
        }

        private void Update()
        {
            if (Vector3.Distance(cameraPivot.localPosition, currentTarget) >= 0.001f)
            {
                cameraPivot.localPosition = Vector3.SmoothDamp(cameraPivot.localPosition, currentTarget, ref velocity, playerData.crouchTransitionSpeed);
            }
        }
    }
}
