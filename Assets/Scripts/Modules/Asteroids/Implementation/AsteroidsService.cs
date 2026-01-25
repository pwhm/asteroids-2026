using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Services;
using Modules.Assets;
using Modules.Common;

namespace Modules.Asteroids.Implementation
{
    internal sealed class AsteroidsService : IAsteroidsService
    {
        private AsteroidController _largePrefab;
        private List<AsteroidLayoutController> _layoutPrefabs = new();
        
        public async Task InitializeAsync()
        {
            await LoadPrefabs();
            // Create pools
            // Create layouts
        }

        public void Dispose()
        {
            // Clear and Dispose Pools
            // Dispose 
        }

        public void SetupForNewRound()
        {
            // pick layout
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
    }
}