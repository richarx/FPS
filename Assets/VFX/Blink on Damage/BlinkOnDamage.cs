using System.Collections;
using Enemies;
using UnityEngine;

namespace VFX.Blink_on_Damage
{
    public class BlinkOnDamage : MonoBehaviour
    {
        [SerializeField] private float duration;
        [SerializeField] private Material blinkMaterial;

        private SpriteRenderer spriteRenderer;
        private Material baseMaterial;
        
        private Coroutine blinkCoroutine = null;
        
        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            baseMaterial = spriteRenderer.material;
            GetComponent<Damageable>().OnTakeDamage.AddListener(Blink);
        }

        private void Blink()
        {
            if (blinkCoroutine == null)
                blinkCoroutine = StartCoroutine(BlinkRoutine());
        }

        private IEnumerator BlinkRoutine()
        {
            spriteRenderer.material = blinkMaterial;
            yield return new WaitForSeconds(duration);
            spriteRenderer.material = baseMaterial;
            blinkCoroutine = null;
        }
    }
}
