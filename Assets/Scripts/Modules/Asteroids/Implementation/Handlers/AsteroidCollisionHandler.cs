using Modules.Common;

namespace Modules.Asteroids.Implementation.Handlers
{
    internal sealed class AsteroidCollisionHandler
    {
        private IAsteroidsServiceContext _context;

        public AsteroidCollisionHandler(IAsteroidsServiceContext context)
        {
            _context = context;
        }
    }
}