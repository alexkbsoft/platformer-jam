using UnityEngine;

namespace Health
{
    public class HealthProcessor : MonoBehaviour
    {
        [field: SerializeField] private ValueBar Bar { get; set; }
        [field: SerializeField] private float MaxValue { get; set; }
        [SerializeField] private float currentValue;
        [SerializeField] private Transform _spawnPoint;

        private float CurrentValue
        {
            get => currentValue;
            set
            {
                currentValue = Mathf.Clamp(value, MinValue, MaxValue);
                if (value < 0) Death();
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

        private void Death()
        {
            if (_spawnPoint) Respawn();
            else gameObject.SetActive(false);
        }

        private void Respawn()
        {
            transform.position = _spawnPoint.position;
            TakeHeal(MaxValue);
        }
    }
}