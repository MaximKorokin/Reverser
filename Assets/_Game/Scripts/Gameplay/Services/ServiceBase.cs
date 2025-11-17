using System;

public class ServiceBase
{
    public bool IsEnabled { get; private set; }

    public event Action ServiceEnabled;
    public event Action ServiceDisabled;

    public virtual void Enable()
    {
        IsEnabled = true;
        ServiceEnabled?.Invoke();
    }

    public virtual void Disable()
    {
        IsEnabled = false;
        ServiceDisabled?.Invoke();
    }

    // todo: add this in children where it is required
    protected bool EnsureEnabled(string name)
    {
        if (!IsEnabled) Logger.Warn($"Trying to use disabled service {name}");
        return IsEnabled;
    }
}
