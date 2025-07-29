using System.Collections;
using Player.Scripts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Dialog_System
{
    public class DialogManager : MonoBehaviour
    {
        public static UnityEvent<DialogData> OnDisplayDialog = new UnityEvent<DialogData>();
        public static UnityEvent OnHideDialog = new UnityEvent();
        
        public static DialogManager instance;

        private PlayerStateMachine player;
        private DialogDisplay dialogDisplay;
        
        private bool isDialogDisplayed;
        public bool IsDialogDisplayed => isDialogDisplayed;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            player = PlayerStateMachine.instance;
            dialogDisplay = GetComponent<DialogDisplay>();
            dialogDisplay.OnReachDialogEnd.AddListener(() =>
            {
                isDialogDisplayed = false;
                StopAllCoroutines();
                StartCoroutine(HideDialog());
            });
        }

        public void TriggerDialog(Transform lookTarget, DialogData dialogData)
        {
            isDialogDisplayed = !isDialogDisplayed;
            
            StopAllCoroutines();
            if (isDialogDisplayed)
                StartCoroutine(DisplayDialog(lookTarget, dialogData));
            else
                StartCoroutine(HideDialog());
        }

        private IEnumerator DisplayDialog(Transform lookTarget, DialogData dialogData)
        {
            player.playerLocked.SetLockState(PlayerStateMachine.instance, PlayerLocked.LockState.Dialog, lookTarget);
            OnDisplayDialog?.Invoke(dialogData);

            yield return dialogDisplay.DisplayDialogBox();
            dialogDisplay.DisplayNewDialog(dialogData.dialoguesLines);
        }
        
        private IEnumerator HideDialog()
        {
            player.ChangeBehaviour(player.playerRun);
            OnHideDialog?.Invoke();
            
            dialogDisplay.StopDialog();
            yield return dialogDisplay.HideDialogBox();
        }
    }
}
