using System.Collections.Generic;
using SFX;
using UnityEngine;

namespace Player.Scripts
{
    public class PlayerSfx : MonoBehaviour
    {
        [SerializeField] private AudioClip adsInWoosh;
        [SerializeField] private AudioClip adsOutWoosh;
        [SerializeField] private List<AudioClip> jumpWoosh;
        [SerializeField] private List<AudioClip> landingLight;
        [SerializeField] private List<AudioClip> slideStart;
        [SerializeField] private List<AudioClip> ledgeGrab;

        private const float wooshVolume = 0.01f;

        private PlayerStateMachine player;

        private void Start()
        {
            player = GetComponent<PlayerStateMachine>();
            
            player.playerAiming.OnChangeAimState.AddListener((isAiming) =>
            {
                SFXManager.instance.PlaySFX(isAiming ? adsInWoosh : adsOutWoosh, wooshVolume);
            });
            player.playerJump.OnJump.AddListener(() => SFXManager.instance.PlayRandomSFX(jumpWoosh));
            player.playerJump.OnGroundedChanged.AddListener((isGrounded, impactVelocity) =>
            {
                if (isGrounded)
                    SFXManager.instance.PlayRandomSFX(landingLight);
            });
            player.playerSlide.OnStartSlide.AddListener((_) => SFXManager.instance.PlayRandomSFX(slideStart, 0.03f));
            player.playerLedgeGrab.OnLedgeGrab.AddListener(() => SFXManager.instance.PlayRandomSFX(ledgeGrab));
        }
    }
}
