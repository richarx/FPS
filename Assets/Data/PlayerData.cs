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
        
        [Space]
        [Header("Gun")]
        public float fireRate;
        
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
    }
}
