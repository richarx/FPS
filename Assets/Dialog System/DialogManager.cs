using System;
using Player.Scripts;
using UnityEngine;

namespace Dialog_System
{
    public class DialogManager : MonoBehaviour
    {
        public static DialogManager instance;

        private PlayerStateMachine player;
        
        private bool isDialogDisplayed;
        
        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            player = PlayerStateMachine.instance;
        }

        public void TriggerDialog(Transform lookTarget)
        {
            Debug.Log("Trigger Dialog");

            isDialogDisplayed = !isDialogDisplayed;
            if (isDialogDisplayed)
                DisplayDialog(lookTarget);
            else
                HideDialog();
        }

        private void DisplayDialog(Transform lookTarget)
        {
            player.playerLocked.SetLookTarget(lookTarget);
            player.ChangeBehaviour(player.playerLocked);
        }
        
        private void HideDialog()
        {
            player.ChangeBehaviour(player.playerRun);
        }
    }
}
