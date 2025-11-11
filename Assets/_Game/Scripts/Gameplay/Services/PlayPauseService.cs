using System;

public class PlayPauseService : ServiceBase
{
    private readonly TimeControlMediator _timeControlMediator;
    private readonly LevelSharedContext _levelSharedContext;
    private readonly Timer _timer;

    private Timer.DelayedAction _timeStateDelayedAction;

    public event Action PlayPaused;
    public event Action PlayResumed;

    public PlayPauseService(
        LevelSharedContext levelSharedContext,
        TimeControlMediator timeControlMediator,
        Timer timer)
    {
        _levelSharedContext = levelSharedContext;
        _timeControlMediator = timeControlMediator;
        _timer = timer;
    }

    public void Pause()
    {
        if (!EnsureEnabled(nameof(PlayPauseService))) return;

        _levelSharedContext.LevelTimeCounter.SetPaused(true);
        _timeControlMediator.SetTimeFlowMode(TimeFlowMode.Paused);

        _timeStateDelayedAction?.Cancel();

        PlayPaused?.Invoke();
    }

    public void Resume()
    {
        if (!EnsureEnabled(nameof(PlayPauseService))) return;

        _levelSharedContext.LevelTimeCounter.SetPaused(false);
        UpdateTimeState();

        PlayResumed?.Invoke();
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
}
