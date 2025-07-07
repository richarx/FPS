using System.Collections.Generic;
using SFX;
using UnityEngine;

namespace Player.Scripts
{
    public class PlayerFootsteps : MonoBehaviour
    {
        [SerializeField] private float metersBetweenSteps;
        [SerializeField] private float metersBetweenSlides;
        [SerializeField] private float volume;
        [SerializeField] private List<AudioClip> stepSounds;
        [SerializeField] private List<AudioClip> slideSounds;
        
        private PlayerStateMachine player;

        private float currentMeters;
        private int lastStepSoundPlayed;
        private int lastSlideSoundPlayed;

        private void Start()
        {
            player = GetComponent<PlayerStateMachine>();
            player.playerJump.OnJump.AddListener(() => PlayStepSound(stepSounds, false));
        }

        private void LateUpdate()
        {
            bool isSlide = IsSlide();
            if (IsTimeToTakeStep(isSlide))
                PlayStepSound(SelectSoundList(isSlide), isSlide);
        }

        private List<AudioClip> SelectSoundList(bool isSlide)
        {
            return isSlide ? slideSounds : stepSounds;
        }

        private void PlayStepSound(List<AudioClip> soundList, bool isSlide)
        {
            if (soundList.Count < 2)
            {
                if (soundList.Count > 0)
                    SFXManager.instance.PlaySFX(soundList[0], 0.03f);
                return;
            }

            int previousSoundIndex = GetLastStepIndex(isSlide);

            int randomIndex = Random.Range(0, soundList.Count);

            if (randomIndex == previousSoundIndex)
                randomIndex = randomIndex == soundList.Count - 1 ? 0 : randomIndex + 1;

            SFXManager.instance.PlaySFX(soundList[randomIndex], volume);

            if (isSlide)
                lastSlideSoundPlayed = randomIndex;
            else
                lastStepSoundPlayed = randomIndex;
        }

        private bool IsTimeToTakeStep(bool isSlide)
        {
            if (!player.playerJump.isGrounded)
                return false;
                
            Vector3 horizontalVelocity = player.moveVelocity;
            horizontalVelocity.y = 0.0f;

            currentMeters += horizontalVelocity.magnitude * Time.deltaTime;

            float distance = isSlide ? metersBetweenSlides : metersBetweenSteps;
            
            if (currentMeters >= distance)
            {
                currentMeters -= distance;
                return true;
            }

            return false;
        }

        private bool IsSlide()
        {
            return player.currentBehaviour.GetBehaviourType() == BehaviourType.Slide;
        }

        private int GetLastStepIndex(bool isSlide)
        {
            return isSlide ? lastSlideSoundPlayed : lastStepSoundPlayed;
        }
    }
}
