using UnityEngine;

namespace Enemies.StateMachines.States
{
    public class FallState : CharacterState
    {
        public FallState(Transform player, Rigidbody2D rbody, Animator animator) : base(player, rbody, animator)
        {
            Animation = "fall";
        }

        public override void Enter()
        {
            base.Enter();
            _rbody.gravityScale = 10;
        }

        public override void Exit()
        {
            base.Exit();
            _rbody.gravityScale = 1;
        }
    }
}