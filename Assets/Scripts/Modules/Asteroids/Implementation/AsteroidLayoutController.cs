using System;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.Asteroids.Implementation
{
    internal sealed class AsteroidLayoutController : MonoBehaviour
    {
        [field: SerializeField]
        public List<AsteroidSpawnPoint> SpawnPoints { get; private set; } = new();

        private void Reset()
        {
            SpawnPoints.Clear();
            var points = GetComponentsInChildren<AsteroidSpawnPoint>();
            SpawnPoints.AddRange(points);
        }
    }
}