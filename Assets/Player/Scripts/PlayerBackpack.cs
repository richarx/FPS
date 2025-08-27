using Inventory.StateMachine;
using UnityEngine;

namespace Player.Scripts
{
    public class PlayerBackpack : IPlayerBehaviour
    {
        public void StartBehaviour(PlayerStateMachine player, BehaviourType previous)
        {
            Debug.Log("BACKPACK");
            
            player.moveVelocity = Vector3.zero;
            player.ApplyMovement();
            
            InventoryStateMachine.instance.OpenBackpack();
        }

        public void UpdateBehaviour(PlayerStateMachine player)
        {
            
        }

        public void FixedUpdateBehaviour(PlayerStateMachine player)
        {
            
        }

        public void StopBehaviour(PlayerStateMachine player, BehaviourType next)
        {
        }

        public BehaviourType GetBehaviourType()
        {
            return BehaviourType.Backpack;
        }
    }
}
