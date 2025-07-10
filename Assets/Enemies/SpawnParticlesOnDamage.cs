using System.Collections.Generic;
using Player.Scripts;
using UnityEngine;

namespace Enemies
{
    public class SpawnParticlesOnDamage : MonoBehaviour
    {
        [SerializeField] private List<GameObject> particlePrefabs;

        private PlayerStateMachine player;
        
        private readonly float distanceTowardsPlayer = 1.3f;
        
        private void Start()
        {
            player = PlayerStateMachine.instance;
            GetComponent<Damageable>().OnTakeDamage.AddListener(SpawnParticles);
        }

        private void SpawnParticles(Vector3 position)
        {
            Vector3 directionToPlayer = (player.position - position).normalized;
            position += directionToPlayer * distanceTowardsPlayer;
            
            int randomIndex = Random.Range(0, particlePrefabs.Count);
            Instantiate(particlePrefabs[randomIndex], position + (Random.insideUnitSphere * 0.5f), Quaternion.identity);
        }
    }
}
