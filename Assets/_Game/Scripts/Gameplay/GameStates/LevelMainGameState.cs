public class LevelMainGameState : GameState
{
    private readonly TimeControlMediator _timeControlMediator;
    private readonly Timer _timer;
    private readonly LevelSharedContext _levelSharedContext;

    public LevelMainGameState(
        UIInputHandler uiInputHandler,
        Timer timer,
        LevelSharedContext levelSharedContext,
        TimeControlMediator timeControlMediator) : base(uiInputHandler)
    {
        _levelSharedContext = levelSharedContext;
        _timeControlMediator = timeControlMediator;
        _timer = timer;
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
        SwitchState(typeof(LevelEndGameState));
    }

    protected override void OnCancelInputRecieved()
    {

    }

    protected override void OnSubmitInputRecieved()
    {

    }
}
