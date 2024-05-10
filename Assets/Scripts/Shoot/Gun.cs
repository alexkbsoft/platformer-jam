using System;
using Melee;
using UnityEngine;

namespace Shoot
{
    public class Gun : Weapon
    {
        [SerializeField] private BulletPool bulletPool;
        [SerializeField] private float shootForce, rotationSpeed;
        [SerializeField] private Transform gunPivot, character;

        private Vector3 targetPosition;
        
        private void Update()
        {
            SetTargetPosition();
            RotateGun();

            if (Input.GetMouseButtonUp(0)) Shoot();
        }

        private void Shoot()
        {
            if (!bulletPool.HasBullets() || _isCooldown) return;

            var bullet = bulletPool.GetBullet().GetComponent<Bullet>();
            bullet.transform.position = transform.position;
            bullet.Damage = this.Damage;

            AnimateSword();

            bullet.Launch(transform.right, shootForce);

            StartCoroutine(Cooldown());
        }

        private void SetTargetPosition()
        {
            targetPosition = Input.mousePosition;
            targetPosition.z = Vector3.Distance(transform.position, Camera.main.transform.position);
            targetPosition = Camera.main.ScreenToWorldPoint(targetPosition);
            targetPosition.z = 0f; // Учитываем, что пушка находится в плоскости xy
        }

        private void RotateGun()
        {
            Vector3 direction = targetPosition - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
            gunPivot.transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
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