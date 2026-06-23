using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CardMiniGame.Input
{
    public class MobileInputController : MonoBehaviour
    {
        public event Action<Vector2> OnTap;
        public event Action<Vector2, Vector2, bool> OnDrag;

        private InputActions inputActions;
        private InputActionMap mobileMap;
        private InputAction tapAction;
        private InputAction pressAction;
        private InputAction positionAction;
        private bool isPressed;
        private Vector2 previousPosition;

        private void Awake()
        {
            inputActions = new InputActions();
            mobileMap = inputActions.asset.FindActionMap("Mobile", true);
            tapAction = mobileMap.FindAction("Tap", true);
            pressAction = mobileMap.FindAction("Press", true);
            positionAction = mobileMap.FindAction("Position", true);
        }

        private void OnEnable()
        {
            if (mobileMap == null)
            {
                return;
            }

            tapAction.performed += HandleTap;
            pressAction.started += HandlePressStarted;
            pressAction.canceled += HandlePressCanceled;
            mobileMap.Enable();
        }

        private void OnDisable()
        {
            if (mobileMap == null)
            {
                return;
            }

            tapAction.performed -= HandleTap;
            pressAction.started -= HandlePressStarted;
            pressAction.canceled -= HandlePressCanceled;
            mobileMap.Disable();
        }

        private void OnDestroy()
        {
            inputActions?.Dispose();
        }

        private void Update()
        {
            if (!isPressed || positionAction == null)
            {
                return;
            }

            Vector2 currentPosition = positionAction.ReadValue<Vector2>();
            Vector2 delta = currentPosition - previousPosition;
            previousPosition = currentPosition;
            OnDrag?.Invoke(currentPosition, delta, true);
        }

        private void HandleTap(InputAction.CallbackContext context)
        {
            if (positionAction == null)
            {
                OnTap?.Invoke(Vector2.zero);
                return;
            }

            OnTap?.Invoke(positionAction.ReadValue<Vector2>());
        }

        private void HandlePressStarted(InputAction.CallbackContext context)
        {
            isPressed = true;
            previousPosition = positionAction == null ? Vector2.zero : positionAction.ReadValue<Vector2>();
            OnDrag?.Invoke(previousPosition, Vector2.zero, true);
        }

        private void HandlePressCanceled(InputAction.CallbackContext context)
        {
            if (!isPressed)
            {
                return;
            }

            isPressed = false;
            Vector2 currentPosition = positionAction == null ? previousPosition : positionAction.ReadValue<Vector2>();
            Vector2 delta = currentPosition - previousPosition;
            previousPosition = currentPosition;
            OnDrag?.Invoke(currentPosition, delta, false);
        }
    }
}
