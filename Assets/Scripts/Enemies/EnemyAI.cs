using System;
using System.Collections;
using Enemies.StateMachines;
using Enemies.StateMachines.States;
using Health;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class EnemyAI : MonoBehaviour
{
    [Header("Enemy parameters")] [SerializeField]
    private float damage;

    [SerializeField] private float attackDelay;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;

    [Space] [Header("Radius to Target")] [SerializeField]
    private float viewingRadius = 3;

    [SerializeField] private float runDistance = 2.5f;
    [SerializeField] private float walkDistance = 1.5f;
    [SerializeField] private float stayDistance = 0.5f;

    [Space] [Header("Target LayerMask")] 
    [SerializeField] private LayerMask layerMask;
    
    [Space] [Header("Sounds")] 
    [SerializeField] private AudioSource attackSound;
    [SerializeField] private AudioSource stepsSound;

    private PersonStateMachine _stateMachine;
    private Collider2D _target;
    private Rigidbody2D _rbody;
    private Animator _animator;
    private bool _isDelayedAttack;

    private void Awake()
    {
        _rbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _stateMachine = new PersonStateMachine(new IdleState(transform, _rbody, _animator));
    }

    private void FixedUpdate()
    {
        _stateMachine.FixedExecute();
    }

    private void Update()
    {
        _stateMachine.Execute();

        stepsSound.mute = Mathf.Abs(_rbody.velocity.x) <= 0;
        
        if (IsFall())
        {
            Fall();
            return;
        }
        
        if (!HasTarget())
        {
            Idle();
            return;
        }

        if (TargetIsFar())
        {
            _target = null;
            return;
        }
        
        if (TargetIsNear())
        {
            if (CanAttack()) Attack();
            else Idle();
            return;
        }

        if (CanWalkFollow())
        {
            Walk();
            return;
        }

        if (CanRunFollow())
        {
            Run();
        }
    }

    private bool HasTarget()
    {
        _target = Physics2D.OverlapCircle(transform.position, viewingRadius, layerMask);
        return _target;
    }

    private void Idle() => _stateMachine.ChangeState(new IdleState(transform, _rbody, _animator));
    private void Walk() => _stateMachine.ChangeState(new WalkState(transform, _target.transform, walkSpeed, walkDistance, _rbody, _animator));
    private void Run() => _stateMachine.ChangeState(new RunState(transform, _target.transform, runSpeed, runDistance, _rbody, _animator));
    private void Fall() => _stateMachine.ChangeState(new FallState(transform, _rbody, _animator));
    private bool CanWalkFollow() => GetTargetDistance() <= walkDistance;
    private bool CanRunFollow() => GetTargetDistance() <= runDistance;
    private bool TargetIsFar() => GetTargetDistance() > viewingRadius;
    private bool TargetIsNear() => GetTargetDistance() <= stayDistance;
    private float GetTargetDistance() => Vector2.Distance(transform.position, _target.transform.position);
    private bool IsFall() => _rbody.velocity.y < -0.2f;
    private bool CanAttack() => true;
    private void Attack()
    {
        if (_isDelayedAttack || _target.GetComponent<HealthProcessor>() == null) return;

        attackSound.Play();
        _stateMachine.ChangeState(new AttackState(transform, _target.transform, damage, walkSpeed, stayDistance, _rbody, _animator));
        StartCoroutine(AttackDelay());
    }

    private IEnumerator AttackDelay()
    {
        _isDelayedAttack = true;
        yield return new WaitForSeconds(attackDelay);
        _isDelayedAttack = false;
    }
    
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, viewingRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, walkDistance);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, runDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stayDistance);
    }
#endif
}