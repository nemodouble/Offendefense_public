using System;
using UnityEngine;
using UnityEngine.UI;

namespace com.Nemodouble.Offendefense.Scripts.UIs
{
    public class DecreaseEffectSlider : MonoBehaviour
    {
        private Slider _slider;
        private Slider _originSlider;
        private float _targetValue;
        
        private void Awake()
        {
            _slider = GetComponent<Slider>();
            _originSlider = transform.parent.GetComponent<Slider>();
            _targetValue = _originSlider.value;
            _slider.value = _targetValue;
            _slider.maxValue = _originSlider.maxValue;
            _slider.minValue = _originSlider.minValue;
        }

        private void Update()
        {
            if (Math.Abs(_slider.value - _targetValue) < 0.01f) return;
            _slider.value = Mathf.Lerp(_slider.value, _targetValue, Time.deltaTime * 5f);
        }

        public void ChangedOriginSliderValue(float value)
        {
            _targetValue = value;
        }
    }
}