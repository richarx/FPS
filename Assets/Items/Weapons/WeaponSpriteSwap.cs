using System;
using Player.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Items.Weapons
{
    public class WeaponSpriteSwap : MonoBehaviour
    {
        [SerializeField] private Image graphics;
        [SerializeField] private Sprite hipSprite;
        [SerializeField] private Sprite aimSprite;
        
        [Space]
        [SerializeField] private float gunAnimationSizeSpeed;
        [SerializeField] private float transitionFalloff = 1200;
        [SerializeField] private Vector2 hipSize;
        [SerializeField] private Vector2 adsSize;
        
        private PlayerStateMachine player;
        
        private RectTransform graphicsTransform;
        
        private Vector2 sizeVelocity;
        
        private void Start()
        {
            player = PlayerStateMachine.instance;
            
            graphicsTransform = graphics.gameObject.GetComponent<RectTransform>();
        }

        private void Update()
        {
            float newSizeX = UpdateSize();
            UpdateSprite(newSizeX);
        }

        private float UpdateSize()
        {
            Vector2 target = player.isAiming ? adsSize : hipSize;
            Vector2 newSize = Vector2.SmoothDamp(graphicsTransform.sizeDelta, target, ref sizeVelocity, gunAnimationSizeSpeed);
            graphicsTransform.sizeDelta = newSize;

            return newSize.x;
        }
        
        private void UpdateSprite(float newSizeX)
        {
            if (graphics == null)
                return;
            
            graphics.sprite = (player.isAiming || newSizeX >= transitionFalloff ? aimSprite : hipSprite);
        }

        private bool IsPlayingShootAnimation()
        {
            return player.playerShootGun.isShooting;
        }
    }
}
