using System;
using System.Collections;
using Player.Scripts;
using UnityEngine;

namespace Items.Weapons
{
    public class WeaponShootScaleAnimation : MonoBehaviour
    {
        [SerializeField] private RectTransform graphics;
        
        [SerializeField] private float duration;

        [SerializeField] private AnimationCurve hipCurveX;
        [SerializeField] private AnimationCurve hipCurveY;
        [SerializeField] private AnimationCurve aimCurveX;
        [SerializeField] private AnimationCurve aimCurveY;

        private PlayerStateMachine player;
        
        private void Start()
        {
            player = PlayerStateMachine.instance;
            
            if (GetComponent<AnimateGun>().isAkimbo)
                player.playerShootGun.OnShootAkimbo.AddListener(TriggerScaling);
            else
                player.playerShootGun.OnShoot.AddListener(TriggerScaling);
        }

        private void TriggerScaling()
        {
            StopAllCoroutines();
            StartCoroutine(TriggerScalingCoroutine());
        }

        private IEnumerator TriggerScalingCoroutine()
        {
            Vector3 sizeDelta = Vector3.one;

            float timer = 0.0f;
            while (timer <= duration)
            {
                sizeDelta.x = SampleCurveAtTime(player.isAiming ? aimCurveX : hipCurveX, timer);
                sizeDelta.y = SampleCurveAtTime(player.isAiming ? aimCurveY : hipCurveY, timer);
                graphics.localScale = sizeDelta;
                yield return null;
                timer += Time.deltaTime;
            }

            graphics.localScale = Vector3.one;
        }

        private float SampleCurveAtTime(AnimationCurve curve, float time)
        {
            return curve.Evaluate(Tools.NormalizeValue(time, 0.0f, duration));
        }
    }
}
