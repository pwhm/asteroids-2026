using System;
using Modules.Common;
using UnityEngine;

namespace Modules.Player.Implementation
{
    public class PlayerController : MonoBehaviour
    {
        public event Action<PlayerController> Collided;
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private float _thrustForce = 10.0f;
        [SerializeField] private float _rotationSpeed = 10.0f;

        public void AddThrust()
        {
            _rigidbody.AddForce(transform.up * _thrustForce);
        }

        public void Rotate(float direction)
        {
            _rigidbody.MoveRotation(_rigidbody.rotation + _rotationSpeed * direction);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Collided?.Invoke(this);
        }
    }
}