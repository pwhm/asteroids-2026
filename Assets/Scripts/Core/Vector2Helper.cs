using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.Extensions
{
    public static class Vector2Helper
    {
        // Naming isn't a perfect here, but for the life of me can't come up with a better one
        // right now
        public static Vector2 GetRandomNormalized(int precision = 10)
        {
            var randX = (float)Random.Range(-precision, precision + 1);
            var randY = (float)Random.Range(-precision, precision + 1);

            return new Vector2(randX / precision, randY / precision);
        }
    }
}