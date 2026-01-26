using System;
using Core.Extensions;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Modules.Asteroids.Implementation
{
    [RequireComponent(typeof(Rigidbody2D))]
    internal sealed class AsteroidController : MonoBehaviour
    {
        public event Action<AsteroidController, string> Collided;
        
        [field:SerializeField] public AsteroidType Type { get; private set; }

        [Space]
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private float speed  = 10;
        [SerializeField] private float rotationSpeed = 3;

        public Vector2 DirectionNormalized => _rigidbody2D.linearVelocity.normalized;
        public Vector3 Position => transform.position;
        public float Spped =>  _rigidbody2D.linearVelocity.magnitude;

        private void Reset()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        public void Initialize()
        {
            _rigidbody2D.linearVelocity = Vector2Helper.GetRandomNormalized(25) * speed;
            _rigidbody2D.angularVelocity = Random.Range(-10, 11) / 10.0f * rotationSpeed;
        }

        public void Initialize(Vector3 position, Vector2 direction)
        {
            transform.position = position;
            _rigidbody2D.linearVelocity = direction * speed;
            _rigidbody2D.angularVelocity = Random.Range(-10, 11) / 10.0f * rotationSpeed;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Collided?.Invoke(this, other.tag);
        }
    }
}