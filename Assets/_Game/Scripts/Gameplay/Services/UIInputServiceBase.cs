public abstract class UIInputServiceBase : ServiceBase
{
    private readonly UIInputHandler UIInputHandler;

    public UIInputServiceBase(UIInputHandler uiInputHandler)
    {
        UIInputHandler = uiInputHandler;
    }

    public override void EnableService()
    {
        UIInputHandler.Enable();
        UIInputHandler.SubmitInputRecieved += OnSubmitInputRecieved;
        UIInputHandler.CancelInputRecieved += OnCancelInputRecieved;
        base.EnableService();
    }

    public override void DisableService()
    {
        UIInputHandler.Disable();
        UIInputHandler.SubmitInputRecieved -= OnSubmitInputRecieved;
        UIInputHandler.CancelInputRecieved -= OnCancelInputRecieved;
        base.DisableService();
    }

    protected abstract void OnSubmitInputRecieved();
    protected abstract void OnCancelInputRecieved();
}