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

        [Space] [Header("Gun")] 
        public float bulletDistance;
        public int startingAmmo;
        public float reloadDuration;
        public float fireRate;        
        public float xRecoil;
        public float yRecoil;
        public float recoilSnappiness;
        public float recoilCancelSnappiness;
        public float recoilCancelPower;
        
        [Space] [Header("Gun Animation")]
        public float fovReductionOnAim;
        public float fovReductionOnSprint;
        public float fovReductionOnSlide;

        public float gunAnimationCosSpeed;
        public float gunAnimationCosSpeedSprinting;
        public float gunAnimationSinSpeed;
        public float gunAnimationSinSpeedSprinting;
        
        [Space]
        public float gunAnimationDistance;
        public float gunAnimationDistanceSprinting;
        public float gunAnimationSmoothTime;
        
        [Space]
        public float gunAnimationLateralDistance;
        public float gunAnimationLateralSmoothTime;
        
        [Space]
        public float gunAnimationIdleDistance;
        public float gunAnimationIdleSpeed;
        
        [Space]
        public float gunAnimationSizeSpeed;

        [Space] [Header("Camera Height")]
        public float standingCameraHeight;
        public float crouchedCameraHeight;
        public float slideCameraHeight;
        
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
