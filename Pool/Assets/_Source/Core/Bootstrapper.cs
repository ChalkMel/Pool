using Input;
using Pool;
using UnityEngine;

namespace Core
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private Bullet bulletPrefab;
        [SerializeField] private PlayerController playerController;
    
        public IObjectPool<Bullet> BulletPool { get; private set; }

        private void Awake()
        {
            IFactory<Bullet> factory = new Factory<Bullet>(bulletPrefab);
            BulletPool = new ObjectPool<Bullet>(factory);
        
            playerController.Initialize(BulletPool);
        }
    }
}