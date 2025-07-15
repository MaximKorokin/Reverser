public class LevelStartGameState : GameState
{
    private readonly TimeControlMediator _timeControlMediator;
    private readonly LevelConstructor _levelConstructor;

    public LevelStartGameState(
        UIInputHandler uiInputHandler,
        LevelConstructor levelConstructor,
        TimeControlMediator timeControlMediator) : base(uiInputHandler)
    {
        _levelConstructor = levelConstructor;
        _timeControlMediator = timeControlMediator;
    }

    protected override void EnableInternal()
    {
        base.EnableInternal();
        _timeControlMediator.SetTimeFlowMode(TimeFlowMode.Paused);
        _levelConstructor.ConstructLevel();
    }

    protected override void OnCancelInputRecieved()
    {

    }

    protected override void OnSubmitInputRecieved()
    {
        SwitchState(typeof(LevelMainGameState));
    }
}
