using System.Threading.Tasks;
using Modules.Common;
using UnityEngine.AddressableAssets;

namespace Modules.Asteroids.Implementation
{
    internal sealed class AsteroidsService : IAsteroidsService
    {
        public async Task InitializeAsync()
        {
            // Load all the prefabs
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
    }
}