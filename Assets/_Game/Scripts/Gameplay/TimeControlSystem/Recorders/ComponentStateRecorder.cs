using System.Collections.Generic;

public abstract class ComponentStateRecorder : ComponentStateProcessor
{
    private readonly LinkedList<ComponentState> _states = new();

    public ComponentStateRecorder(TimeControlSettings settings) : base(settings) { }

    public ComponentState PopComponentState()
    {
        if (_states.Count == 0) return null;
        var state = _states.Last.Value;
        _states.RemoveLast();
        return state;
    }

    public virtual void RecordState()
    {
        var state = RecordStateInternal();
        _states.AddLast(state);
        while (_states.Count > 0 && state.Time - _states.First.Value.Time > Settings.MaxSecondsToRecord)
        {
            _states.RemoveFirst();
        }
    }

    public void IncreaseTimeOffset(float timeOffset)
    {
        _states.ForEach(x => x.Time += timeOffset);
    }

    protected abstract ComponentState RecordStateInternal();
}
