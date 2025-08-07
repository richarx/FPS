using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Player.Scripts
{
    public class PlayerBloom : MonoBehaviour
    {
        private PlayerStateMachine player;

        private Bloom bloom;

        private bool isSetup;
        
        private void Start()
        {
            player = PlayerStateMachine.instance;
            
            Volume volume = GetComponent<Volume>();
            VolumeProfile volumeProfile = volume.profile;
            isSetup = volumeProfile.TryGet<Bloom>(out bloom);
            
            player.playerScanning.OnStartScanning.AddListener(() => SetBloomValue(5.0f));
            player.playerScanning.OnStopScanning.AddListener(() => SetBloomValue(0.0f));
        }

        private void SetBloomValue(float value)
        {
            bloom.intensity.value = value;
        }
    }
}
