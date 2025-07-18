using System.Collections.Generic;
using Data;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Player.Scripts
{
    public class PlayerVfx : MonoBehaviour
    {
        [SerializeField] private List<GameObject> secondaryGunImpacts;
        [SerializeField] private GameObject slideSpeedLines;

        private PlayerStateMachine player;
        
        private readonly float distanceTowardsPlayer = 1.3f;
        
        private void Start()
        {
            player = GetComponent<PlayerStateMachine>();
            player.playerGun.OnHit.AddListener(SpawnImpact);
            player.playerSlide.OnStartSlide.AddListener((_) => slideSpeedLines.SetActive(true));
            player.playerSlide.OnStopSlide.AddListener((_) => slideSpeedLines.SetActive(false));
        }

        private void SpawnImpact(Vector3 position, SurfaceData.SurfaceType surfaceType)
        {
            Vector3 directionToPlayer = (player.position - position).normalized;
            position += directionToPlayer * distanceTowardsPlayer;
            
            int randomImpact = Random.Range(0, secondaryGunImpacts.Count);
            Instantiate(secondaryGunImpacts[randomImpact], position + (Random.insideUnitSphere), Quaternion.identity);

            Instantiate(player.surfaceData.GetRandomImpactFromSurface(surfaceType), position, Quaternion.identity);
        }
    }
}
