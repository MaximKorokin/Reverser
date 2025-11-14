using System;

public class TimeControlMediator
{
    public TimeFlowMode TimeFlowMode { get; private set; }

    public event Action<TimeFlowMode, TimeFlowMode> TimeFlowModeChanged;

    public TimeControlMediator(TimeControlSettings settings)
    {
    }

    public void SetTimeFlowMode(TimeFlowMode mode)
    {
        if (TimeFlowMode != mode)
        {
            TimeFlowModeChanged?.Invoke(TimeFlowMode, mode);
        }
        TimeFlowMode = mode;
    }
}

public enum TimeFlowMode
{
    None = 0,
    Forward = 1,
    Backward = 2,
    Paused = 3,
}
