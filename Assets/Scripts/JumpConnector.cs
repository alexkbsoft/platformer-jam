using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpConnector : MonoBehaviour
{
    [SerializeField] HingeJoint2D _closestHinge;

    private PlayerMovement _playerMovement;
    private Rigidbody2D _playerRb;

    void Start()
    {
        _playerRb = GetComponent<Rigidbody2D>();
        _playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (_closestHinge.connectedBody == null)
            {
                _closestHinge.connectedBody = _playerRb;
                _playerRb.constraints = RigidbodyConstraints2D.None;
                _playerMovement.enabled = false;
                _playerRb.gravityScale = 1;

            } else
            {
                _closestHinge.connectedBody = null;
            }
        }
    }
}
