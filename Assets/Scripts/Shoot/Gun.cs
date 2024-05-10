using System;
using Melee;
using UnityEngine;

namespace Shoot
{
    public class Gun : Weapon
    {
        [SerializeField] private BulletPool bulletPool;
        [SerializeField] private float shootForce;

        private void Update()
        {
            if (Input.GetMouseButtonUp(0)) Shoot();
        }

        private void Shoot()
        {
            if (!bulletPool.HasBullets() || _isCooldown) return;
            
            var bullet = bulletPool.GetBullet().GetComponent<Bullet>();
            bullet.transform.position = transform.position;
            bullet.Damage = this.Damage;  
            
            AnimateSword();
            
            bullet.Launch(GetDirection(), shootForce);

            StartCoroutine(Cooldown());
        }
        
        private Vector2 GetDirection()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var plane = new Plane(transform.forward, transform.position);

            Vector3 hit = Vector3.zero;

            if (plane.Raycast(ray, out var enter))
                hit = ray.direction;

            return hit;
        }
    }
}