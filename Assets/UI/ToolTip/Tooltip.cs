using TMPro;
using UnityEngine;

namespace UI.ToolTip
{
    public class Tooltip : MonoBehaviour
    {
        [SerializeField] private float fadeInDuration;
        [SerializeField] private float fadeOutDuration;
        
        protected TextMeshProUGUI textMeshProUGUI;
        
        private bool isHiding;
        private float destroyTimer = -1.0f;
        
        public void Setup(string text, float destroyAfterDuration)
        {
            textMeshProUGUI = GetComponent<TextMeshProUGUI>();
            textMeshProUGUI.text = ComputeTooltipText(text);

            if (destroyAfterDuration > 0.0f)
                destroyTimer = Time.time + destroyAfterDuration;

            StartCoroutine(Tools.Fade(textMeshProUGUI, fadeInDuration, true));
        }

        private void Update()
        {
            if (destroyTimer > 0.0f && Time.time >= destroyTimer)
                Hide();
        }

        protected string ComputeTooltipText(string text)
        {
            if (text.Contains("$"))
                text = AddColorToText(text);

            return text;
        }

        //Press <b><color="yellow">E</b></color> to Loot Weapon
        private string AddColorToText(string text)
        {
            string[] blocs = text.Split("$");

            if (blocs.Length != 3)
                return text;
            
            return $"{blocs[0]}<b><color=\"yellow\">{blocs[1]}</b></color>{blocs[2]}";
        }

        public void Hide()
        {
            if (isHiding)
                return;

            isHiding = true;
            StopAllCoroutines();
            StartCoroutine(Tools.Fade(textMeshProUGUI, fadeOutDuration, false, textMeshProUGUI.color.a));
            Destroy(gameObject, fadeOutDuration);
        }

        public void HideInstantly()
        {
            isHiding = true;
            Destroy(gameObject);
        }
    }
}
