using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PlayerData")]
    public class PlayerData : ScriptableObject
    {
        [Header("Movement - Ground")]
        public float stickToGroundHeight;
        public float walkMaxSpeed;
        public float sprintMaxSpeed;
        public float groundMaxSpeedAiming;
        public float groundAcceleration;
        public float groundDeceleration;
        public float maxSlopeAngle;
        public float steepSlopeFallSpeed;
        
        [Space] [Header("Slide")] 
        public float slideTransitionSpeed;
        public float slideVelocityThresholdToCrouch;
        public float slidePower;
        public float slideFriction;
        public float slideSteerAcceleration;
        public float slideGravity;

        [Space] [Header("Slide")] 
        public float slamMaxSpeed;
        public float slamAcceleration;
        
        [Space] [Header("Crouch")] 
        public float crouchTransitionSpeed;
        public float crouchMaxSpeed;
        public float crouchMaxSpeedAiming;
        public float crouchAcceleration;
        public float crouchDeceleration;

        [Space] [Header("Jump")] 
        public int maxJumpCount;
        public float groundingForce;
        public float coyoteTime;
        public float jumpForce;
        public float jumpEndEarlyGravityModifier;
        public float fallMaxSpeed;
        public float fallAcceleration;
        public float airMaxSpeed;
        public float airAcceleration;
        public float airDeceleration;
        
        [Space] [Header("FOV Animation")]
        public float fovReductionOnAim;
        public float fovReductionOnSprint;
        public float fovReductionOnSlide;

        [Space] [Header("Camera Height")]
        public float standingCameraHeight;
        public float crouchedCameraHeight;
        public float slideCameraHeight;

        [Space] [Header("Camera Vignette")] 
        public float crouchVignetteIntensity;
        public float standingVignetteIntensity;
        public float vignetteIntensityTransitionDuration;
        
        [Space] [Header("Camera Default Sensitivity")]
        public float joystickSensitivityX;
        public float joystickSensitivityY;
        
        [Space]
        public float mouseSensitivity;
        
        [Space]
        public float aimSensitivityMultiplier;

        [Space] [Header("Layers")] 
        public LayerMask layersToIgnoreForGroundCheck;
    }
}
