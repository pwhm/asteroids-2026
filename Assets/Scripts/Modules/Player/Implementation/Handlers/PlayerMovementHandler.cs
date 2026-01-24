using Modules.Common;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Modules.Player.Implementation.Handlers
{
    internal sealed class PlayerMovementHandler
    {
        private InputSystem_Actions _inputActions;
        private PlayerController _player;

        public PlayerMovementHandler(InputSystem_Actions inputActions)
        {
            _inputActions = inputActions;
        }
        
        public void Update()
        {
            if (_inputActions == null)
            {
                return;
            }
            
            if (_inputActions.Player_1.Thrust.IsPressed())
            {
                _player.AddThrust();
            }

            if (_inputActions.Player_1.Rotation.IsPressed())
            {
                _player.Rotate(-_inputActions.Player_1.Rotation.ReadValue<Vector2>().x);
            }
        }

        public void Initialize(PlayerController player)
        {
            _player = player;
        }

        public void EnableInput()
        {
            _inputActions.Player_1.Enable();
        }

        public void DisableInput()
        {
            _inputActions.Player_1.Disable();
        }
    }
}