using System;

namespace Modules.Common
{
    public static class Events
    {
        public static class Gameplay
        {
            public static Action AsteroidDestroyed;
            public static Action RoundCompleted;
            public static Action PlayerHit;
            public static Action PlayerLostLife;
            public static Action RoundStarted;
            public static Action GameOver;
        }
    }
}