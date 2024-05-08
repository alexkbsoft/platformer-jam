using UnityEngine;

namespace Shoot
{
    public class Gun : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [SerializeField] private BulletPool bulletPool;
        [SerializeField] private float shootForce;
        
        private void Update()
        {
            if (Input.GetMouseButtonUp(0)) Shoot();
        }

        private void Shoot()
        {
            if (!bulletPool.HasBullets()) return;
            
            var bullet = bulletPool.GetBullet();
            bullet.transform.position = transform.position;
            bullet.GetComponent<Bullet>().Launch(GetDirection(), shootForce);
        }

        private Vector2 GetDirection() => transform.right * player.transform.localScale.x; //
    }
}