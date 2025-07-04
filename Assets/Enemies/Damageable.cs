using UnityEngine;
using UnityEngine.Events;

namespace Enemies
{
    public class Damageable : MonoBehaviour
    {
        [SerializeField] private float startingHealth;
        
        [HideInInspector] public UnityEvent OnTakeDamage = new UnityEvent();
        [HideInInspector] public UnityEvent OnDeath = new UnityEvent();

        private float currentHealth = 1;
        
        public bool isDead => currentHealth <= 0;

        private void Start()
        {
            currentHealth = startingHealth;
        }

        public void TakeDamage(float damage)
        {
            if (isDead)
                return;
            
            currentHealth -= damage;
            
            OnTakeDamage?.Invoke();
            
            if (isDead)
                OnDeath?.Invoke();
        }
    }
}
