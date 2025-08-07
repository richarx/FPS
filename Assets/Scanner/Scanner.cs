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
        [SerializeField] private GameObject scannerTerrainEffect;
        [SerializeField] private int scanLinesCount;
        [SerializeField] private float delayBetweenScanLines;
        
        private float visorDisplayDuration = 0.1f;

        private PlayerStateMachine player;
        
        private void Start()
        {
            player = PlayerStateMachine.instance;
            player.playerScanning.OnStartScanning.AddListener(() =>
            {
                StopAllCoroutines();
                StartCoroutine(TriggerDisplayAnimation());
            });
            player.playerScanning.OnStopScanning.AddListener(() =>
            {
                StopAllCoroutines();
                StartCoroutine(TriggerHideAnimation());
            });

            scanner.position = new Vector3(scanner.position.x, Screen.height * 2.0f, 0.0f);
        }

        private IEnumerator TriggerDisplayAnimation()
        {
            StartCoroutine(Tools.TweenPosition(scanner, scanner.position.x, Screen.height, visorDisplayDuration));
            yield return Tools.TweenScale(scanner, 1.0f, 1.0f, 1.0f, visorDisplayDuration);
            yield return Tools.TweenScale(center, 6.0f, 6.0f, 1.0f, visorDisplayDuration);
            yield return SpawnScanLines();
        }

        private IEnumerator SpawnScanLines()
        {
            for (int i = 0; i < scanLinesCount; i++)
            {
                GameObject scan = Instantiate(scannerTerrainEffect, transform.position, Quaternion.identity);

                player.playerScanning.OnStopScanning.AddListener(() =>
                {
                    if (scan != null)
                        Destroy(scan);
                });
                
                yield return new WaitForSeconds(delayBetweenScanLines);
            }
        }
        
        private IEnumerator TriggerHideAnimation()
        {
            yield return Tools.TweenScale(center, 1.0f, 1.0f, 1.0f, visorDisplayDuration);
            StartCoroutine(Tools.TweenPosition(scanner, scanner.position.x, Screen.height * 2.0f, visorDisplayDuration));
            yield return Tools.TweenScale(scanner, 1.0f, 0.4f, 1.0f, visorDisplayDuration);
        }
    }
}
