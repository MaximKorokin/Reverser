public class LevelStartGameState : GameState
{
    private readonly TimeControlMediator _timeControlMediator;
    private readonly LevelMainGameState _levelMainGameState;

    public LevelStartGameState(UIInputHandler uiInputHandler, TimeControlMediator timeControlMediator, LevelMainGameState levelMainGameState) : base(uiInputHandler)
    {
        _timeControlMediator = timeControlMediator;
        _levelMainGameState = levelMainGameState;
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
        SwitchState(_levelMainGameState);
    }
}
