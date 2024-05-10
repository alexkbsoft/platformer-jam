using Health;
using UnityEngine;

namespace Shoot
{
    public class Damageable : MonoBehaviour
    {
        [field: SerializeField] public float Damage { get; set; }

        private void OnCollisionEnter2D(Collision2D other)
        {
            DoDamage(other.gameObject);
        }

        protected virtual void DoDamage(GameObject go)
        {
            if (go.gameObject.TryGetComponent(out HealthProcessor health)) health.TakeDamage(Damage);
        }
    }
}