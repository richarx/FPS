using UnityEngine;

namespace Enemies
{
    public class SpawnCorpse : MonoBehaviour
    {
        [SerializeField] private GameObject corpsePrefab;
        [SerializeField] private Damageable damageable;

        public void SpawnCorpseAndDelete()
        {
            Debug.Log("Spawn Corpse !");
            Instantiate(corpsePrefab, transform.position, Quaternion.identity);
            Destroy(damageable.gameObject);
        }
    }
}
