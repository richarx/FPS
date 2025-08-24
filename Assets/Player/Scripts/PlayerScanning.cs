using Tools_and_Scripts;
using UnityEngine;
using UnityEngine.Events;

namespace Player.Scripts
{
    public class PlayerScanning : IPlayerBehaviour
    {
        public UnityEvent OnStartScanning = new UnityEvent();
        public UnityEvent OnStopScanning = new UnityEvent();

        public void StartBehaviour(PlayerStateMachine player, BehaviourType previous)
        {
            Debug.Log("SCANNING");
            OnStartScanning?.Invoke();
        }

        public void UpdateBehaviour(PlayerStateMachine player)
        {
            if (player.inputPackage.GetToolUp.wasPressedThisFrame)
            {
                player.ChangeBehaviour(player.playerRun);
                return;
            }

            if (player.inputPackage.GetToolDown.wasPressedThisFrame)
            {
                player.scanner.TriggerNewScan();
            }
        }

        public void FixedUpdateBehaviour(PlayerStateMachine player)
        {
            player.playerRun.FixedUpdateBehaviour(player);
        }

        public void StopBehaviour(PlayerStateMachine player, BehaviourType next)
        {
            OnStopScanning?.Invoke();
        }

        public BehaviourType GetBehaviourType()
        {
            return BehaviourType.Scanning;
        }
    }
}
