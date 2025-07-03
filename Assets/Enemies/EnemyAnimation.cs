using Player.Scripts;
using UnityEngine;

namespace Enemies
{
    public class EnemyAnimation : MonoBehaviour
    {
        [SerializeField] private Transform orientation;
        [SerializeField] private Animator graphics;
        [SerializeField] private SpriteRenderer spriteRenderer;
    
        private Transform player;
        
        private Tools.MoveDirection currentAnimationDirection = Tools.MoveDirection.Front;
        private string currentAnimation;
    
        private void Start()
        {
            player = PlayerStateMachine.instance.transform;
            currentAnimation = "Run";
        }
    
        private void LateUpdate()
        {
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            directionToPlayer.y = 0.0f;
            directionToPlayer.Normalize();
        
            Vector3 animationDirection = (directionToPlayer.x * orientation.right + directionToPlayer.z * orientation.forward).normalized;
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

        private void PlayAnimation(string targetAnimation)
        {
            Tools.MoveDirection directionIndex = currentAnimationDirection;
            directionIndex = flipDirection(directionIndex);
        
            graphics.Play($"{targetAnimation}_{directionIndex}", 0, graphics.GetCurrentAnimatorStateInfo(0).normalizedTime);
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
    }
}
