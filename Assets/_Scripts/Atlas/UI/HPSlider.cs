using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Atlas.UI
{
    public class HPSlider : MonoBehaviour
    {
        [SerializeField]
        private Slider _slider;
        [SerializeField]
        private TMP_Text _numbers;

        public void Initialize()
        {
            _slider.value = 1;
            _numbers.text = "";
        }

        public void UpdateSlider(int value, int maxValue)
        {
            _slider.value = (float) value / maxValue;
            _numbers.text = $"{value}/{maxValue}";
        }   
    }
}
