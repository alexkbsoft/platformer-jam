using UnityEngine;

namespace Enemies.StateMachines.States
{
    public class WalkState : CharacterState
    {
        protected Transform _target;
        private readonly float _speed;

        public WalkState(Transform player, Transform target, float speed, float stayDistance, Rigidbody2D rbody, Animator animator) : base(player, rbody, animator)
        {
            _target = target;
            _speed = speed;
            Animation = "walk";
        }

        public override void FixedExecute()
        {
            base.FixedExecute();
            WalkTo(GetDirection(_target.position), _speed);
        }

        private void WalkTo(Vector2 direction, float speed) =>
            _rbody.velocity = new Vector2(direction.x, _rbody.velocity.y) * speed * Time.deltaTime;

        private Vector2 GetDirection(Vector2 target) => target - (Vector2)_player.transform.position;
    }
}