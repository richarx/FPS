using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PlayerData")]
    public class PlayerData : ScriptableObject
    {
        [Header("Movement - Ground")]
        public float groundMaxSpeed;
        public float groundAcceleration;
        public float groundDeceleration;

        [Space] [Header("Jump")] 
        public int maxJumpCount;
        public float groundingForce;
        public float coyoteTime;
        public float jumpForce;
        public float jumpEndEarlyGravityModifier;
        public float fallMaxSpeed;
        public float fallAcceleration;
        public float airDeceleration;

        [Space]
        [Header("Gun")]
        public float fireRate;        
        public float xRecoil;
        public float yRecoil;
        public float recoilSnappiness;
        
        [Space]
        [Header("Gun Animation")]
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
