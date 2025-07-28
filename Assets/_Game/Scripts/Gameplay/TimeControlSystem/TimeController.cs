using UnityEngine;

public class TimeController
{
    private readonly ComponentStateRecorder _recorder;
    private readonly ComponentStatePlayer _player;

    private ComponentState _nextComponentState;
    private ComponentState _previousComponentState;
    private float _previousStateTime;

    public TimeController(ComponentStateRecorder recorder, ComponentStatePlayer player)
    {
        _recorder = recorder;
        _player = player;
    }

    public void Tick(TimeFlowMode timeFlowMode, float deltaTime)
    {
        if (timeFlowMode == TimeFlowMode.Paused)
        {
            _recorder.IncreaseTimeOffset(deltaTime);
            ResetCurrentStates();
        }
        else if (timeFlowMode == TimeFlowMode.Forward)
        {
            _recorder.RecordState();
            ResetCurrentStates();
        }
        else if (timeFlowMode == TimeFlowMode.Backward)
        {
            UpdateCurrentStates();
            if (_nextComponentState != null)
            {
                _player.PlayInterpolatedState(_nextComponentState, _previousComponentState, _previousComponentState.Time - (Time.time - _previousStateTime));
            }
        }
    }

    private void UpdateCurrentStates()
    {
        if (_previousComponentState == null)
        {
            _previousStateTime = Time.time;
            _previousComponentState = _recorder.PopComponentState();
            _nextComponentState = _recorder.PopComponentState();
        }
        while (_nextComponentState != null && Time.time - _previousStateTime > _previousComponentState.Time - _nextComponentState.Time)
        {
            _previousStateTime = _previousStateTime + _previousComponentState.Time - _nextComponentState.Time;
            _previousComponentState = _nextComponentState;
            _nextComponentState = _recorder.PopComponentState();
        }
    }

    private void ResetCurrentStates()
    {
        _previousStateTime = 0;
        _previousComponentState = null;
        _nextComponentState = null;
    }
}
