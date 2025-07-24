using System.Collections;
using Data;
using UnityEngine;

namespace Player.Scripts
{
    public class PlayerFreezeFrame : MonoBehaviour
    {
        private VFXData vfxData;
        
        private bool isSetup;
        
        private void Start()
        {
            PlayerStateMachine player = PlayerStateMachine.instance;

            vfxData = player.vfxData;
            player.playerShootGun.OnHit.AddListener((_, surfaceData) =>
            {
                if (isSetup && surfaceData == SurfaceData.SurfaceType.Enemy)
                {
                    StopAllCoroutines();
                    StartCoroutine(TriggerFreezeFrame());
                }
            });
            isSetup = true;
        }

        private IEnumerator TriggerFreezeFrame()
        {
            Time.timeScale = vfxData.freezeFrameIntensity;
            yield return new WaitForSecondsRealtime(vfxData.freezeFrameDuration);
            Time.timeScale = 1.0f;
        }
    }
}
