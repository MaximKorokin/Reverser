using System;

public abstract class GameState
{
    public bool IsEnabled { get; private set; } = false;

    public event Action<GameState, Type, object> SwitchStateRequested;
    public event Action Enabled;
    public event Action Disabled;

    public bool Disable()
    {
        if (!IsEnabled)
        {
            Logger.Warn($"Trying to disable {nameof(GameState)} of type {GetType()} that is already disabled");
            return false;
        }

        IsEnabled = false;

        DisableInternal();

        Disabled?.Invoke();

        return true;
    }

    protected virtual void DisableInternal() { }

    public bool Enable(object parameter)
    {
        if (IsEnabled)
        {
            Logger.Warn($"Trying to enable {nameof(GameState)} of type {GetType()} that is already enabled");
            return false;
        }

        IsEnabled = true;

        EnableInternal(parameter);

        Enabled?.Invoke();

        return true;
    }

    protected virtual void EnableInternal(object parameter) { }

    protected void SwitchState(Type gameStateType, object parameter = null)
    {
        SwitchStateRequested?.Invoke(this, gameStateType, parameter);
    }
}
