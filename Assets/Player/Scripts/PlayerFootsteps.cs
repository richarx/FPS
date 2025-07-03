using System.Collections.Generic;
using UnityEngine;

namespace Player.Scripts
{
    public class PlayerFootsteps : MonoBehaviour
    {
        [SerializeField] private float metersBetweenSteps;
        [SerializeField] private float volume;
        [SerializeField] private List<AudioClip> stepSounds;
        
        private PlayerStateMachine player;

        private float currentMeters;
        private int lastStepSoundPlayed;
        
        private void Start()
        {
            player = GetComponent<PlayerStateMachine>();
            player.playerJump.OnJump.AddListener(PlayStepSound);
        }

        private void LateUpdate()
        {
            if (IsTimeToTakeStep())
                PlayStepSound();
        }

        private void PlayStepSound()
        {
            if (stepSounds.Count < 2)
            {
                SFXManager.instance.PlaySFX(stepSounds[0], volume);
            }

            int randomIndex = Random.Range(0, stepSounds.Count);

            if (randomIndex == lastStepSoundPlayed)
                randomIndex = randomIndex == stepSounds.Count - 1 ? 0 : randomIndex + 1;

            SFXManager.instance.PlaySFX(stepSounds[randomIndex], volume);

            lastStepSoundPlayed = randomIndex;
        }

        private bool IsTimeToTakeStep()
        {
            if (!player.playerJump.isGrounded)
                return false;
                
            Vector3 horizontalVelocity = player.moveVelocity;
            horizontalVelocity.y = 0.0f;

            currentMeters += horizontalVelocity.magnitude * Time.deltaTime;

            if (currentMeters >= metersBetweenSteps)
            {
                currentMeters -= metersBetweenSteps;
                return true;
            }

            return false;
        }
    }
}
