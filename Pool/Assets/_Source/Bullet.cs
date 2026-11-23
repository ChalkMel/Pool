using System;
using Pool;
using UnityEngine;

public class Bullet : MonoBehaviour, IPoolableItem
{
    private Action _poolCallback;
    private Rigidbody2D _rb;
    [SerializeField] private float lifetime = 2f;
    [SerializeField] private float speed = 10f;

    public void SetCallback(Action callback) => _poolCallback = callback;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }
    
    public void Launch(Vector2 direction)
    {
        _rb.linearVelocity = direction * speed;
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        CancelInvoke();
        Invoke(nameof(Disable), lifetime);
    }

    private void OnDisable()
    {
        CancelInvoke();
        _rb.linearVelocity = Vector2.zero;
        _poolCallback?.Invoke();
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        gameObject.SetActive(false);
    }
}