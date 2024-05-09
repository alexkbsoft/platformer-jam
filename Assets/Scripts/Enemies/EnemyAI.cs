using System.Collections;
using Health;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class EnemyAI : MonoBehaviour
{
    [Header("Enemy parameters")] 
    [SerializeField] private float damage;
    [SerializeField] private float attackDelay;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;

    [Space] [Header("Radius to Target")] 
    [SerializeField] private float viewingRadius = 3;
    [SerializeField] private float runDistance = 2.5f;
    [SerializeField] private float walkDistance = 1.5f;
    [SerializeField] private float stayDistance = 0.5f;

    [Space] [Header("Target LayerMask")] 
    [SerializeField] private LayerMask layerMask;

    private Collider2D _target;
    private Rigidbody2D _rbody;
    private Animator _animator;
    private bool _isDelayedAttack;

    public void Awake()
    {
        _rbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    public void Update()
    {
        if (!HasTarget())
        {
            Idle();
            return;
        }

        LookToTarget();

        if (TargetIsNear())
        {
            if (CanAttack()) Attack();
            else Idle();
            return;
        }

        if (CanWalkFollow())
        {
            WalkFollow(CanWalkFollow());
            return;
        }

        if (CanRunFollow())
        {
            RunFollow(CanRunFollow());
        }
    }

    private bool CanAttack()
    {
        return true;
    }

    private void LookToTarget()
    {
    }

    private bool HasTarget()
    {
        _target = Physics2D.OverlapCircle(transform.position, viewingRadius, layerMask);
        return _target;
    }

    private bool CanWalkFollow() => GetTargetDistance() <= walkDistance;
    private bool CanRunFollow() => GetTargetDistance() <= runDistance;

    private void RunFollow(bool canRun)
    {
        _animator.SetBool("isRun", canRun);
        WalkTo(GetVectorToTarget(), runSpeed);
    }

    private void WalkFollow(bool canWalk)
    {
        _animator.SetBool("isWalk", canWalk);
        WalkTo(GetVectorToTarget(), walkSpeed);
    }

    private void Idle()
    {
    }

    private void Attack()
    {
        if (_isDelayedAttack || !_target.TryGetComponent(out HealthProcessor health)) return;

        _animator.SetBool("isAttack", true);
        health.TakeDamage(damage);
        StartCoroutine(AttackDelay());
    }

    private IEnumerator AttackDelay()
    {
        _isDelayedAttack = true;
        yield return new WaitForSeconds(attackDelay);
        _isDelayedAttack = false;
    }

    private void WalkTo(Vector2 direction, float speed) =>
        _rbody.velocity = new Vector2(direction.x, 0) * speed * Time.deltaTime;
    
    private bool TargetIsNear() => GetTargetDistance() <= stayDistance;
    private float GetTargetDistance() => Vector2.Distance(transform.position, _target.transform.position);
    private Vector2 GetVectorToTarget() => _target.transform.position - transform.position;


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