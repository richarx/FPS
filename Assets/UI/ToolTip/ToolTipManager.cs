using UnityEngine;

namespace UI.ToolTip
{
    public class ToolTipManager : MonoBehaviour
    {
        [SerializeField] private Transform tooltipPivot;
        [SerializeField] private Tooltip tooltipPrefab;
        [SerializeField] private GameplayTooltip gameplayTooltipPrefab;

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
        
        public Tooltip DisplayGameplayToolTip(string textGamepad, string textKeyboard, float destroyAfterDuration = -1.0f)
        {
            if (isTooltipDisplayed)
                currentTooltip.HideInstantly();
            
            GameplayTooltip gameplayTooltip = Instantiate(gameplayTooltipPrefab, tooltipPivot.position, Quaternion.identity, tooltipPivot);
            gameplayTooltip.Setup(textGamepad, textKeyboard, destroyAfterDuration);

            currentTooltip = gameplayTooltip;
            
            return currentTooltip;
        }
    }
}
