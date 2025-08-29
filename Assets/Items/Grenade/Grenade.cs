using System.Collections;
using UnityEngine;

namespace Items.Grenade
{
    public class Grenade : MonoBehaviour
    {
        [SerializeField] private float timeBeforeExplosion;
        [SerializeField] private GameObject explosionPrefab;
        [SerializeField] private float heightOffset;

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(timeBeforeExplosion);
            Vector3 position = transform.position;
            position.y += heightOffset;
            Instantiate(explosionPrefab, position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
