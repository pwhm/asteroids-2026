using Modules.Common;
using Modules.Player.Implementation.Handlers;
using UnityEngine;

namespace Modules.Player.Implementation
{
    internal sealed class PlayerService : MonoBehaviour, IPlayerService
    {
        [SerializeField] private PlayerController _playerPrefab;

        [SerializeField] private PlayerController _playerInstance;
        
        private PlayerInputHandler _inputHandler;

        private void OnEnable()
        {
            Initialize();
        }
        
        private void Update()
        {
            if (_playerInstance == null)
            {
                return;
            }
            
            _inputHandler.Update();
        }

        public void Initialize()
        {
            _inputHandler = new PlayerInputHandler();
            _inputHandler.Initialize(_playerInstance);
        }

        public void StartRound()
        {
            _inputHandler.EnableInput();
        }
        
        public void SetupForNewRound()
        {
            _playerInstance = Instantiate(_playerPrefab, transform);
        }

        public void FinishRound()
        {
            _inputHandler.DisableInput();
        }
    }


}