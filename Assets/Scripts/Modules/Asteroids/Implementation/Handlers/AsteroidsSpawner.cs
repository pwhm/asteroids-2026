using System;
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

        public void SpawnAsteroids()
        {
            foreach (var spawnPoint in _context.CurrentLayout.SpawnPoints)
            {
                var type =  spawnPoint.Type;
                var asteroidInstance = type switch
                {
                    AsteroidType.Large => _largeAsteroidPool.Get(),
                    AsteroidType.Medium => _mediumAsteroidPool.Get(),
                    AsteroidType.Small => _smallAsteroidPool.Get(),
                    _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
                };
                
                asteroidInstance.transform.position = spawnPoint.Position;
                asteroidInstance.Initialize();
            }
        }

        public void ReleaseAsteroid(AsteroidController asteroid)
        {
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
                AsteroidGet,
                AsteroidRelease);
            
            _mediumAsteroidPool= new ObjectPool<AsteroidController>(
                () => AsteroidCreate(AsteroidType.Medium),
                AsteroidGet,
                AsteroidRelease);
            
            _smallAsteroidPool = new ObjectPool<AsteroidController>(
                () => AsteroidCreate(AsteroidType.Small),
                AsteroidGet,
                AsteroidRelease);
        }
        
        private AsteroidController AsteroidCreate(AsteroidType asteroidType)
        {
            var prefab = GetAsteroidOfType(asteroidType);
            
            var instance = Object.Instantiate(prefab, Vector3.zero, Quaternion.identity);
            
            return instance;
        }

        private void AsteroidGet(AsteroidController asteroid)
        {
            asteroid.gameObject.SetActive(true);
            asteroid.Collided += _context.AsteroidCollided;
        }
        
        private void AsteroidRelease(AsteroidController asteroid)
        {
            asteroid.gameObject.SetActive(false);
            
            asteroid.Collided -= _context.AsteroidCollided;
        }

        private AsteroidController GetAsteroidOfType(AsteroidType asteroidType)
        {
            return asteroidType switch
            {
                AsteroidType.Large => _largePrefab,
                AsteroidType.Medium => _mediumPrefab,
                AsteroidType.Small => _smallPrefab,
                _ => throw new ArgumentOutOfRangeException(nameof(asteroidType), asteroidType, null)
            };
        }
    }
}