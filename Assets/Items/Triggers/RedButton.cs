using System;
using System.Collections;
using UnityEngine;

namespace Items.Triggers
{
    public class RedButton : MonoBehaviour
    {
        [SerializeField] private Transform redPart;

        private Vector3 startingPosition;
        private Vector3 velocity;
        
        private void Start()
        {
            startingPosition = redPart.localPosition;
            GetComponent<InteractableTrigger>().OnTrigger.AddListener(Activate);
        }

        private void Activate()
        {
            StopAllCoroutines();
            StartCoroutine(ActivateCoroutine());
        }

        private IEnumerator ActivateCoroutine()
        {
            Vector3 targetPosition = startingPosition + (Vector3.down * 1.5f);
            
            float timer = 0.0f;
            while (timer <= 0.15f)
            {
                redPart.localPosition = Vector3.SmoothDamp(redPart.localPosition, targetPosition, ref velocity, 0.05f);
                yield return null;
                timer += Time.deltaTime;
            }

            yield return new WaitForSeconds(0.5f);
            
            timer = 0.0f;
            while (timer <= 0.6f)
            {
                redPart.localPosition = Vector3.SmoothDamp(redPart.localPosition, startingPosition, ref velocity, 0.3f);
                yield return null;
                timer += Time.deltaTime;
            }

            redPart.localPosition = startingPosition;
        }
    }
}
