using UnityEngine;

namespace Health
{
    public class HealthProcessor : MonoBehaviour
    {
        public delegate void HealthEvent();
        public HealthEvent Die;

        [field: SerializeField] private ValueBar Bar { get; set; }
        [field: SerializeField] private float MaxValue { get; set; }
        [SerializeField] private float currentValue;

        private float CurrentValue
        {
            get => currentValue;
            set
            {
                if (value <= MaxValue && value >= MinValue) currentValue = value;
                if (value == 0) Die?.Invoke();
            }
        }
        
        private float MinValue { get; } = 0;

        public void TakeHeal(float value) => AddValue(value);
        public void TakeDamage(float value) => AddValue(-value);

        private void AddValue(float value)
        {
            CurrentValue += value;
            ChangeBar();
        }

        private void ChangeBar()
        {
            if (Bar != null) Bar.SetCurrentValue(GetPercentageRation());
        }

        private float GetPercentageRation() => CurrentValue / MaxValue * 100;
    }
}