using System;

public class GamePauseService : UIInputServiceBase
{
    private readonly TimeControlMediator _timeControlMediator;
    private readonly LevelSharedContext _levelSharedContext;
    private readonly Timer _timer;

    private Timer.DelayedAction _timeStateDelayedAction;

    public bool IsPaused { get; private set; }

    public event Action GamePaused;
    public event Action GameResumed;

    public GamePauseService(
        UIInputHandler inputHandler,
        LevelSharedContext levelSharedContext,
        TimeControlMediator timeControlMediator,
        Timer timer) : base(inputHandler)
    {
        _levelSharedContext = levelSharedContext;
        _timeControlMediator = timeControlMediator;
        _timer = timer;
    }

    public void Pause(bool silent = false)
    {
        if (!EnsureEnabled(nameof(GamePauseService))) return;

        IsPaused = true;

        _levelSharedContext.LevelTimeCounter.SetPaused(true);
        _timeControlMediator.SetTimeFlowMode(TimeFlowMode.Paused);

        _timeStateDelayedAction?.Complete();

        if (!silent) GamePaused?.Invoke();
    }

    public void Resume(bool silent = false)
    {
        if (!EnsureEnabled(nameof(GamePauseService))) return;

        IsPaused = false;

        _levelSharedContext.LevelTimeCounter.SetPaused(false);
        UpdateTimeState();

        if (!silent) GameResumed?.Invoke();
    }

    private void UpdateTimeState()
    {
        var remainingTime = _levelSharedContext.LevelTimeCounter.RemainingTime;
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
            _levelSharedContext.InvokeLevelFailed();
        }
    }

    public override void Disable()
    {
        Pause(true);

        base.Disable();
    }

    protected override void OnSubmitInputRecieved()
    {

    }

    protected override void OnCancelInputRecieved()
    {
        if (IsPaused) Resume();
        else Pause();
    }
}
