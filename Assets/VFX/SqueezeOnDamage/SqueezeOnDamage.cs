using Enemies;
using UnityEngine;

namespace VFX.SqueezeOnDamage
{
    public class SqueezeOnDamage : MonoBehaviour
    {
        private SqueezeAndStretch squeezeAndStretch;
        
        private void Start()
        {
            squeezeAndStretch = GetComponent<SqueezeAndStretch>();
            GetComponent<Damageable>().OnTakeDamage.AddListener(() => squeezeAndStretch.Trigger());
        }
    }
}
