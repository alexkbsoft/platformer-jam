using UnityEngine;

namespace Shoot
{
    public class Damageable : MonoBehaviour
    {
        [SerializeField] private float damage;
        
        private void OnCollisionEnter2D(Collision2D other) => DoDamage();

        protected virtual void DoDamage()
        {
            Debug.Log($"Нанесено {damage} урона");
        }
    }
}