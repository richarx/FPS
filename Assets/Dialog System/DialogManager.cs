using Player.Scripts;
using UnityEngine;
using UnityEngine.Events;

namespace Dialog_System
{
    public class DialogManager : MonoBehaviour
    {
        public static UnityEvent OnDisplayDialog = new UnityEvent();
        public static UnityEvent OnHideDialog = new UnityEvent();
        
        public static DialogManager instance;

        private PlayerStateMachine player;
        
        private bool isDialogDisplayed;
        public bool IsDialogDisplayed => isDialogDisplayed;

        [HideInInspector] public string npcName; 
        
        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            player = PlayerStateMachine.instance;
        }

        public void TriggerDialog(Transform lookTarget, string npc)
        {
            npcName = npc;
            
            isDialogDisplayed = !isDialogDisplayed;
            if (isDialogDisplayed)
                DisplayDialog(lookTarget);
            else
                HideDialog();
        }

        private void DisplayDialog(Transform lookTarget)
        {
            player.playerLocked.SetLockState(PlayerStateMachine.instance, PlayerLocked.LockState.Dialog, lookTarget);
            OnDisplayDialog?.Invoke();
        }
        
        private void HideDialog()
        {
            player.ChangeBehaviour(player.playerRun);
            OnHideDialog?.Invoke();
        }
    }
}
