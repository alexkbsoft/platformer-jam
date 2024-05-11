using System.Collections;
using UnityEngine;

namespace Health
{
    public class HealthProcessor : MonoBehaviour
    {
        [field: SerializeField] private ValueBar Bar { get; set; }
        [field: SerializeField] private float MaxValue { get; set; }
        [SerializeField] private float currentValue;
        [SerializeField] private float DamageRollbackDelaly = 1.5f;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private AudioSource deathSound;
        [SerializeField] private AudioSource hitSound;

        private bool _canDamage = true;

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

        public void TakeDamage(float value)
        {
            if (!_canDamage) return;
            
            hitSound.Play();
            AddValue(-value);
            StartCoroutine(DamageRollback());
        }

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
            deathSound.Play();
            if (_spawnPoint) Respawn();
            else gameObject.SetActive(false);
        }

        private void Respawn()
        {
            transform.position = _spawnPoint.position;
            TakeHeal(MaxValue);
        }

        private IEnumerator DamageRollback()
        {
            _canDamage = false;
            _renderer.color = Color.red;
            yield return new WaitForSeconds(DamageRollbackDelaly);
            _canDamage = true;
            _renderer.color = Color.white;
        }
    }
}