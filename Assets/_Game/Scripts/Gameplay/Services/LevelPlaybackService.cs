using System;

public class LevelPlaybackService : ServiceBase
{
    private readonly LevelSharedContext _levelSharedContext;
    private readonly Timer _timer;

    private Timer.DelayedAction _timeCountingDelayedAction;

    public float RemainingTime => _levelSharedContext.LevelTimeCounter.RemainingTime;

    /// <summary>
    /// Provides values in range [0, 1]
    /// </summary>
    public Action<float> PlaybackValueChanged;

    public LevelPlaybackService(
        TimeControlMediator timeControlMediator,
        LevelSharedContext levelSharedContext,
        Timer timer)
    {
        _levelSharedContext = levelSharedContext;
        _timer = timer;

        timeControlMediator.TimeFlowModeChanged += OnTimeFlowModeChanged;
    }

    public override void Enable()
    {
        base.Enable();
    }

    public override void Disable()
    {
        _timeCountingDelayedAction?.Cancel();

        base.Disable();
    }

    private void OnTimeFlowModeChanged(TimeFlowMode previousMode, TimeFlowMode newMode)
    {
        _timeCountingDelayedAction?.Cancel();
        if (newMode == TimeFlowMode.Forward)
        {
            _timeCountingDelayedAction = _timer.ScheduleRepeating(InvokeTimeChanged, 0);
        }
        else if (newMode == TimeFlowMode.Backward)
        {
            _timeCountingDelayedAction = _timer.ScheduleRepeating(InvokeTimeChanged, 0);
        }
    }

    private void InvokeTimeChanged()
    {
        var left = _levelSharedContext.LevelTimeCounter.RemainingTime;
        var half = _levelSharedContext.LevelData.LevelHalfDuration;

        var currentHalf = left >= half
            ? half - (left - half)
            : left;

        PlaybackValueChanged?.Invoke(currentHalf / half);
    }
}
