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
            // spawn asteroids
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
            _largePrefab = await assetService.LoadPrefab<AsteroidController>(key, Constants.Addressables.Tags.GAMEPLAY);

            key = string.Format(KEY_FORMAT, nameof(AsteroidType.Small));
            _largePrefab = await assetService.LoadPrefab<AsteroidController>(key, Constants.Addressables.Tags.GAMEPLAY);
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
            
            _largeAsteroidPool = new ObjectPool<AsteroidController>(
                () => OnAsteroidCreate(AsteroidType.Medium),
                OnAsteroidGet,
                OnAsteroidRelease);
            
            _largeAsteroidPool = new ObjectPool<AsteroidController>(
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
    }
}