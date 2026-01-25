using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Services;
using Modules.Assets;
using Modules.Asteroids.Implementation.Handlers;
using Modules.Common;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace Modules.Asteroids.Implementation
{
    internal sealed class AsteroidsService : IAsteroidsService, IDisposable, IAsteroidsServiceContext
    {
        public AsteroidLayoutController CurrentLayout { get; set; }
        public Action<AsteroidController, string> AsteroidCollided { get; set; }

        private AsteroidsLayoutLoader _layoutLoader;
        private AsteroidCollisionHandler _collisionHandler;
        public AsteroidsSpawner Spawner { get; private set; }
        public AsteroidSplitHandler Spliter { get; private set; }
        
        public async Task InitializeAsync()
        {
            _layoutLoader = new AsteroidsLayoutLoader(this);
            Spawner = new AsteroidsSpawner(this);
            _collisionHandler = new AsteroidCollisionHandler(this);
            Spliter = new AsteroidSplitHandler(this);

            await _layoutLoader.InitializeAsync();
            await Spawner.InitializeAsync();

            AsteroidCollided += _collisionHandler.ResolveCollision;
        }

        public void Dispose()
        {
            Spawner?.Dispose();
        }

        public void SetupForNewRound(int roundNumber)
        {
            _layoutLoader.PrepareAsteroidsLayout(roundNumber % _layoutLoader.AvailableLayoutsCount % 2);
        }

        public void StartRound()
        {
            Spawner.SpawnAsteroidWave();
        }
    }
}