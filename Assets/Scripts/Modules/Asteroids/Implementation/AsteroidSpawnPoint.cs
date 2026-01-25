using UnityEngine;

namespace Modules.Asteroids.Implementation
{
    internal sealed class AsteroidSpawnPoint : MonoBehaviour
    {
        [field: SerializeField] public AsteroidType Type { get; private set; }
        
        public Vector3 Position => transform.position;
    }
}