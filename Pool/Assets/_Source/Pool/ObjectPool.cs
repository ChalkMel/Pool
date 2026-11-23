using System.Collections.Generic;

namespace Pool
{
    public class ObjectPool<T> : IObjectPool<T> where T : IPoolableItem
    {
        private readonly Queue<T> _poolableObjects;
        private readonly IFactory<T> _factory;
        private int _totalCreatedCount; 
    
        private readonly int _maxPoolSize = 8;
        private readonly int _startPoolSize = 3;

        public ObjectPool(IFactory<T> factory)
        {
            _factory = factory;
        
            _poolableObjects = new Queue<T>();
            _totalCreatedCount = 0;
        
            Init();
        }

        private void Init()
        {
            for (int i = 0; i < _startPoolSize; i++)
            {
                T item = _factory.Create();
                item.Deactivate(); 
                _poolableObjects.Enqueue(item);
                _totalCreatedCount++;
            }
        }
    
        public bool TryGet(out T item)
        {
            item = default;
        
            if (_poolableObjects.Count > 0)
            {
                item = _poolableObjects.Dequeue();
            }
            else if (_totalCreatedCount < _maxPoolSize)
            {
                item = _factory.Create();
                _totalCreatedCount++;
            }
            else
            {
                return false; 
            }

            var poolableItem = item;
            item.SetCallback(() => Return(poolableItem));
            item.Activate();
        
            return true;
        }

        public void Return(T poolableObject)
        {
            _poolableObjects.Enqueue(poolableObject);
        }
    }
}