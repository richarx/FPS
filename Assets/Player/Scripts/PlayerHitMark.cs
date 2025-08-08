using System.Collections;
using Data;
using UnityEngine;
using UnityEngine.UI;

namespace Player.Scripts
{
    public class PlayerHitMark : MonoBehaviour
    {
        [SerializeField] private Image hitMark;

        private SqueezeAndStretch squeezeAndStretch;
        
        private VFXData vfxData;
        
        private void Start()
        {
            hitMark.gameObject.SetActive(false);
            squeezeAndStretch = hitMark.GetComponent<SqueezeAndStretch>();
            
            PlayerStateMachine player = PlayerStateMachine.instance;

            vfxData = player.vfxData;
            player.playerShootGun.OnHit.AddListener((_, surfaceData) =>
            {
                if (surfaceData == SurfaceData.SurfaceType.Enemy)
                {
                    StopAllCoroutines();
                    StartCoroutine(TriggerHitMark());
                }
            });
        }

        private IEnumerator TriggerHitMark()
        {
            hitMark.gameObject.SetActive(true);
            hitMark.color = Color.white;
            squeezeAndStretch.Trigger();

            yield return new WaitForSeconds(vfxData.hitMarkDuration);
            yield return Tools.Fade(hitMark, vfxData.hitMarkFadeDuration, false);
        }
    }
}
