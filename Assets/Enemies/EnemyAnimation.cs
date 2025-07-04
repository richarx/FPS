using System;
using Player.Scripts;
using UnityEngine;

namespace Enemies
{
    public class EnemyAnimation : MonoBehaviour
    {
        [SerializeField] private Transform orientation;
        [SerializeField] private Animator graphics;
        [SerializeField] private SpriteRenderer spriteRenderer;

        private Damageable damageable;
        private Transform player;
        
        private Tools.MoveDirection currentAnimationDirection = Tools.MoveDirection.Front;
        private string currentAnimation;

        private Vector3 animationDirection;

        private void Start()
        {
            damageable = GetComponent<Damageable>();
            damageable.OnDeath.AddListener(OnDeath);
            player = PlayerStateMachine.instance.transform;
            currentAnimation = "Run";
        }

        private void OnDeath()
        {
            PlayAnimation("Death", false);
        }
    
        private void LateUpdate()
        {
            if (damageable.isDead)
                return;
            
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            directionToPlayer.y = 0.0f;
            directionToPlayer.Normalize();
        
            animationDirection = (directionToPlayer.x * orientation.right + directionToPlayer.z * orientation.forward).normalized;
            Vector2 direction = new Vector2(animationDirection.x, animationDirection.z);
            //Vector2 direction = new Vector2(directionToPlayer.x, directionToPlayer.z);
        
            Tools.MoveDirection directionIndex = (Tools.MoveDirection)Tools.GetCardinalDirection(direction);

            Debug.Log($"Direction : {directionIndex}");
        
            if (currentAnimationDirection != directionIndex)
            {
                currentAnimationDirection = directionIndex;
                PlayAnimation(currentAnimation);
            }
        }

        private void PlayAnimation(string targetAnimation, bool useDirection = true, bool useSmoothTransition = true)
        {
            string animationToPlay = "";
            float startTime = 0.0f;

            if (useDirection)
            {
                Tools.MoveDirection directionIndex = currentAnimationDirection;
                directionIndex = flipDirection(directionIndex);
                animationToPlay = $"{targetAnimation}_{directionIndex}";

                startTime = graphics.GetCurrentAnimatorStateInfo(0).normalizedTime;
            }
            else
            {
                animationToPlay = targetAnimation;
            }
            
            
            graphics.Play(animationToPlay, 0, startTime);
        }

        private Tools.MoveDirection flipDirection(Tools.MoveDirection directionIndex)
        {
            bool isFlipped = false;
        
            if (directionIndex == Tools.MoveDirection.Right)
            {
                isFlipped = true;
                directionIndex = Tools.MoveDirection.Left;
            }
        
            if (directionIndex == Tools.MoveDirection.RightFront)
            {
                isFlipped = true;
                directionIndex = Tools.MoveDirection.LeftFront;
            }
        
            if (directionIndex == Tools.MoveDirection.RightBack)
            {
                isFlipped = true;
                directionIndex = Tools.MoveDirection.LeftBack;
            }
        
            SetSpriteDirection(isFlipped);
        
            return directionIndex;
        }

        private void SetSpriteDirection(bool isFlipped)
        {
            spriteRenderer.flipX = isFlipped;
        }

        private void OnDrawGizmos()
        {
            Vector3 position = transform.position;

            Gizmos.color = Color.green;
            Gizmos.DrawLine(position, position + orientation.forward * 8.0f);
            
            Gizmos.color = Color.red;
            Gizmos.DrawLine(position, position + animationDirection * 5.0f);
        }
    }
}
