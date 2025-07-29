using Items;
using UnityEngine;

namespace Dialog_System
{
    public class DialogTrigger : Interactable
    {
        [SerializeField] private Transform lookTarget;
        [SerializeField] DialogData dialogData;

        private bool isDialogDisplayed;
        
        private void Start()
        {
            DialogManager.OnHideDialog.AddListener(() =>
            {
                if (isDialogDisplayed && isPlayerInRange)
                    CreateTooltip();
            });
        }

        public override void Interact()
        {
            isDialogDisplayed = !DialogManager.instance.IsDialogDisplayed;
            UpdateTooltipDisplay();
            TriggerDialog();
        }

        private void UpdateTooltipDisplay()
        {
            if (DialogManager.instance.IsDialogDisplayed && isPlayerInRange)
                CreateTooltip();
            else
                DestroyTooltip();
        }
        
        private void TriggerDialog()
        {
            DialogManager.instance.TriggerDialog(lookTarget, dialogData);
        }

        protected override void OnPlayerExitRange()
        {
            if (DialogManager.instance.IsDialogDisplayed)
                DialogManager.instance.TriggerDialog(lookTarget, dialogData);
            
            base.OnPlayerExitRange();
        }
    }
}
