public class LevelStartGameState : GameState
{
    private readonly TimeControlMediator _timeControlMediator;

    public LevelStartGameState(UIInputHandler uiInputHandler, TimeControlMediator timeControlMediator) : base(uiInputHandler)
    {
        _timeControlMediator = timeControlMediator;
    }

    protected override void EnableInternal()
    {
        base.EnableInternal();
        _timeControlMediator.SetTimeFlowMode(TimeFlowMode.Paused);
    }

    protected override void OnCancelInputRecieved()
    {

    }

    protected override void OnSubmitInputRecieved()
    {
        SwitchState(typeof(LevelMainGameState));
    }
}
