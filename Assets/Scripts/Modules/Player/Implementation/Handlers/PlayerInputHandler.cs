using Modules.Common;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Modules.Player.Implementation.Handlers
{
    internal sealed class PlayerInputHandler
    {
        private readonly InputSystem_Actions _inputActions = new();
        private PlayerController _player;

        public void Update()
        {
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