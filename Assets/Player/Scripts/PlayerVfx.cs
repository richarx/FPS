using System.Collections.Generic;
using Data;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Player.Scripts
{
    public class PlayerVfx : MonoBehaviour
    {
        [SerializeField] private List<GameObject> secondaryGunImpacts;
        [SerializeField] private List<GameObject> bloodSplats;
        [SerializeField] private float distanceTowardsPlayer;

        private PlayerStateMachine player;
        
        private void Start()
        {
            player = GetComponent<PlayerStateMachine>();
            player.playerGun.OnHit.AddListener(SpawnImpact);
        }

        private void SpawnImpact(Vector3 position, SurfaceData.SurfaceType surfaceType)
        {
            Vector3 directionToPlayer = (player.position - position).normalized;
            position += directionToPlayer * distanceTowardsPlayer;
            
            int randomImpact = Random.Range(0, secondaryGunImpacts.Count);
            Instantiate(secondaryGunImpacts[randomImpact], position + (Random.insideUnitSphere), Quaternion.identity);

            if (surfaceType == SurfaceData.SurfaceType.Enemy)
            {
                int randomBlood = Random.Range(0, bloodSplats.Count);
                Instantiate(bloodSplats[randomBlood], position + (Random.insideUnitSphere * 0.5f), Quaternion.identity);
            }
            
            Instantiate(player.surfaceData.GetRandomImpactFromSurface(surfaceType), position, Quaternion.identity);
        }
    }
}
