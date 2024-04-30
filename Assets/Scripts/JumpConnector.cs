using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpConnector : MonoBehaviour
{
    [SerializeField] private HingeJoint2D _myHinge;
    [SerializeField] private Rigidbody2D _closestHinge;
    [SerializeField] [Range(0, 50)] private float _connectionRadius;
    [SerializeField] private LayerMask _layer;

    private PlayerMovement _playerMovement;
    private Rigidbody2D _playerRb;

    private void Start()
    {
        _playerRb = GetComponent<Rigidbody2D>();
        _playerMovement = GetComponent<PlayerMovement>();
        _myHinge ??= GetComponent<HingeJoint2D>();
        _myHinge.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.E))
        {
            SetClosestJoint();
            if (_closestHinge != null) GrabJoint();
        }
    }

    private void GrabJoint()
    {
        if (_myHinge.connectedBody == null)
        {
            _closestHinge.transform.position = transform.position;
            _myHinge.enabled = true;
            _myHinge.connectedBody = _closestHinge;
            _playerRb.constraints = RigidbodyConstraints2D.None;
            _playerMovement.enabled = false;
            _playerRb.gravityScale = 2;
        }
        else
        {
            _myHinge.connectedBody = null;
            _myHinge.enabled = false;
            transform.rotation = Quaternion.identity;
            _playerRb.constraints = RigidbodyConstraints2D.FreezeRotation;
            _playerMovement.enabled = true;
        }
    }

    private void SetClosestJoint()
    {
        var colliders = GetAllColliders();
        var joints = GetHingeJoints(colliders);
        
        if (joints.Length == 0) return;
        
        _closestHinge = GetClosestJoint(joints).GetComponent<Rigidbody2D>();
    }

    private Collider2D[] GetAllColliders() =>
        Physics2D.OverlapCircleAll(transform.position, _connectionRadius, _layer);

    private HingeJoint2D[] GetHingeJoints(Collider2D[] colliders)
    {
        var joints = new List<HingeJoint2D>();

        foreach (var c in colliders)
            if (c.TryGetComponent(out HingeJoint2D joint)) joints.Add(joint);

        return joints.ToArray();
    }
    
    private HingeJoint2D GetClosestJoint(HingeJoint2D[] joints)
    {
        var closestJoint = joints[0];
        var distance = Vector2.Distance(transform.position, closestJoint.transform.position);
        
        foreach (var joint in joints)
        {
            if (Vector2.Distance(transform.position, joint.transform.position) < distance)
                closestJoint = joint;
        }

        return closestJoint;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _connectionRadius);
    }
}