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
        public Action<AsteroidController> AsteroidCollided { get; set; }

        private AsteroidsLayoutLoader _layoutLoader;
        public AsteroidsSpawner Spawner { get; private set; }
        
        public async Task InitializeAsync()
        {
            _layoutLoader = new AsteroidsLayoutLoader(this);
            Spawner = new AsteroidsSpawner(this);

            await _layoutLoader.InitializeAsync();
            await Spawner.InitializeAsync();
        }

        public void Dispose()
        {
            Spawner?.Dispose();
        }

        public void SetupForNewRound(int layoutIndex)
        {
            _layoutLoader.PrepareAsteroidsLayout(layoutIndex);
        }

        public void StartRound()
        {
            Spawner.SpawnAsteroids();
        }
    }
}