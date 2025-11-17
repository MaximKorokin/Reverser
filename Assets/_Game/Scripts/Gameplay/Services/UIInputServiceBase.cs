public abstract class UIInputServiceBase : ServiceBase
{
    private readonly UIInputHandler UIInputHandler;

    public UIInputServiceBase(UIInputHandler uiInputHandler)
    {
        UIInputHandler = uiInputHandler;
    }

    public override void Enable()
    {
        UIInputHandler.Enable();
        UIInputHandler.SubmitInputRecieved += OnSubmitInputRecieved;
        UIInputHandler.CancelInputRecieved += OnCancelInputRecieved;
        base.Enable();
    }

    public override void Disable()
    {
        UIInputHandler.Disable();
        UIInputHandler.SubmitInputRecieved -= OnSubmitInputRecieved;
        UIInputHandler.CancelInputRecieved -= OnCancelInputRecieved;
        base.Disable();
    }

    protected abstract void OnSubmitInputRecieved();
    protected abstract void OnCancelInputRecieved();
}