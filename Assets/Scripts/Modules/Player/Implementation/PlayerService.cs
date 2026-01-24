using System.Threading.Tasks;
using Modules.Common;
using Modules.Player.Implementation.Handlers;
using UnityEngine;

namespace Modules.Player.Implementation
{
    internal sealed class PlayerService : MonoBehaviour, IPlayerService
    {
        [SerializeField] private PlayerController _playerPrefab;
        
        private PlayerController _playerInstance;
        
        private PlayerInputHandler _inputHandler;

        private void Update()
        {
            if (_playerInstance == null)
            {
                return;
            }
            
            _inputHandler.Update();
        }

        public Task InitializeAsync()
        {
            _inputHandler = new PlayerInputHandler();
            
            return Task.CompletedTask;
        }

        public void StartRound()
        {
            _inputHandler.EnableInput();
        }
        
        public void SetupForNewRound()
        {
            _playerInstance = Instantiate(_playerPrefab);
            _inputHandler.Initialize(_playerInstance);
        }

        public void FinishRound()
        {
            _inputHandler.DisableInput();
        }
    }
}