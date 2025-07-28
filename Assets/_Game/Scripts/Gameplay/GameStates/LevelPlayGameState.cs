using VContainer.Unity;

public class LevelPlayGameState : GameState
{
    private readonly TimeControlMediator _timeControlMediator;
    private readonly Timer _timer;
    private readonly LevelSharedContext _levelSharedContext;

    private Timer.DelayedAction _timeStateDelayedAction;

    public LevelPlayGameState(
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

        _levelSharedContext.LevelTimeCounter.SetPaused(false);
        UpdateTimeState();
        
        _levelSharedContext.LevelCompleted += OnLevelCompleted;
    }

    private void UpdateTimeState()
    {
        var remainingTime = _levelSharedContext.LevelTimeCounter.RemainingTime;
        Logger.Log(remainingTime);
        if (remainingTime > _levelSharedContext.LevelData.LevelHalfDuration)
        {
            _timeControlMediator.SetTimeFlowMode(TimeFlowMode.Forward);
            _timeStateDelayedAction = _timer.Schedule(UpdateTimeState, remainingTime - _levelSharedContext.LevelData.LevelHalfDuration);
        }
        else if (remainingTime > 0)
        {
            _timeControlMediator.SetTimeFlowMode(TimeFlowMode.Backward);
            _timeStateDelayedAction = _timer.Schedule(UpdateTimeState, remainingTime);
        }
        else
        {
            SwitchState(typeof(LevelFailGameState));
        }
    }

    protected override void DisableInternal()
    {
        base.DisableInternal();
        _timeControlMediator.SetTimeFlowMode(TimeFlowMode.Paused);

        _timeStateDelayedAction?.Cancel();

        _levelSharedContext.LevelCompleted -= OnLevelCompleted;
    }

    private void OnLevelCompleted()
    {
        SwitchState(typeof(LevelCompleteGameState));
    }

    protected override void OnCancelInputRecieved()
    {
        base.OnCancelInputRecieved();
        _levelSharedContext.LevelTimeCounter.SetPaused(true);
        SwitchState(typeof(LevelPauseGameState));
    }
}
