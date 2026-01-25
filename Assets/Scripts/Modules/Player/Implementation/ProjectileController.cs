using System;
using UnityEngine;

namespace Modules.Player.Implementation
{
    [RequireComponent(typeof(Rigidbody2D))]
    internal sealed class ProjectileController : MonoBehaviour
    {
        public event Action<ProjectileController> Collided;
        
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private float _speed = 15;

        private void Reset()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        public void Initialize(Vector3 direction, Vector3 position)
        {
            _rigidbody.linearVelocity = direction * _speed;
            transform.position = position + direction.normalized * 0.25f;
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            Collided?.Invoke(this);
        }
    }
}