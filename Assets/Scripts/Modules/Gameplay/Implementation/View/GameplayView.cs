using Core.Services;
using Modules.Common;
using Modules.User;
using UnityEngine;

namespace Modules.Gameplay.Implementation.View
{
    internal class GameplayView : MonoBehaviour
    {
        private IUserSessionStateService _userSessionStateService => Services.GetService<IUserSessionStateService>();
        
        [SerializeField] private LifeCounterController _lifeCounter;
        [SerializeField] private GameObject _gameOverContainer;

        private void Awake()
        {
            _gameOverContainer.SetActive(false);
            _lifeCounter.Refresh(0);
        }
        
        private void OnEnable()
        {
            Events.Gameplay.RoundStarted += OnRoundStarted;
            Events.Gameplay.PlayerLostLife +=  RefreshLifeCounter;
            Events.Gameplay.GameOver += OnGameOver;
        }

        private void OnDisable()
        {
            Events.Gameplay.RoundStarted -= OnRoundStarted;
            Events.Gameplay.PlayerLostLife -=  RefreshLifeCounter;
            Events.Gameplay.GameOver -= OnGameOver;
        }

        private void OnRoundStarted()
        {
            RefreshLifeCounter();
        }

        private void RefreshLifeCounter()
        {
            var lives = _userSessionStateService.Session.Lives;
            _lifeCounter.Refresh(lives);
        }

        private void OnGameOver()
        {
            _gameOverContainer.SetActive(true);
        }
    }
}