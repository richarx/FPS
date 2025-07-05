using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Pause_Menu
{
    public class SettingSlider : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
        
        public Slider slider;

        private float min;
        private float max;

        private void Start()
        {
            slider.onValueChanged.AddListener(UpdateText);
            UpdateText(slider.value);
        }

        public void Initialize(float value, float minValue, float maxValue)
        {
            min = minValue;
            max = maxValue;
            SetSliderValue(value);
        }

        public void SetSliderValue(float value)
        {
            slider.value = Tools.NormalizeValue(value, min, max);
        }

        public float GetSliderValue()
        {
            return ComputeValue(slider.value);
        }

        private float ComputeValue(float value)
        {
            return Tools.NormalizeValueInRange(value, 0.0f, 1.0f, min, max);
        }

        private void UpdateText(float newValue)
        {
            float value = ComputeValue(newValue);

            if (max >= 1000.0f)
                value /= 1000.0f;
            
            text.text = value.ToString(CultureInfo.InvariantCulture);
        }
    }
}
