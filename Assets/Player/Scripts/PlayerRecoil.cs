using System;
using Data;
using UnityEngine;

namespace Player.Scripts
{
    public class PlayerRecoil : MonoBehaviour
    {
        private PlayerLook playerLook;
        private PlayerData playerData;
        
        private Vector2 currentRecoil;
        private Vector2 targetRecoil;
        private Vector2 velocity;

        private void Start()
        {
            playerLook = GetComponent<PlayerLook>();
            playerData = PlayerStateMachine.instance.playerData;
        }

        private void Update()
        {
            if (currentRecoil.Distance(targetRecoil) >= 0.01f)
            {
                Vector2 previousRecoil = currentRecoil;
                currentRecoil = Vector2.SmoothDamp(currentRecoil, targetRecoil, ref velocity, playerData.recoilSnappiness);
                Vector2 deltaRecoil = currentRecoil - previousRecoil;
                playerLook.ApplyKickBack(deltaRecoil.x, deltaRecoil.y);
            }
        }

        public void KickBack(float xKickback, float yKickback)
        {
            currentRecoil.x = playerLook.xRotation;
            currentRecoil.y = playerLook.yRotation;

            targetRecoil = currentRecoil;

            targetRecoil.x += xKickback;
            targetRecoil.y += yKickback;
        }
    }
}
