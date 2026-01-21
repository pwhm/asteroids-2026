using Modules.Common;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Modules.Player.Implementation.Handlers
{
    internal sealed class PlayerInputHandler
    {
        private InputSystem_Actions _inputActions;
        private PlayerController _player;
        
        public PlayerInputHandler()
        {
            _inputActions = new InputSystem_Actions();
            _inputActions.Player_1.Rotation.canceled += OnRotationCanceled;
            _inputActions.Player_1.Rotation.performed += OnRotationActionPerformed;
            EnableInput();
        }

        public void Update()
        {
            if (_inputActions.Player_1.Thrust.IsPressed())
            {
                _player.AddThrust();
            }
        }

        public void Initialize(PlayerController player)
        {
            _player = player;
        }

        public void CleanUp()
        {
            _inputActions.Player_1.Rotation.canceled -= OnRotationCanceled;
            _inputActions.Player_1.Rotation.performed -= OnRotationActionPerformed;
        }

        public void EnableInput()
        {
            _inputActions.Player_1.Enable();
        }

        public void DisableInput()
        {
            _inputActions.Player_1.Disable();
        }

        private void OnRotationActionPerformed(InputAction.CallbackContext context)
        {
            var rotationInput = context.ReadValue<Vector2>();
            _player.Rotate(-rotationInput.x);
        }

        private void OnRotationCanceled(InputAction.CallbackContext _)
        {
            _player.Rotate(0);
        }
    }
}