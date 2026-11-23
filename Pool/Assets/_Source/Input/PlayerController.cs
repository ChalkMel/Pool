using Pool;
using UnityEngine;

namespace Input
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private Transform firePoint;
    
        private IObjectPool<Bullet> _bulletPool;
        private Rigidbody2D _rb;
        public Transform  FirePoint => firePoint;
    
        private const float BULLET_ROTATION = 90;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }
    
        public void Initialize(IObjectPool<Bullet> bulletPool)
        {
            _bulletPool = bulletPool;
        }

        public void Move(Vector2 direction)
        {
            _rb.linearVelocity = direction * moveSpeed;
            transform.up = direction;
        }

        public void Shoot(Vector2 direction)
        {
            if (_bulletPool.TryGet(out Bullet bullet))
            {
                bullet.transform.SetPositionAndRotation(firePoint.position, Quaternion.identity);
                bullet.transform.up = direction;
                bullet.Activate();
                bullet.Launch(direction);
            }
            else Debug.Log("пул переполнен");
        }
    }
}