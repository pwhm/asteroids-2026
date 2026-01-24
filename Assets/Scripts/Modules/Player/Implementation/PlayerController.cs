using System;
using Modules.Common;
using UnityEngine;

namespace Modules.Player.Implementation
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private float _thrustForce = 10.0f;
        [SerializeField] private float _rotationSpeed = 10.0f;

        public void AddThrust()
        {
            _rigidbody.AddForce(transform.up * _thrustForce);
        }

        public void Rotate(float direction)
        {
            _rigidbody.angularVelocity = _rotationSpeed * direction;
        }
    }
}