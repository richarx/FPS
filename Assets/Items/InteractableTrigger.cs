using UI.ToolTip;
using UnityEngine;

namespace Items
{
    public class InteractableTrigger : Interactable
    {
        private GameObject tooltip;

        public override void Interact()
        {
            OnTrigger?.Invoke();
        }
        
        protected override void SetItemDisplay(bool isInteractable)
        {
            if (isInteractable)
                tooltip = ToolTipManager.instance.DisplayToolTip("Press $E$ to Interact");
            else if (tooltip != null)
                Destroy(tooltip);
        }
    }
}
