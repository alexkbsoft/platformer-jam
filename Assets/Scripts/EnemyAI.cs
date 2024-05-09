using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float walkSpeed, runSpeed;
    [SerializeField] private float viewingRadius = 3, runDistance = 2.5f, walkDistance = 1.5f, stayDistance = 0.5f;
    [SerializeField] private LayerMask layerMask;

    private Collider2D _target;
    private Rigidbody2D _rbody;

    public void Awake()
    {
        _rbody = GetComponent<Rigidbody2D>();
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
            WalkFollow();
            return;
        }

        if (CanRunFollow())
        {
            RunFollow();
        }
    }

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

    private bool CanStay() => GetTargetDistance() <= stayDistance;
    private bool CanWalkFollow() => GetTargetDistance() <= walkDistance;
    private bool CanRunFollow() => GetTargetDistance() <= runDistance;

    private void RunFollow()
    {
        WalkTo(GetVectorToTarget(), runSpeed);
    }

    private void WalkFollow()
    {
        WalkTo(GetVectorToTarget(), walkSpeed);
    }

    private void Idle()
    {
        Debug.Log("idle");
    }

    private void Attack()
    {
        Debug.Log("attacked!");
    }

    private void WalkTo(Vector2 direction, float speed) =>
        _rbody.velocity = new Vector2(direction.x, 0) * speed * Time.deltaTime;

    private bool TargetIsNear() => GetTargetDistance() <= stayDistance;
    private float GetTargetDistance() => Vector2.Distance(transform.position, _target.transform.position);
    private Vector2 GetVectorToTarget() => _target.transform.position - transform.position;
}