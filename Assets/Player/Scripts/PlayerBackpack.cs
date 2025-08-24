using Tools_and_Scripts;
using UnityEngine;
using UnityEngine.Events;

namespace Player.Scripts
{
    public class PlayerBackpack : IPlayerBehaviour
    {
        public UnityEvent OnOpenBag = new UnityEvent();
        public UnityEvent OnCloseBag = new UnityEvent();
        
        public void StartBehaviour(PlayerStateMachine player, BehaviourType previous)
        {
            Debug.Log("BACKPACK");
            
            player.moveVelocity = Vector3.zero;
            player.ApplyMovement();
            
            OnOpenBag?.Invoke();
        }

        public void UpdateBehaviour(PlayerStateMachine player)
        {
            if (player.inputPackage.GetBackpack.wasPressedThisFrame)
            {
                player.ChangeBehaviour(player.playerRun);
                return;
            }
        }

        public void FixedUpdateBehaviour(PlayerStateMachine player)
        {
            
        }

        public void StopBehaviour(PlayerStateMachine player, BehaviourType next)
        {
            OnCloseBag?.Invoke();
        }

        public BehaviourType GetBehaviourType()
        {
            return BehaviourType.Backpack;
        }
    }
}
