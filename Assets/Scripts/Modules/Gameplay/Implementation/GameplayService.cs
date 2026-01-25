using System.Threading.Tasks;
using Core.Services;
using Modules.Asteroids;
using Modules.Player;

namespace Modules.Gameplay.Implementation
{
    internal sealed class GameplayService : IGameplayService
    {
        private IPlayerService _playerService => Services.GetService<IPlayerService>();
        private IAsteroidsService _asteroidsService => Services.GetService<IAsteroidsService>();
        
        public Task InitializeAsync()
        {
            return Task.CompletedTask;
        }
        
        public void SwitchTo()
        {
        }

        public void StartRound()
        {
            _playerService.SetupForNewRound();
            _asteroidsService.SetupForNewRound(0);
            
            _playerService.StartRound();
            _asteroidsService.StartRound();
        }
    }
}