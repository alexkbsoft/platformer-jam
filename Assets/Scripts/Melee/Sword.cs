using System;
using System.Collections;
using Health;
using UnityEngine;

namespace Melee
{
    public class Sword : Weapon
    {
        [SerializeField] private float attackRadius;
        [SerializeField] private LayerMask layer;

        private void Update()
        {
            if (Input.GetMouseButtonUp(1)) Attack();
        }

        private void Attack()
        {
            if (_isCooldown) return;

            AnimateSword();
            PlaySoundWeapon();
            
            var colliders = Physics2D.OverlapCircleAll(transform.position, attackRadius, layer);

            foreach (var collider in colliders)
                if (collider.TryGetComponent(out HealthProcessor healthProcessor))
                    healthProcessor.TakeDamage(Damage);

            StartCoroutine(Cooldown());
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