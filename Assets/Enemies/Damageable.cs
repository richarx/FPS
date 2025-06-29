using UnityEngine;
using UnityEngine.Events;

namespace Enemies
{
    public class Damageable : MonoBehaviour
    {
        [HideInInspector] public UnityEvent OnTakeDamage = new UnityEvent();

        public void TakeDamage(float damage)
        {
            OnTakeDamage?.Invoke();
        }
    }
}
