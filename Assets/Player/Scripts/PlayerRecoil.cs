using System;
using Data;
using UnityEngine;

namespace Player.Scripts
{
    public class PlayerRecoil : MonoBehaviour
    {
        private PlayerLook playerLook;
        private PlayerGun playerGun;
        private PlayerData playerData;
        
        private Vector2 currentRecoil;
        private Vector2 targetRecoil;
        private Vector2 velocity;

        private float startingHeight;
        private bool isReturning;

        private void Start()
        {
            playerLook = GetComponent<PlayerLook>();
            playerGun = PlayerStateMachine.instance.playerGun;
            playerData = PlayerStateMachine.instance.playerData;
        }

        private void Update()
        {
            if (currentRecoil.Distance(targetRecoil) >= 0.001f)
            {
                Vector2 previousRecoil = currentRecoil;
                currentRecoil = Vector2.SmoothDamp(currentRecoil, targetRecoil, ref velocity, isReturning ? playerGun.CurrentWeapon.recoilCancelSnappiness : playerGun.CurrentWeapon.recoilSnappiness);
                
                Vector2 deltaRecoil = currentRecoil - previousRecoil;
                playerLook.ApplyKickBack(deltaRecoil.x, deltaRecoil.y);
            }
            else if (!playerGun.isShooting && !isReturning)
            {
                targetRecoil.y += (startingHeight - targetRecoil.y) * playerGun.CurrentWeapon.recoilCancelPower;
                startingHeight = 0.0f;
                isReturning = true;
            }
        }

        public void KickBack(float xKickback, float yKickback)
        {
            currentRecoil.x = playerLook.yRotation;
            currentRecoil.y = -playerLook.xRotation;

            if (isReturning)
            {
                startingHeight = currentRecoil.y;
                isReturning = false;
            }
            
            targetRecoil = currentRecoil;

            targetRecoil.x += xKickback;
            targetRecoil.y += yKickback;
        }
    }
}
