using System;
using System.Threading.Tasks;
using Modules.Common;
using Modules.Player.Implementation.Handlers;
using UnityEngine;

namespace Modules.Player.Implementation
{
    internal sealed class PlayerService : MonoBehaviour, IPlayerService, IDisposable
    {
        [SerializeField] private PlayerController _playerPrefab;
        [SerializeField] private ProjectileController _projectilePrefab;
        
        private PlayerController _playerInstance;

        private InputSystem_Actions _inputActions;
        
        private PlayerMovementHandler _movementHandler;
        private ProjectileHandler _projectileHandler;

        private void Update()
        {
            if (_playerInstance == null)
            {
                return;
            }
            
            _movementHandler.Update();
            _projectileHandler.Update();
        }

        public void Dispose()
        {
            _projectileHandler.Dispose();
        }

        public Task InitializeAsync()
        {
            _inputActions = new InputSystem_Actions();
            
            _movementHandler = new PlayerMovementHandler(_inputActions);
            _projectileHandler = new ProjectileHandler(_inputActions, _projectilePrefab);
            
            return Task.CompletedTask;
        }

        public void StartRound()
        {
            _movementHandler.EnableInput();
        }
        
        public void SetupForNewRound()
        {
            _playerInstance = Instantiate(_playerPrefab);
            
            _movementHandler.Initialize(_playerInstance);
            _projectileHandler.Initialize(_playerInstance.transform);
        }

        public void FinishRound()
        {
            _movementHandler.DisableInput();
        }
    }
}