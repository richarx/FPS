using UnityEngine;
using UnityEngine.Events;

namespace Player.Scripts
{
    public class PlayerLocked : IPlayerBehaviour
    {
        public UnityEvent OnLockPlayer = new UnityEvent();
        public UnityEvent OnUnlockPlayer = new UnityEvent();

        private Transform lookTarget;
        public Vector3 targetPosition => lookTarget.position;
        
        public void StartBehaviour(PlayerStateMachine player, BehaviourType previous)
        {
            player.moveVelocity = Vector3.zero;
            player.ApplyMovement();
            
            OnLockPlayer?.Invoke();
        }

        public void SetLookTarget(Transform newTarget)
        {
            lookTarget = newTarget;
        }

        public void UpdateBehaviour(PlayerStateMachine player)
        {
        }

        public void FixedUpdateBehaviour(PlayerStateMachine player)
        {
        }

        public void StopBehaviour(PlayerStateMachine player, BehaviourType next)
        {          
            OnUnlockPlayer?.Invoke();
        }

        public BehaviourType GetBehaviourType()
        {
            return BehaviourType.Locked;
        }
    }
}
