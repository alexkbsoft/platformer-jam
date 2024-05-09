using Health;
using UnityEngine;
using Task = System.Threading.Tasks.Task;

namespace Enemies.StateMachines.States
{
    public class AttackState : WalkState
    {
        private bool _isDelayedAttack;
        private int _attackDelay = 1500; //
        private float _damage = 5; //
        
        public AttackState(Transform player, Transform target, float speed, float stayDistance, Rigidbody2D rbody, Animator animator)
            : base(player, target, speed, stayDistance, rbody, animator)
        {
            Animation = "attack";
        }

        public override bool CanEnter() => IsCompleted;

        public override void Execute()
        {
            CheckAttack();
            SafetyCompleting();
        }

        private void CheckAttack()
        {
            if (TargetIsNear()) Attack();
        }

        private void Attack()
        {
            if (_isDelayedAttack || !_target.TryGetComponent(out HealthProcessor health)) return;

            _isDelayedAttack = true;
            health.TakeDamage(_damage);
            Task.Delay(_attackDelay).ContinueWith(_ =>_isDelayedAttack = false);
        }
        
        private bool TargetIsNear() => GetTargetDistance() <= _stayDistance;
        private float GetTargetDistance() => Vector2.Distance(_player.position, _target.position);
    }
}