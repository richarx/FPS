using Items;
using UnityEngine;

namespace Dialog_System
{
    public class DialogTrigger : Interactable
    {
        [SerializeField] private Transform lookTarget;
        [SerializeField] private string npcName;

        public override void Interact()
        {
            UpdateTooltipDisplay();
            TriggerDialog();
            base.Interact();
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
            DialogManager.instance.TriggerDialog(lookTarget, npcName);
        }

        protected override void OnPlayerExitRange()
        {
            if (DialogManager.instance.IsDialogDisplayed)
                DialogManager.instance.TriggerDialog(lookTarget, npcName);
            
            base.OnPlayerExitRange();
        }
    }
}
