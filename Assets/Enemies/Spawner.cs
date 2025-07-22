using System.Collections.Generic;
using Items.Triggers;
using UnityEngine;

namespace Enemies
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private Trigger trigger;
        [SerializeField] private Damageable enemyPrefab;
        [SerializeField] private List<Transform> spawnPositions;
        [SerializeField] private AnimationCurve difficulty;
        [SerializeField] private float fightDuration;

        private int currentEnemyCount = 0;
        private float startSpawningTimestamp;
        
        private bool isSpawning;
        
        private void Start()
        {
            trigger.OnTrigger.AddListener(() =>
            {
                if (isSpawning)
                    StopSpawning();
                else
                    StartSpawning();
            });
        }

        private void StartSpawning()
        {
            startSpawningTimestamp = Time.time;
            isSpawning = true;
        }

        private void StopSpawning()
        {
            isSpawning = false;
        }

        private void Update()
        {
            if (isSpawning)
                TrySpawning();
        }

        private void TrySpawning()
        {
            int targetEnemyCount = ComputeTargetEnemyCount();

            if (currentEnemyCount < targetEnemyCount)
                SpawnEnemy();
        }

        private int ComputeTargetEnemyCount()
        {
            return Mathf.RoundToInt(difficulty.Evaluate(Time.time - startSpawningTimestamp) * fightDuration);
        }

        private void SpawnEnemy()
        {
            Vector3 spawnPosition = ComputeSpawnPosition();

            Damageable enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

            currentEnemyCount += 1;
            enemy.OnDeath.AddListener(() => currentEnemyCount -= 1);
        }

        private Vector3 ComputeSpawnPosition()
        {
            int index = Random.Range(0, spawnPositions.Count);
            Vector3 position = spawnPositions[index].position + Random.insideUnitSphere * 5.0f;
            position.y = 0.0f;

            return position;
        }
    }
}
