using System;

public abstract class GameState
{
    public bool IsEnabled { get; private set; } = false;
    protected readonly UIInputHandler UIInputHandler;

    public event Action<GameState, Type> SwitchStateRequested;

    public GameState(UIInputHandler uiInputHandler)
    {
        UIInputHandler = uiInputHandler;
    }

    public bool Disable()
    {
        if (!IsEnabled)
        {
            Logger.Warn($"Trying to disable {nameof(GameState)} of type {GetType()} that is already disabled");
            return false;
        }

        UIInputHandler.Disable();
        UIInputHandler.SubmitInputRecieved -= OnSubmitInputRecieved;
        UIInputHandler.CancelInputRecieved -= OnCancelInputRecieved;
        IsEnabled = false;

        DisableInternal();

        return true;
    }

    protected virtual void DisableInternal() { }

    public bool Enable()
    {
        if (IsEnabled)
        {
            Logger.Warn($"Trying to enable {nameof(GameState)} of type {GetType()} that is already enabled");
            return false;
        }

        UIInputHandler.Enable();
        UIInputHandler.SubmitInputRecieved += OnSubmitInputRecieved;
        UIInputHandler.CancelInputRecieved += OnCancelInputRecieved;
        IsEnabled = true;

        EnableInternal();

        return true;
    }

    protected virtual void EnableInternal() { }

    protected void SwitchState(Type gameStateType)
    {
        SwitchStateRequested?.Invoke(this, gameStateType);
    }

    protected virtual void OnSubmitInputRecieved() { }
    protected virtual void OnCancelInputRecieved() { }
}
