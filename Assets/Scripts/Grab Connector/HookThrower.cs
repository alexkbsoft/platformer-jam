using Unity.Mathematics;
using UnityEngine;

namespace Grab_Connector
{
    public class HookThrower : MonoBehaviour
    {
        [SerializeField] private GameObject _hookPrefab;
        [SerializeField] private JumpConnector _jumpConnector;
        [SerializeField] [Range(0, 10000)] private float _forceThrow;

        private GameObject _prefabInstance;
        private HingeJoint2D _lastChain;
        private HookConnector _hook;

        private void Update()
        {
            if (Input.GetMouseButtonUp(0))ThrowHook();
        }
        
        private void ThrowHook()
        {
            if (_prefabInstance != null) Destroy(_prefabInstance.gameObject);

            _prefabInstance = Instantiate(_hookPrefab, transform.position, quaternion.identity);
            _hook = _prefabInstance.GetComponentInChildren<HookConnector>();
            _lastChain = _prefabInstance.GetComponentInChildren<LastChain>().GetComponent<HingeJoint2D>();

            Throw();
            GrabJoint(_lastChain);
        }

        private void Throw() => _hook.GetComponent<Rigidbody2D>().velocity =
            GetDirection().normalized * _forceThrow;

        private Vector2 GetDirection() => Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        private void GrabJoint(HingeJoint2D joint) => _jumpConnector.GrabJoint(joint);
    }
}