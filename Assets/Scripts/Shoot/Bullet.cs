using UnityEngine;

namespace Shoot
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Bullet : Damageable
    {
        private BulletPool _pool;
        private Rigidbody2D _rbody;
        
        public void InitBullet(BulletPool pool)
        {
            _pool = pool;
            _rbody = GetComponent<Rigidbody2D>();
        }

        public void Launch(Vector2 direction, float force) => _rbody.AddForce(direction * force * _rbody.mass, ForceMode2D.Impulse);
        
        protected override void DoDamage()
        {
            base.DoDamage();
            _pool.AddBullet(gameObject);
        }
    }
}