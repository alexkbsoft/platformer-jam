using UnityEngine;

namespace Shoot
{
    public class Damageable : MonoBehaviour
    {
        [field: SerializeField] public float Damage { get; set; }

        protected virtual void DoDamage()
        {
            Debug.Log($"Нанесено {Damage} урона");
        }
    }
}