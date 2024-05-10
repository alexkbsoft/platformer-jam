using System;
using System.Collections;
using Health;
using UnityEngine;

namespace Melee
{
    public class Sword : MonoBehaviour
    {
        [SerializeField] private GameObject weaponAnimator;
        [SerializeField] private float attackRadius;
        [SerializeField] private float attackCooldown = 0.3f;
        [SerializeField] private LayerMask layer;

        private bool _isCooldown;
        
        [field: SerializeField] public float Damage { get; set; }
        
        private void Update()
        {
            if (Input.GetMouseButtonUp(1)) Attack();
        }

        private void Attack()
        {
            if (_isCooldown) return;

            AnimateSword();
            
            var colliders = Physics2D.OverlapCircleAll(transform.position, attackRadius, layer);

            foreach (var collider in colliders)
                if (collider.TryGetComponent(out HealthProcessor healthProcessor))
                    healthProcessor.TakeDamage(Damage);

            StartCoroutine(Cooldown());
        }

        private void AnimateSword()
        {
            weaponAnimator.SetActive(false);
            weaponAnimator.SetActive(true);
        }

        private IEnumerator Cooldown()
        {
            _isCooldown = true;
            yield return new WaitForSeconds(attackCooldown);
            _isCooldown = false;
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRadius);
        }
#endif
    }
}