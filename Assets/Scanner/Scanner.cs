using System.Collections;
using Player.Scripts;
using UnityEngine;

namespace Scanner
{
    public class Scanner : MonoBehaviour
    {
        [SerializeField] private RectTransform scanner;
        [SerializeField] private RectTransform center;
        
        [Space]
        [SerializeField] private float displayDuration;

        [Space]
        [SerializeField] private float hideDuration;
        
        private float startingHeight;
        
        private void Start()
        {
            PlayerStateMachine.instance.playerScanning.OnStartScanning.AddListener(() =>
            {
                StopAllCoroutines();
                StartCoroutine(TriggerDisplayAnimation());
            });
            PlayerStateMachine.instance.playerScanning.OnStopScanning.AddListener(() =>
            {
                StopAllCoroutines();
                StartCoroutine(TriggerHideAnimation());
            });

            scanner.position = new Vector3(scanner.position.x, Screen.height * 2.0f, 0.0f);
        }

        private IEnumerator TriggerDisplayAnimation()
        {
            StartCoroutine(Tools.TweenPosition(scanner, scanner.position.x, Screen.height, displayDuration));
            yield return Tools.TweenScale(scanner, 1.0f, 1.0f, 1.0f, displayDuration);
            StartCoroutine(Tools.TweenScale(center, 6.0f, 6.0f, 1.0f, displayDuration));
        }
        
        private IEnumerator TriggerHideAnimation()
        {
            yield return Tools.TweenScale(center, 1.0f, 1.0f, 1.0f, displayDuration);
            StartCoroutine(Tools.TweenPosition(scanner, scanner.position.x, Screen.height * 2.0f, hideDuration));
            StartCoroutine(Tools.TweenScale(scanner, 1.0f, 0.4f, 1.0f, displayDuration));
        }
    }
}
