using UnityEngine;
using UnityEngine.Events;

namespace Enemies
{
    public class Damageable : MonoBehaviour
    {
        [SerializeField] private float startingHealth;
        
        [HideInInspector] public UnityEvent<Vector3> OnTakeDamage = new UnityEvent<Vector3>();
        [HideInInspector] public UnityEvent OnDeath = new UnityEvent();

        private float currentHealth = 1;
        
        public bool isDead => currentHealth <= 0;

        private void Start()
        {
            currentHealth = startingHealth;
        }

        public void TakeDamage(float damage, Vector3 hitPosition)
        {
            if (isDead)
                return;
            
            currentHealth -= damage;
            
            OnTakeDamage?.Invoke(hitPosition);
            
            if (isDead)
                OnDeath?.Invoke();
        }
    }
}
