using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "VFXData", menuName = "ScriptableObjects/VFXData")]
    public class VFXData : ScriptableObject
    {
        [Space] [Header("Hit Mark")]
        public float hitMarkDuration;
        public float hitMarkFadeDuration;
        
        [Space] [Header("Freeze Frame")]
        public float freezeFrameDuration;
        [Range(0.0f, 1.0f)]
        public float freezeFrameIntensity;
        
        [Space] [Header("Chromatic Aberration")]
        public float chromaticAberrationFadeDuration;
        public float chromaticAberrationDuration;
        [Range(0.0f, 1.0f)]
        public float chromaticAberrationIntensity;
        
        [Space] [Header("Camera Vignette")] 
        public float crouchVignetteIntensity;
        public float standingVignetteIntensity;
        public float vignetteIntensityTransitionDuration;
    }
}
