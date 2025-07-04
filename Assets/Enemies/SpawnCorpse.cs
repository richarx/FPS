using UnityEngine;

namespace Enemies
{
    public class SpawnCorpse : MonoBehaviour
    {
        [SerializeField] private GameObject corpsePrefab;

        private void Start()
        {
            GetComponent<Damageable>().OnDeath.AddListener(SpawnCorpseAndDelete);
        }

        public void SpawnCorpseAndDelete()
        {
            Instantiate(corpsePrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
