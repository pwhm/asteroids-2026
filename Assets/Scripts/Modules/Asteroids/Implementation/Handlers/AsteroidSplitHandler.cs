using UnityEngine;

namespace Modules.Asteroids.Implementation.Handlers
{
    internal sealed class AsteroidSplitHandler
    {
        private IAsteroidsServiceContext _context;

        public AsteroidSplitHandler(IAsteroidsServiceContext context)
        {
            _context = context;
        }

        public void ResolveSplit(AsteroidType type, Vector2 directionNormalised, Vector2 position, float speed)
        {
            var nextType = type + 1;
            var a = _context.Spawner.GetAsteroidInstanceOfType(nextType);
            var b = _context.Spawner.GetAsteroidInstanceOfType(nextType);

            var aOffset = new Vector2(directionNormalised.y, -directionNormalised.x);
            var bOffset = new Vector2(-directionNormalised.y, directionNormalised.x);

            a.Initialize(position + aOffset, aOffset);
            b.Initialize(position + bOffset, bOffset);
        }
    }
}