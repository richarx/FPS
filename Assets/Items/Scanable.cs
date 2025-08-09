using System.Collections;
using Player.Scripts;
using UnityEngine;

namespace Items
{
    public class Scanable : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;

        private Material material;
        
        private float currentDistance;
        private float maxDistance;
        private static readonly int OutlineThickness = Shader.PropertyToID("_OutlineThickness");

        private void Start()
        {
            Scanner.Scanner.OnPlayerSphereScan.AddListener(TriggerDisplay);
            PlayerStateMachine.instance.playerScanning.OnStopScanning.AddListener(ResetDisplay);
            material = spriteRenderer.material;
        }

        private void TriggerDisplay(Vector3 scanPosition)
        {
            StopAllCoroutines();
            
            currentDistance = Vector3.Distance(scanPosition, transform.position);
            maxDistance = Scanner.Scanner.ScanSphereMaxDistance;
            
           if (currentDistance < maxDistance)
                StartCoroutine(TriggerDisplayCoroutine());
        }

        private IEnumerator TriggerDisplayCoroutine()
        {
            yield return null;
            
            bool hasBeenTriggered = false;
            
            float sphereDistance = 0.0f;
            while (sphereDistance < maxDistance)
            {
                if (sphereDistance >= currentDistance)
                {
                    hasBeenTriggered = true;
                    break;
                }
                yield return null;
                sphereDistance += 25.0f * Time.deltaTime;
            }

            if (hasBeenTriggered)
                yield return ActivateOutline();
        }

        private IEnumerator ActivateOutline()
        {
            float thickness = 0.0f;
            float velocity = 0.0f;
            
            float timer = 0.0f;
            while (thickness <= 5.0f)
            {
                thickness = Mathf.SmoothDamp(thickness, 5.5f, ref velocity, 0.1f);
                material.SetFloat(OutlineThickness, thickness);
                yield return null;
                timer += Time.deltaTime;
            }
            material.SetFloat(OutlineThickness, 5.0f);

            yield return new WaitForSeconds(1.5f - timer);

            while (thickness > 0.0f)
            {
                thickness = Mathf.SmoothDamp(thickness, -0.5f, ref velocity, 0.5f);
                material.SetFloat(OutlineThickness, thickness);
                yield return null;
            }
            material.SetFloat(OutlineThickness, 0.0f);
        }

        private void ResetDisplay()
        {
            StopAllCoroutines();
            material.SetFloat(OutlineThickness, 0.0f);
        }
    }
}
