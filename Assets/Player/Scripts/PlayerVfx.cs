using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Player.Scripts
{
    public class PlayerVfx : MonoBehaviour
    {
        [SerializeField] private GameObject mainGunImpact;
        [SerializeField] private List<GameObject> secondaryGunImpacts;
        [SerializeField] private List<GameObject> bloodSplats;

        private PlayerStateMachine player;
        
        private void Start()
        {
            player = GetComponent<PlayerStateMachine>();
            player.playerGun.OnHit.AddListener(SpawnImpact);
        }

        private void SpawnImpact(Vector3 position)
        {
            int randomImpact = Random.Range(0, secondaryGunImpacts.Count);
            int randomBlood = Random.Range(0, bloodSplats.Count);
            Instantiate(secondaryGunImpacts[randomImpact], position + (Random.insideUnitSphere * 0.5f), Quaternion.identity);
            Instantiate(bloodSplats[randomBlood], position + (Random.insideUnitSphere * 0.5f), Quaternion.identity);
            //Instantiate(mainGunImpact, position, Quaternion.identity);
        }
    }
}
