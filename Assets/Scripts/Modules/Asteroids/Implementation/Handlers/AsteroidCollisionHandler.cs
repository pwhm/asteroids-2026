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

        public void ResolveCollision(AsteroidController asteroid, string tag)
        {
            Events.Gameplay.AsteroidDestroyed?.Invoke();
            
            if (asteroid.Type == AsteroidType.Small || tag == Constants.Gameplay.PLAYER_TAG)
            {
                _context.Spawner.ReleaseAsteroid(asteroid);
                return;
            }
            
            var type = asteroid.Type;
            var direction = asteroid.DirectionNormalized;
            var position = asteroid.Position;
            var speed = asteroid.Spped;
            
            _context.Spliter.ResolveSplit(type, direction, position, speed);
            _context.Spawner.ReleaseAsteroid(asteroid);
        }
    }
}