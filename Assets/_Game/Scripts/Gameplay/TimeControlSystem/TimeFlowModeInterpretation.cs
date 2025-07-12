public enum TimeFlowModeInterpretation
{
    Original,
    Inversed,
}

public static class TimeFlowModeInterpretationExtensions
{
    public static TimeFlowMode InterpretTimeFlowMode(this TimeFlowModeInterpretation interpretation, TimeFlowMode timeFlowMode)
    {
        return interpretation switch
        {
            TimeFlowModeInterpretation.Original => timeFlowMode,
            TimeFlowModeInterpretation.Inversed => timeFlowMode == TimeFlowMode.Backward ? TimeFlowMode.Forward : TimeFlowMode.Paused,
            _ => timeFlowMode
        };
    }
}
