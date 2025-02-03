using System;
using Subsystems;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityInputAction;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace Game.Input
{
    public sealed class InputReader : GameSubsystem<InputReader>, Controls.IPlayerActions
    {
        public event Action<bool> OnPlayerPrimaryAttack;
        public event Action<bool> OnPlayerSecondaryAttack;
        public event Action<Vector2> OnPlayerMove;
        public event Action<Vector2> OnPlayerAim;

        private Controls _controls;

        public InputType InputType { get; private set; }
        private InputDevice _lastAimDevice;

        public override void OnAwake()
        {
            if (_controls == null)
            {
                _controls = new Controls();
                _controls.Player.SetCallbacks(this);
            }

            _controls.Player.Enable();
        }

        public override void OnDestroy()
        {
            _controls.Player.Disable();
        }

        public void OnMove(CallbackContext context)
        {
            var direction = context.ReadValue<Vector2>();
            OnPlayerMove?.Invoke(direction);
        }

        public void OnAim(CallbackContext context)
        {
            if (context.action.activeControl.device != _lastAimDevice)
            {
                _lastAimDevice = context.action.activeControl.device;
                InputType = _lastAimDevice is Gamepad
                    ? InputType.Gamepad
                    : InputType.Keyboard;
            }

            var direction = context.ReadValue<Vector2>();
            OnPlayerAim?.Invoke(direction);
        }

        public void OnAttack(CallbackContext context)
        {
            if (context.performed)
                OnPlayerPrimaryAttack?.Invoke(true);
            else if (context.canceled)
                OnPlayerPrimaryAttack?.Invoke(false);
        }

        public void OnSecondaryAttack(CallbackContext context)
        {
            if (context.performed)
                OnPlayerSecondaryAttack?.Invoke(true);
            else if (context.canceled)
                OnPlayerSecondaryAttack?.Invoke(false);
        }

        public void OnInteract(CallbackContext context)
        {
            throw new System.NotImplementedException();
        }

        public void OnDash(CallbackContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}