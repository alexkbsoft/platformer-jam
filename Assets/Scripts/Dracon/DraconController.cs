using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DraconController : MonoBehaviour,Idamagebl
{
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _currentHealth;

    [Header("Target data")]
    [SerializeField] private Transform _target;

    [SerializeField] private float _visionDistance, _patrolDistance;
    [SerializeField] private float _speed;
    [SerializeField] private Vector3 _startlocalScale;
    [SerializeField] private LayerMask _layer;


    [Header("Fire data")]
    [SerializeField] private FireBall _projectilePrefab;

    [SerializeField] private Transform _firePoint;


    private Vector2 _startPosition, _patrolPosition;
    private Transform _thisTransform;
    private bool _isRightRotation, _isSeePlayer, _corutineStarted, _isDead;
    private float _distance;
    private Animator _animator;
    private Rigidbody2D _rb;
    private static readonly int Fire1 = Animator.StringToHash("Fire");
    private static readonly int Dead1 = Animator.StringToHash("Dead");


    void Start()
    {
        _thisTransform = GetComponent<Transform>();
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _rb.simulated = false;
        _startPosition = _thisTransform.position;
        _patrolPosition = ChangePatrolPosition();
        _currentHealth = _maxHealth;
        Debug.Log(_startlocalScale);
    }

    private void FixedUpdate()
    {
        if (_isDead) return;
        _isSeePlayer = IsPlayercanSee();
        if (_isSeePlayer)
        {
           _animator.SetBool(Fire1, true);
        }
        else
        {
            _animator.SetBool(Fire1, false);
            Patrol();
        }
        CheckRotation(_isSeePlayer);
    }

    public void Fire()
    {
        var fire = Instantiate(_projectilePrefab);
        fire.transform.position = _firePoint.position;
        fire.transform.up = _target.position - _firePoint.position;
    }

    private void Patrol()
    {
        _distance = (_patrolPosition - (Vector2)_thisTransform.position).SqrMagnitude();
        if (_distance <= 0.1f)
        {
            _patrolPosition = ChangePatrolPosition();
        }
        else
        {
            _thisTransform.position += ((Vector3)_patrolPosition - _thisTransform.position) * (_speed * Time.deltaTime);
        }
    }

    private bool IsPlayercanSee()
    {
        _distance = Vector2.Distance(_target.position, _thisTransform.position);
        if (_distance <= _visionDistance && !Physics2D.Raycast(_thisTransform.position, (_target.position-_thisTransform.position),_distance,_layer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private Vector2 ChangePatrolPosition()
    {
        bool isNeededPosition = false;
        Vector2 patrolPosition;
        while (!isNeededPosition)
        {
            patrolPosition = _startPosition + new Vector2(Random.Range(-_patrolDistance, _patrolDistance),
                Random.Range(-_patrolDistance, _patrolDistance));
            _distance = Vector2.Distance(patrolPosition, _thisTransform.position);
            if (!Physics2D.Raycast(_thisTransform.position, (patrolPosition - (Vector2)_thisTransform.position), _distance,_layer))
            {
                isNeededPosition = true;
                return patrolPosition;
            }
        }
       

        return _startPosition;
    }

    private void CheckRotation(bool isSeePlayer)
    {
        if (!isSeePlayer)
        {
            if (_patrolPosition.x > _thisTransform.position.x)
            {
                _thisTransform.localScale = new Vector3(-_startlocalScale.x, _startlocalScale.y, _startlocalScale.z);
            }
            else if (_patrolPosition.x < _thisTransform.position.x)
            {
                _thisTransform.localScale = new Vector3(_startlocalScale.x, _startlocalScale.y, _startlocalScale.z);
            }
        }
        else
        {
            if (_target.position.x > _thisTransform.position.x)
            {
                _thisTransform.localScale = new Vector3(-_startlocalScale.x, _startlocalScale.y, _startlocalScale.z);
            }
            else
            {
                _thisTransform.localScale = new Vector3(_startlocalScale.x, _startlocalScale.y, _startlocalScale.z);
            }
        }
    }

    public void Damage(float damage)
    {
        _currentHealth -= damage;
        if (_currentHealth <=0)
            Dead();
    }

    public void Dead()
    {
        _animator.SetTrigger(Dead1);
        _isDead = true;
        _rb.simulated = true;
    }
}
