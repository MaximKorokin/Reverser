using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : InputHandler
{
    public event Action JumpInputRecieved;
    public event Action<float> MoveInputRecieved;
    public event Action InteractInputRecieved;

    public PlayerInputHandler(InputSystemActions inputActions) : base(inputActions)
    {
    }

    private void OnMove(InputAction.CallbackContext obj)
    {
        MoveInputRecieved?.Invoke(InputActions.Player.Move.ReadValue<Vector2>().x);
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        JumpInputRecieved?.Invoke();
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        InteractInputRecieved?.Invoke();
    }

    protected override void SubscribeToActions()
    {
        InputActions.Player.Jump.performed += OnJump;

        InputActions.Player.Move.performed += OnMove;
        InputActions.Player.Move.canceled += OnMove;

        InputActions.Player.Interact.performed += OnInteract;
    }

    protected override void UnsubscribeSubscribeFromActions()
    {
        InputActions.Player.Jump.performed -= OnJump;

        InputActions.Player.Move.performed -= OnMove;
        InputActions.Player.Move.canceled -= OnMove;

        InputActions.Player.Interact.performed -= OnInteract;
    }
}
