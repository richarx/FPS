using System;
using UnityEngine;

namespace Player.Scripts
{
    public class PlayerRecoil : MonoBehaviour
    {
        [SerializeField] private float snappiness;

        private PlayerLook playerLook;

        private Vector2 currentRecoil;
        private Vector2 targetRecoil;
        private Vector2 velocity;

        private void Start()
        {
            playerLook = GetComponent<PlayerLook>();
        }

        private void Update()
        {
            if (currentRecoil.Distance(targetRecoil) >= 0.01f)
            {
                Vector2 previousRecoil = currentRecoil;
                currentRecoil = Vector2.SmoothDamp(currentRecoil, targetRecoil, ref velocity, snappiness);
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
