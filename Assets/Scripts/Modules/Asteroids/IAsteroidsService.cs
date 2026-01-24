using System;
using Core.Services;

namespace Modules.Asteroids
{
    public interface IAsteroidsService : IService, IDisposable
    {
        void SetupForNewRound();
        void StartRound();
    }
}