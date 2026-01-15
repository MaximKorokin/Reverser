using System;

public abstract class ViewBase : UIBehaviourBase
{
    private ServiceBase _service;

    public event Action Enabled;
    public event Action Disabled;

    protected void ConstructBase(ServiceBase service)
    {
        gameObject.SetActive(true);

        _service = service;

        _service.ServiceEnabled += EnableBase;
        _service.ServiceDisabled += DisableBase;

        OnDestroying += () =>
        {
            _service.ServiceEnabled -= EnableBase;
            _service.ServiceDisabled -= DisableBase;
        };
    }

    private void EnableBase()
    {
        if (_service.IsEnabled)
        {
            Enable();
            Enabled?.Invoke();
        }
    }

    private void DisableBase()
    {
        if (!_service.IsEnabled)
        {
            Disable();
            Disabled?.Invoke();
        }
    }

    protected virtual void Enable() { }
    protected virtual void Disable() { }
}
