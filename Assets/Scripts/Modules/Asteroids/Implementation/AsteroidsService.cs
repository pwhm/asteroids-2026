using System.Threading.Tasks;

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