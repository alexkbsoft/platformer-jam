using UnityEngine;

namespace Enemies.StateMachines.States
{
    public class WalkState : CharacterState
    {
        protected Transform _target;
        private readonly float _speed;
        protected readonly float _stayDistance;

        public WalkState(Transform player, Transform target, float speed, float stayDistance, Rigidbody2D rbody, Animator animator) : base(player, rbody, animator)
        {
            _target = target;
            _speed = speed;
            _stayDistance = stayDistance;
            Animation = "walk";
        }

        public override void FixedExecute()
        {
            base.FixedExecute();
            WalkTo(_target.position, _speed);
        }

        private void WalkTo(Vector2 direction, float speed) =>
            _rbody.velocity = new Vector2(direction.x, 0) * speed * Time.deltaTime;
    }
}