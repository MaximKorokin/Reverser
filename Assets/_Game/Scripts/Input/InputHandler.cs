using System;

public abstract class InputHandler : IDisposable
{
    protected readonly InputSystemActions InputActions;

    public InputHandler(InputSystemActions inputActions)
    {
        InputActions = inputActions;
    }

    public virtual void Enable()
    {
        UnsubscribeSubscribeFromActions();
        SubscribeToActions();
        InputActions.Enable();
    }

    public virtual void Disable()
    {
        UnsubscribeSubscribeFromActions();
        InputActions.Disable();
    }

    public virtual void Dispose()
    {
        Disable();
    }

    protected abstract void SubscribeToActions();
    protected abstract void UnsubscribeSubscribeFromActions();
}
