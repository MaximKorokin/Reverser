using System;
using UnityEngine.InputSystem;

public class UIInputHandler : InputHandler
{
    public event Action SubmitInputRecieved;
    public event Action CancelInputRecieved;

    public UIInputHandler(InputSystemActions inputActions) : base(inputActions)
    {
    }

    private void OnSubmit(InputAction.CallbackContext context)
    {
        SubmitInputRecieved?.Invoke();
    }

    private void OnCancel(InputAction.CallbackContext context)
    {
        CancelInputRecieved?.Invoke();
    }

    protected override void SubscribeToActions()
    {
        InputActions.UI.Submit.performed += OnSubmit;
        InputActions.UI.Cancel.performed += OnCancel;
    }

    protected override void UnsubscribeSubscribeFromActions()
    {
        InputActions.UI.Submit.performed -= OnSubmit;
        InputActions.UI.Cancel.performed -= OnCancel;
    }
}
