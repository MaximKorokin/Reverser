using System;

public class ServiceBase
{
    public bool IsEnabled { get; private set; }

    public event Action ServiceEnabled;
    public event Action ServiceDisabled;

    public virtual void EnableService()
    {
        IsEnabled = true;
        ServiceEnabled?.Invoke();
    }

    public virtual void DisableService()
    {
        IsEnabled = false;
        ServiceDisabled?.Invoke();
    }

    protected bool EnsureEnabled(string name)
    {
        if (!IsEnabled) Logger.Warn($"Trying to use disabled service {name}");
        return IsEnabled;
    }
}
