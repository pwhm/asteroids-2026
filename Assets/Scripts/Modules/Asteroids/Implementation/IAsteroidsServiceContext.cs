using System;
using Modules.Asteroids.Implementation.Handlers;

namespace Modules.Asteroids.Implementation
{
    internal interface IAsteroidsServiceContext
    {
        public AsteroidLayoutController CurrentLayout { get; set; }
        public Action<AsteroidController> AsteroidCollided { get; }

        public AsteroidsSpawner Spawner { get; }

    }
}