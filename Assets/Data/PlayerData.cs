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
        public float gunAnimationDistance;
        public float gunAnimationLateralDistance;
        public float gunAnimationCosSpeed;
        public float gunAnimationSinSpeed;
        public float gunAnimationSmoothTime;
        public float gunAnimationLateralSmoothTime;
        
        [Space]
        [Header("Camera")]
        public float joystickSensitivityX;
        public float joystickSensitivityY;
        
        [Space]
        public float mouseSensitivity;
    }
}
