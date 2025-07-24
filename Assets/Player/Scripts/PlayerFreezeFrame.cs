using System.Collections;
using System.Collections.Generic;
using Data;
using Player.Scripts;
using UnityEngine;

public class PlayerFreezeFrame : MonoBehaviour
{
    private PlayerData playerData;
        
    private bool isSetup;
        
    private void Start()
    {
        PlayerStateMachine player = PlayerStateMachine.instance;

        playerData = player.playerData;
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
        Time.timeScale = playerData.freezeFrameIntensity;
        yield return new WaitForSecondsRealtime(playerData.freezeFrameDuration);
        Time.timeScale = 1.0f;
    }
}
