using System;
using Core.Extensions;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Modules.Asteroids.Implementation
{
    [RequireComponent(typeof(Rigidbody2D))]
    internal sealed class AsteroidController : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private float speed  = 10;
        [SerializeField] private float rotationSpeed = 3;

        private void Reset()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        public void Initialize()
        {
            _rigidbody2D.linearVelocity = Vector2Helper.GetRandomNormalized(25) * speed;
            _rigidbody2D.angularVelocity = Random.Range(-10, 11) / 10.0f * rotationSpeed;
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log($"Asteroid hit {other.gameObject.name}");
        }
    }
}