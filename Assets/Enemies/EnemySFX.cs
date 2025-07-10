using System.Collections.Generic;
using SFX;
using UnityEngine;

namespace Enemies
{
    public class EnemySFX : MonoBehaviour
    {
        [SerializeField] private List<AudioClip> onHit;
        [SerializeField] private List<AudioClip> onDeath;
        
        private void Start()
        {
            Damageable damageable = GetComponent<Damageable>();
            
            damageable.OnTakeDamage.AddListener((position) => SFXManager.instance.PlayRandomSFXInChannel("Hit", 1.5f, onHit, null));
            damageable.OnDeath.AddListener(() => SFXManager.instance.PlayRandomSFXInChannel("Death", 0.5f, onDeath, null));
        }
    }
}
