using Player.Scripts;
using UnityEngine;

namespace Items.Weapons
{
    public class WeaponTilt : MonoBehaviour
    {
        [SerializeField] private float tiltRotationAmount;
        [SerializeField] private float tiltSmoothTime;
        [SerializeField] private float tiltSnapBackTime;
        [SerializeField] private float minTilt;
        [SerializeField] private float maxTilt;
        
        private PlayerStateMachine player;
        
        private RectTransform rootTransform;
        private Quaternion initialRotation;
        
        private void Start()
        {
            player = PlayerStateMachine.instance;
            
            initialRotation = Quaternion.identity;
            rootTransform = GetComponent<RectTransform>();
            rootTransform.localPosition = Vector3.zero;
        }

        private void Update()
        {
            UpdateTilt();
        }

        private void UpdateTilt()
        {
            float tilt = 0.0f;
            float time = tiltSnapBackTime;

            if (player.isAiming)
            {
                float input = player.moveInput.x;

                tilt = Mathf.Clamp(input * tiltRotationAmount, minTilt, maxTilt);
             
                if (Mathf.Abs(input) >= 0.15f)
                    time = tiltSmoothTime;
            }
            
            Quaternion finalRotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, tilt));

            rootTransform.localRotation = Quaternion.Slerp(rootTransform.localRotation, finalRotation * initialRotation, time * Time.deltaTime);
        }
    }
}
