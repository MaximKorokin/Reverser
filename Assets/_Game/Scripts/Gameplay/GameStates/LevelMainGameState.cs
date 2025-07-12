public class LevelMainGameState : GameState
{
    private readonly TimeControlMediator _timeControlMediator;
    private readonly Timer _timer;
    private readonly LevelSharedContext _levelSharedContext;
    private readonly LevelEndGameState _levelEndGameState;

    public LevelMainGameState(
        UIInputHandler uiInputHandler,
        Timer timer,
        LevelSharedContext levelSharedContext,
        TimeControlMediator timeControlMediator,
        LevelEndGameState levelEndGameState) : base(uiInputHandler)
    {
        _levelSharedContext = levelSharedContext;
        _timeControlMediator = timeControlMediator;
        _timer = timer;
        _levelEndGameState = levelEndGameState;
    }

    protected override void EnableInternal()
    {
        base.EnableInternal();

        _timeControlMediator.SetTimeFlowMode(TimeFlowMode.Forward);
        _timer.Schedule(
            () => _timeControlMediator.SetTimeFlowMode(TimeFlowMode.Backward),
            _levelSharedContext.LevelData.LevelHalfDuration);

        _levelSharedContext.LevelCompleted += OnLevelCompleted;
    }

    protected override void DisableInternal()
    {
        base.DisableInternal();

        _levelSharedContext.LevelCompleted -= OnLevelCompleted;
    }

    private void OnLevelCompleted()
    {
        SwitchState(_levelEndGameState);
    }

    protected override void OnCancelInputRecieved()
    {

    }

    protected override void OnSubmitInputRecieved()
    {

    }
}
