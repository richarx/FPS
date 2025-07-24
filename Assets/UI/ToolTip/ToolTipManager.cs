using TMPro;
using UnityEngine;

namespace UI.ToolTip
{
    public class ToolTipManager : MonoBehaviour
    {
        //Press <b><color="yellow">E</b></color> to Loot Weapon
        
        [SerializeField] private Transform tooltipPivot;
        [SerializeField] private GameObject tooltipPrefab;

        public static ToolTipManager instance;

        private void Awake()
        {
            instance = this;
        }

        public GameObject DisplayToolTip(string text, float destroyAfterDuration = -1.0f)
        {
            GameObject tooltip = Instantiate(tooltipPrefab, tooltipPivot.position, Quaternion.identity, tooltipPivot);

            if (destroyAfterDuration > 0.0f)
                Destroy(tooltip, destroyAfterDuration);

            SetTooltipText(tooltip, text);
            
            return tooltip;
        }

        private void SetTooltipText(GameObject tooltip, string text)
        {
            if (text.Contains("$"))
                text = AddColorToText(text);

            tooltip.GetComponent<TextMeshProUGUI>().text = text;
        }

        private string AddColorToText(string text)
        {
            string[] blocs = text.Split("$");

            if (blocs.Length != 3)
                return text;

            return $"{blocs[0]}<b><color=\"yellow\">{blocs[1]}</b></color>{blocs[2]}";
        }
    }
}
