using UnityEngine;

namespace Enemies.StateMachines.States
{
    public abstract class CharacterState
    {
        private Animator _animator;
        public bool IsCompleted { get; private set; } = true;
        protected string Animation { get; set; }

        protected Transform _player;
        protected Rigidbody2D _rbody;
        
        public CharacterState(Transform player, Rigidbody2D rbody, Animator animator)
        {
            _animator = animator;
            _player = player;
            _rbody = rbody;
        }
        
        public virtual bool CanEnter() => IsCompleted;
        public virtual void Enter() => _animator.CrossFade(Animation, 0);

        public virtual void Execute()
        {
        }

        public virtual void FixedExecute() => SideByVelocity();
        public virtual void Exit() => _animator.StopPlayback();
        protected void SafetyCompleting() => IsCompleted = AnimationCompleted();
        protected void SideByVelocity() => RotateObj(_rbody.velocity.x < 0 ? -1 : 0);
        private void RotateObj(float angle) => _player.transform.localRotation = new Quaternion(0, angle, 0, 0);
        
        protected bool AnimationCompleted()
        {
            var animInfo = _animator.GetCurrentAnimatorStateInfo(0);
            return animInfo.normalizedTime >= animInfo.length + 0.45f;
        }

        protected float GetPercentCurrentMomentAnim()
        {
            var animInfo = _animator.GetCurrentAnimatorStateInfo(0);
            return (animInfo.normalizedTime / animInfo.length + 0.45f) * 100;
        }
    }
}