using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Services;
using Modules.Assets;
using Modules.Common;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace Modules.Asteroids.Implementation
{
    internal sealed class AsteroidsService : IAsteroidsService, IDisposable
    {
        private AsteroidController _largePrefab;
        private AsteroidController _mediumPrefab;
        private AsteroidController _smallPrefab;
        
        private readonly List<AsteroidLayoutController> _layoutPrefabs = new();
        private Dictionary<int, AsteroidLayoutController> _loadedLayouts = new();
        private AsteroidLayoutController _activeLayout;
        
        private ObjectPool<AsteroidController> _largeAsteroidPool;
        private ObjectPool<AsteroidController> _mediumAsteroidPool;
        private ObjectPool<AsteroidController> _smallAsteroidPool;

        private readonly List<AsteroidController> _activeAsteroids = new();
        
        public async Task InitializeAsync()
        {
            await LoadPrefabs();
            SetUpAsteroidPools();
        }

        public void Dispose()
        {
            _largeAsteroidPool.Dispose();
            _mediumAsteroidPool.Dispose();
            _smallAsteroidPool.Dispose();
        }

        public void SetupForNewRound(int layoutIndex)
        {
            PrepareAsteroidsLayout(layoutIndex);
        }

        public void StartRound()
        {
            SpawnAsteroids();
        }

        private async Task LoadPrefabs()
        {
            await LoadAsteroids();
            await LoadLayouts();
        }
        
        // ToDo: Move to a helper class of sorts
        private async Task LoadAsteroids()
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

        private async Task LoadLayouts()
        {
            const string KEY_FORMAT = "gameplay/asteroids/layouts/{0}";
            var assetService = Services.GetService<IAssetService>();

            // Normally it would be data driven... some meta json file or scriptable object
            for (var i = 0; i < 2; i++)
            {
                var key = string.Format(KEY_FORMAT, i);
                var prefab = await assetService.LoadPrefab<AsteroidLayoutController>(key, Constants.Addressables.Tags.GAMEPLAY);
                _layoutPrefabs.Add(prefab);
            }
        }

        private void SetUpAsteroidPools()
        {
            _largeAsteroidPool = new ObjectPool<AsteroidController>(
                () => OnAsteroidCreate(AsteroidType.Large),
                OnAsteroidGet,
                OnAsteroidRelease);
            
            _mediumAsteroidPool= new ObjectPool<AsteroidController>(
                () => OnAsteroidCreate(AsteroidType.Medium),
                OnAsteroidGet,
                OnAsteroidRelease);
            
            _smallAsteroidPool = new ObjectPool<AsteroidController>(
                () => OnAsteroidCreate(AsteroidType.Small),
                OnAsteroidGet,
                OnAsteroidRelease);
        }

        private AsteroidController OnAsteroidCreate(AsteroidType asteroidType)
        {
            var prefab = GetAsteroidOfType(asteroidType);
            
            var instance = Object.Instantiate(prefab, Vector3.zero, Quaternion.identity);
            
            return instance;
        }

        private void OnAsteroidGet(AsteroidController asteroid)
        {
            asteroid.gameObject.SetActive(true);
            _activeAsteroids.Add(asteroid);
        }
        
        private void OnAsteroidRelease(AsteroidController asteroid)
        {
            asteroid.gameObject.SetActive(false);
            _activeAsteroids.Remove(asteroid);
            
            asteroid.Collided -= OnAsteroidCollided;
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

        private void PrepareAsteroidsLayout(int layoutIndex)
        {
            if (_loadedLayouts.TryGetValue(layoutIndex, out var layout))
            {
                layout.gameObject.SetActive(true);
                SetCurrentLayout(layout);
                return;
            }
            
            layout = Object.Instantiate(_layoutPrefabs[layoutIndex], Vector3.zero, Quaternion.identity);
            _loadedLayouts.Add(layoutIndex, layout);
            SetCurrentLayout(layout);
        }

        private void SetCurrentLayout(AsteroidLayoutController layout)
        {
            if (_activeLayout != null)
            {
                _activeLayout.gameObject.SetActive(false);
            }

            _activeLayout = layout;
        }

        private void SpawnAsteroids()
        {
            foreach (var spawnPoint in _activeLayout.SpawnPoints)
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
                asteroidInstance.Collided += OnAsteroidCollided;
            }
        }

        private void OnAsteroidCollided(AsteroidController asteroid)
        {
            ReleaseAsteroid(asteroid);
        }

        private void ReleaseAsteroid(AsteroidController asteroid)
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
    }
}