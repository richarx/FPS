using UnityEngine;

namespace Enemies
{
    public class SpawnCorpse : MonoBehaviour
    {
        [SerializeField] private GameObject corpsePrefab;
        [SerializeField] private float heightOffset;

        private void Start()
        {
            GetComponent<Damageable>().OnDeath.AddListener(SpawnCorpseAndDelete);
        }

        public void SpawnCorpseAndDelete()
        {
            Vector3 position = transform.position + (Vector3.up * heightOffset);
            
            Instantiate(corpsePrefab, position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
