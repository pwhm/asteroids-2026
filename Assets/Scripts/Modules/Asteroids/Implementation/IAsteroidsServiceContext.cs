using System;
using Modules.Asteroids.Implementation.Handlers;

namespace Modules.Asteroids.Implementation
{
    internal interface IAsteroidsServiceContext
    {
        public AsteroidLayoutController CurrentLayout { get; set; }
        public Action<AsteroidController> AsteroidCollided { get; set; }

        public AsteroidsSpawner Spawner { get; }
        public AsteroidSplitHandler Spliter { get; }

    }
}