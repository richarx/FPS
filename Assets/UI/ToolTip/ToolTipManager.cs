using UnityEngine;

namespace UI.ToolTip
{
    public class ToolTipManager : MonoBehaviour
    {
        [SerializeField] private Transform tooltipPivot;
        [SerializeField] private Tooltip tooltipPrefab;

        public static ToolTipManager instance;

        private Tooltip currentTooltip;
        public bool isTooltipDisplayed => currentTooltip != null;
        public Tooltip CurrentToolTip => currentTooltip;

        private void Awake()
        {
            instance = this;
        }

        public Tooltip DisplayToolTip(string text, float destroyAfterDuration = -1.0f)
        {
            if (isTooltipDisplayed)
                currentTooltip.HideInstantly();
            
            currentTooltip = Instantiate(tooltipPrefab, tooltipPivot.position, Quaternion.identity, tooltipPivot);
            currentTooltip.Setup(text, destroyAfterDuration);

            return currentTooltip;
        }
    }
}
