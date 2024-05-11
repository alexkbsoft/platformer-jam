using System;
using System.Collections;
using UnityEngine;

namespace Health
{
    public class HealthProcessor : MonoBehaviour
    {
        public bool IsDeath { get; private set; }
        [field: SerializeField] private ValueBar Bar { get; set; }
        [field: SerializeField] private float MaxValue { get; set; }
        
        [SerializeField] private float currentValue;
        [SerializeField] private float damageCooldown = 1;
        [SerializeField] private float deathDelay = 5;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private SpriteRenderer renderer;
        [SerializeField] private Animator animator;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private AudioSource deathSound;
        [SerializeField] private AudioSource hitSound;

        private bool _canDamage = true;
        
        private float CurrentValue
        {
            get => currentValue;
            set
            {
                currentValue = Mathf.Clamp(value, MinValue, MaxValue);
                if (value < 0 && !IsDeath) Death();
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
            StartCoroutine(spawnPoint ? CleaningOfCorpses(Respawn) : CleaningOfCorpses(Disable));
        }

        private void Respawn()
        {
            IsDeath = false;
            animator.CrossFade("idle", 0);
            transform.position = spawnPoint.position;
            TakeHeal(MaxValue);
        }

        private void Disable() => gameObject.SetActive(false);
        
        private IEnumerator CleaningOfCorpses(Action a)
        {
            IsDeath = true;
            animator.CrossFade("death", 0);
            yield return new WaitForSeconds(deathDelay);
            a.Invoke();
        }
        
        private IEnumerator DamageRollback()
        {
            _canDamage = false;
            renderer.color = Color.red;
            yield return new WaitForSeconds(damageCooldown);
            _canDamage = true;
            renderer.color = Color.white;
        }
    }
}