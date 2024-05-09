using UnityEngine;
using UnityEngine.UI;

namespace Health
{
    public class ValueBar : MonoBehaviour
    {
        [SerializeField] protected Slider slider;
      
        public virtual void SetCurrentValue(float value)
        {
            slider.value = value;
        }
    }
}