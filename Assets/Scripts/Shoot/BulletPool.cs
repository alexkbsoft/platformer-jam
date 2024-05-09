using System.Collections.Generic;
using UnityEngine;

namespace Shoot
{
    public class BulletPool : MonoBehaviour 
    {
                
        [SerializeField] private int bulletCount;
        [SerializeField] private GameObject bulletPrefab;
        
        private Queue<GameObject> _pool = new Queue<GameObject>();
        
        private void Awake() => CreateBullets();
        
        private void CreateBullets()
        {
            for (int i = 0; i < bulletCount; i++)
            {
                var bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity, transform);
                bullet.GetComponent<Bullet>().InitBullet(this);
                AddBullet(bullet);
            }
        }

        public void AddBullet(GameObject bullet)
        {
            _pool.Enqueue(bullet);
            bullet.SetActive(false);
        }

        public GameObject GetBullet()
        {
            var bullet = _pool.Dequeue();
            bullet.SetActive(true);

            return bullet;
        }

        public bool HasBullets() => _pool.Count > 0;
    }
}