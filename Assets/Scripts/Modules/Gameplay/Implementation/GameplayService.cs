using System;
using System.Threading.Tasks;
using Core.Services;
using Modules.Asteroids;
using Modules.Common;
using Modules.Player;
using Modules.User;
using UnityEngine;

namespace Modules.Gameplay.Implementation
{
    internal sealed class GameplayService : IGameplayService
    {
        private IPlayerService _playerService => Services.GetService<IPlayerService>();
        private IAsteroidsService _asteroidsService => Services.GetService<IAsteroidsService>();
        private IUserSessionStateService _sessionStateService => Services.GetService<IUserSessionStateService>();
        
        public Task InitializeAsync()
        {
            Events.Gameplay.RoundCompleted +=  OnRoundCompleted;
            Events.Gameplay.PlayerHit += OnPlayerHit;
            return Task.CompletedTask;
        }

        public void SwitchTo()
        {
        }

        public void StartSession()
        {
            _sessionStateService.StartNewSession();
            StartRound();
        }

        private void StartRound()
        {
            var roundNumber = _sessionStateService.Session.RoundNumber;
            Debug.Log($"#Gameplay# Starting round {roundNumber}");
            
            _sessionStateService.StartNewSession();
            _playerService.SetupForNewRound();
            _asteroidsService.SetupForNewRound(roundNumber);
            
            _playerService.StartRound();
            _asteroidsService.StartRound();
            Events.Gameplay.RoundStarted?.Invoke();
        }

        private void OnRoundCompleted()
        {
            _sessionStateService.Session.StartNewRound();
            StartRound();
        }

        private void OnPlayerHit()
        {
            _sessionStateService.Session.DeductLife();
            Events.Gameplay.PlayerLostLife?.Invoke();
            // Update UI
        }
    }
}