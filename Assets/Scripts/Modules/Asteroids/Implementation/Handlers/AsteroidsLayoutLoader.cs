using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Services;
using Modules.Assets;
using Modules.Common;
using UnityEngine;

namespace Modules.Asteroids.Implementation.Handlers
{
    internal sealed class AsteroidsLayoutLoader
    {
        private IAsteroidsServiceContext _context;
        
        private readonly List<AsteroidLayoutController> _layoutPrefabs = new();
        private readonly Dictionary<int, AsteroidLayoutController> _loadedLayouts = new();

        public AsteroidsLayoutLoader(IAsteroidsServiceContext context)
        {
            _context = context;
        }
        
        public int AvailableLayoutsCount => _layoutPrefabs.Count;
        
        public async Task InitializeAsync()
        {
            await LoadLayouts();
        }
        
        public void PrepareAsteroidsLayout(int layoutIndex)
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
        
        private void SetCurrentLayout(AsteroidLayoutController layout)
        {
            if (_context.CurrentLayout != null)
            {
                _context.CurrentLayout.gameObject.SetActive(false);
            }

            _context.CurrentLayout = layout;
        }
    }
}