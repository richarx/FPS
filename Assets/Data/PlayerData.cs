using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PlayerData")]
    public class PlayerData : ScriptableObject
    {
        [Header("Movement - Ground")]
        public float groundMaxSpeed;
        public float groundMaxSpeedAiming;
        public float groundAcceleration;
        public float groundDeceleration;
        public float maxSlopeAngle;
        public float steepSlopeFallSpeed;

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
        
        [Space]
        [Header("Gun Animation")]
        public float fovReductionOnAim;

        public float gunAnimationCosSpeed;
        public float gunAnimationSinSpeed;
        
        [Space]
        public float gunAnimationDistance;
        public float gunAnimationSmoothTime;
        
        [Space]
        public float gunAnimationLateralDistance;
        public float gunAnimationLateralSmoothTime;
        
        [Space]
        public float gunAnimationIdleDistance;
        public float gunAnimationIdleSpeed;
        
        [Space]
        public float gunAnimationSizeSpeed;

        [Space]
        [Header("Camera")]
        public float joystickSensitivityX;
        public float joystickSensitivityY;
        
        [Space]
        public float mouseSensitivity;

        [Space]
        [Header("Layers")] 
        public LayerMask layersToIgnoreForGroundCheck;
    }
}
