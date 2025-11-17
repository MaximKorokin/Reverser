using System;

public abstract class InputHandler : IDisposable
{
    protected readonly InputSystemActions InputActions;

    public InputHandler(InputSystemActions inputActions)
    {
        InputActions = inputActions;
        InputActions.Enable();
    }

    public virtual void Enable()
    {
        UnsubscribeFromActions();
        SubscribeToActions();
    }

    public virtual void Disable()
    {
        UnsubscribeFromActions();
    }

    public virtual void Dispose()
    {
        Disable();
        InputActions.Disable();
    }

    protected abstract void SubscribeToActions();
    protected abstract void UnsubscribeFromActions();
}
