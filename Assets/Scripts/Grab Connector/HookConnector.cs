using Player;
using UnityEngine;

namespace Grab_Connector
{
    [RequireComponent(typeof(HingeJoint2D))]
    public class HookConnector : MonoBehaviour
    {
        public delegate void ButtonMessage();
        public ButtonMessage onUnconnectHook;
    
        private HingeJoint2D _hook;
        private bool _hookIsReady;
    
        private void Start()
        {
            _hookIsReady = true;
            _hook = GetComponent<HingeJoint2D>();
            onUnconnectHook += UnconnectHook;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!_hookIsReady || !other.gameObject.TryGetComponent(out Rigidbody2D rbody)) return;

            _hook.enabled = true;
            SetConnectedBody(rbody);
            _hookIsReady = false;
        }

        private void UnconnectHook()
        {
            _hook.enabled = false;
            SetConnectedBody(null);
            _hookIsReady = true;
        }
        private void SetConnectedBody(Rigidbody2D body) =>_hook.connectedBody = body;
        private void OnDestroy() => onUnconnectHook -= UnconnectHook;
    }
}