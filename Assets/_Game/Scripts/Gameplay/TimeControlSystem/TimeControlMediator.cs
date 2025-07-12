using System;

public class TimeControlMediator
{
    public TimeFlowMode TimeFlowMode { get; private set; }

    public event Action<TimeFlowMode, TimeFlowMode> TimeFlowModeChanged;

    public TimeControlMediator(TimeControlSettings settings)
    {
        //TimeFlowMode = settings.InitialTimeFlowMode;
    }

    public void SetTimeFlowMode(TimeFlowMode mode)
    {
        if (TimeFlowMode != mode)
        {
            TimeFlowModeChanged?.Invoke(TimeFlowMode, mode);
        }
        //Logger.Log("Time flow changed: " + mode);
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
