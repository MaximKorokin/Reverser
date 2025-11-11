public abstract class ViewBase : UIBehaviourBase
{
    private ServiceBase _service;

    protected void ConstructBase(ServiceBase service)
    {
        _service = service;

        _service.ServiceEnabled += EnableViewBase;
        _service.ServiceDisabled += DisableViewBase;

        OnDestroying += () =>
        {
            _service.ServiceEnabled -= EnableViewBase;
            _service.ServiceDisabled -= DisableViewBase;
        };
    }

    private void EnableViewBase()
    {
        if (_service.IsEnabled) EnableView();
    }

    private void DisableViewBase()
    {
        if (!_service.IsEnabled) DisableView();
    }

    protected abstract void EnableView();
    protected abstract void DisableView();
}
