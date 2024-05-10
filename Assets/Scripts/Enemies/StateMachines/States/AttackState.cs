using Health;
using UnityEngine;
using Task = System.Threading.Tasks.Task;

namespace Enemies.StateMachines.States
{
    public class AttackState : WalkState
    {
        private float _damage = 5;

        public AttackState(Transform player, Transform target, float damage, float speed, float stayDistance,
            Rigidbody2D rbody, Animator animator)
            : base(player, target, speed, stayDistance, rbody, animator)
        {
            Animation = "attack";
            _damage = damage;
        }

        public override bool CanEnter() => IsCompleted;
        
        public override void Enter()
        {
            base.Enter();
            Attack();
        }

        public override void Execute() => SafetyCompleting();
        public override void FixedExecute() => SideByVelocity();
        
        private void Attack()
        {
            if (_target.TryGetComponent(out HealthProcessor health)) health.TakeDamage(_damage);
        }
    }
}