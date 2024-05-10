using UnityEngine;

namespace Enemies.StateMachines.States
{
    public class IdleState : CharacterState
    {
        public IdleState(Transform player, Rigidbody2D rbody, Animator animator) : base(player, rbody, animator)
        {
            Animation = "idle";
        }
    }
}