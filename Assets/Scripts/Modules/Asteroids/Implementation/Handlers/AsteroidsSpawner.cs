using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Services;
using Modules.Assets;
using Modules.Common;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace Modules.Asteroids.Implementation.Handlers
{
    internal sealed class AsteroidsSpawner : IDisposable
    {
        private readonly IAsteroidsServiceContext _context;
        
        private AsteroidController _largePrefab;
        private AsteroidController _mediumPrefab;
        private AsteroidController _smallPrefab;
        
        private ObjectPool<AsteroidController> _largeAsteroidPool;
        private ObjectPool<AsteroidController> _mediumAsteroidPool;
        private ObjectPool<AsteroidController> _smallAsteroidPool;

        private List<AsteroidController> _activeAsteroids = new();
        
        public AsteroidsSpawner(IAsteroidsServiceContext context)
        {
            _context = context;
        }
        
        public async Task InitializeAsync()
        {
            await LoadAsteroidPrefabs();
            SetUpAsteroidPools();
        }

        public void Dispose()
        {
            _largeAsteroidPool?.Dispose();
            _mediumAsteroidPool?.Dispose();
            _smallAsteroidPool?.Dispose();
        }

        public void SpawnAsteroidWave()
        {
            foreach (var spawnPoint in _context.CurrentLayout.SpawnPoints)
            {
                var type =  spawnPoint.Type;
                
                var asteroidInstance = GetAsteroidInstanceOfType(type);
                asteroidInstance.transform.position = spawnPoint.Position;
                asteroidInstance.Initialize();
            }
        }

        public AsteroidController GetAsteroidInstanceOfType(AsteroidType type)
        {
                var asteroid = type switch
                {
                    AsteroidType.Large => _largeAsteroidPool.Get(),
                    AsteroidType.Medium => _mediumAsteroidPool.Get(),
                    AsteroidType.Small => _smallAsteroidPool.Get(),
                    _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
                };
                
                _activeAsteroids.Add(asteroid);
                
                return asteroid;
        }

        public void ReleaseAsteroid(AsteroidController asteroid)
        {
            if (!_activeAsteroids.Contains(asteroid))
            {
                return;
            }
            _activeAsteroids.Remove(asteroid);
            switch (asteroid.Type)
            {
                case AsteroidType.Large:
                    _largeAsteroidPool.Release(asteroid);
                    break;
                case AsteroidType.Medium:
                    _mediumAsteroidPool.Release(asteroid);
                    break;
                case AsteroidType.Small:
                    _smallAsteroidPool.Release(asteroid);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(asteroid.Type), asteroid.Type, null);
            }
            Events.Gameplay.AsteroidDestroyed?.Invoke();

            if (_activeAsteroids.Count == 0)
            {
                Events.Gameplay.RoundCompleted?.Invoke();
            }
        }
        
        private async Task LoadAsteroidPrefabs()
        {
            const string KEY_FORMAT = "gameplay/asteroids/{0}";
            var assetService = Services.GetService<IAssetService>();

            var key = string.Format(KEY_FORMAT, nameof(AsteroidType.Large));
            _largePrefab = await assetService.LoadPrefab<AsteroidController>(key, Constants.Addressables.Tags.GAMEPLAY);

            key = string.Format(KEY_FORMAT, nameof(AsteroidType.Medium));
            _mediumPrefab= await assetService.LoadPrefab<AsteroidController>(key, Constants.Addressables.Tags.GAMEPLAY);

            key = string.Format(KEY_FORMAT, nameof(AsteroidType.Small));
            _smallPrefab = await assetService.LoadPrefab<AsteroidController>(key, Constants.Addressables.Tags.GAMEPLAY);
        }
        
        private void SetUpAsteroidPools()
        {
            _largeAsteroidPool = new ObjectPool<AsteroidController>(
                () => AsteroidCreate(AsteroidType.Large),
                OnAsteroidGet,
                OnAsteroidRelease);
            
            _mediumAsteroidPool= new ObjectPool<AsteroidController>(
                () => AsteroidCreate(AsteroidType.Medium),
                OnAsteroidGet,
                OnAsteroidRelease);
            
            _smallAsteroidPool = new ObjectPool<AsteroidController>(
                () => AsteroidCreate(AsteroidType.Small),
                OnAsteroidGet,
                OnAsteroidRelease);
        }
        
        private AsteroidController AsteroidCreate(AsteroidType asteroidType)
        {
            var prefab = GetAsteroidPrefabOfType(asteroidType);
            
            var instance = Object.Instantiate(prefab, Vector3.zero, Quaternion.identity);
            
            return instance;
        }

        private void OnAsteroidGet(AsteroidController asteroid)
        {
            asteroid.gameObject.SetActive(true);
            asteroid.Collided += _context.AsteroidCollided;
        }
        
        private void OnAsteroidRelease(AsteroidController asteroid)
        {
            asteroid.gameObject.SetActive(false);
            
            asteroid.Collided -= _context.AsteroidCollided;
        }

        private AsteroidController GetAsteroidPrefabOfType(AsteroidType type)
        {
            return type switch
            {
                AsteroidType.Large => _largePrefab,
                AsteroidType.Medium => _mediumPrefab,
                AsteroidType.Small => _smallPrefab,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
    }
}