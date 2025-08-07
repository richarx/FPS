using System;
using System.Collections;
using Player.Scripts;
using UnityEngine;
using UnityEngine.Events;

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

        public static UnityEvent<Vector3> OnPlayerSphereScan = new UnityEvent<Vector3>();
        [HideInInspector] public UnityEvent OnScannerVisorAppear = new UnityEvent();
        [HideInInspector] public UnityEvent OnScannerVisorDisappear = new UnityEvent();
        [HideInInspector] public UnityEvent OnScannerVisorFullyDisappear = new UnityEvent();

        private float visorDisplayDuration = 0.1f;

        private PlayerStateMachine player;
        
        public const float ScanSphereMaxDistance = 200.0f;
        
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
            OnScannerVisorAppear?.Invoke();
            yield return Tools.TweenScale(center, 6.0f, 6.0f, 1.0f, visorDisplayDuration);
            Vector3 scanPosition = transform.position;
            OnPlayerSphereScan?.Invoke(scanPosition);
            yield return SpawnScanLines(scanPosition);
        }

        public void TriggerNewScan()
        {
            Vector3 scanPosition = transform.position;
            OnPlayerSphereScan?.Invoke(scanPosition);
            StartCoroutine(SpawnScanLines(scanPosition));
        }

        private IEnumerator SpawnScanLines(Vector3 scanPosition)
        {
            for (int i = 0; i < scanLinesCount; i++)
            {
                GameObject scan = Instantiate(scannerTerrainEffect, scanPosition, Quaternion.identity);

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
            OnScannerVisorDisappear?.Invoke();
            yield return Tools.TweenScale(center, 1.0f, 1.0f, 1.0f, visorDisplayDuration);
            OnScannerVisorFullyDisappear?.Invoke();
            StartCoroutine(Tools.TweenPosition(scanner, scanner.position.x, Screen.height * 2.0f, visorDisplayDuration));
            yield return Tools.TweenScale(scanner, 1.0f, 0.4f, 1.0f, visorDisplayDuration);
        }
    }
}
