using UnityEngine;

namespace Enemies.StateMachines.States
{
    public class RunState : WalkState
    {
        public RunState(Transform player, Transform target, float speed, float stayDistance, Rigidbody2D rbody, Animator animator)
            : base(player, target, speed, stayDistance, rbody, animator)
        {
            Animation = "run";
        }
    }
}