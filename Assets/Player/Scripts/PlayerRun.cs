using UnityEngine;

namespace Player.Scripts
{
    public class PlayerRun : IPlayerBehaviour
    {
        public void StartBehaviour(PlayerStateMachine player, BehaviourType previous)
        {
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
            return BehaviourType.Run;
        }
    }
}
