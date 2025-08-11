using System.Collections.Generic;
using Items;
using UnityEngine;

namespace Decor.VendingMachine
{
    public class VendingMachine : MonoBehaviour
    {
        [SerializeField] private Transform spawnPivot;
        [SerializeField] private float spawnForce;
        [SerializeField] private List<GameObject> itemsToSpawn;
        
        private void Start()
        {
            GetComponent<InteractableTrigger>().OnTrigger.AddListener(TriggerSpawnRandomItem);
        }

        private void TriggerSpawnRandomItem()
        {
            int randomIndex = Random.Range(0, itemsToSpawn.Count);
            GameObject itemPrefab = itemsToSpawn[randomIndex];

            GameObject item = Instantiate(itemPrefab, spawnPivot.position, Quaternion.identity);
            item.GetComponent<Rigidbody>().AddForce(spawnPivot.forward * spawnForce + Vector3.up * spawnForce);
        }
    }
}
