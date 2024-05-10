using System;
using Health;
using UnityEngine;

namespace Melee
{
    public class Sword : MonoBehaviour
    {
        [SerializeField] private GameObject weaponAnimator;
        [SerializeField] private float attackRadius;
        [SerializeField] private LayerMask layer;

        [field: SerializeField] public float Damage { get; set; }
        
        private void Update()
        {
            if (Input.GetMouseButtonUp(1)) Attack();
        }

        private void Attack()
        {
            AnimateSword();
            
            var colliders = Physics2D.OverlapCircleAll(transform.position, attackRadius, layer);

            foreach (var collider in colliders)
                if (collider.TryGetComponent(out HealthProcessor healthProcessor))
                    healthProcessor.TakeDamage(Damage);
        }

        private void AnimateSword()
        {
            weaponAnimator.SetActive(false);
            weaponAnimator.SetActive(true);
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