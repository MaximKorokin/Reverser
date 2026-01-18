using System;
using UnityEngine;
using VContainer;

public class Pressable : MonoBehaviourBase
{
    [SerializeField]
    private Collision2DTriggerDetector _pressableObject;
    [SerializeField]
    private Vector2 _pressDelta;
    [SerializeField]
    private float _pressTime;
    [SerializeField]
    private float _unpressDelayTime;

    private Timer.DelayedAction _pressDelayedAction;
    private Vector2 _pressableObjectInitialPosition;
    private Timer _timer;

    public Action<bool> StateChanged;

    [Inject]
    private void Construct(Timer timer)
    {
        _timer = timer;
    }

    protected override void Awake()
    {
        base.Awake();

        _pressableObjectInitialPosition = _pressableObject.transform.position;

        _pressableObject.Collision2DEntered += OnPessed;
        _pressableObject.Collision2DExited += OnUnpressed;
    }

    private void OnPessed(Collision2D collision)
    {
        _pressDelayedAction?.Cancel();
        _pressDelayedAction = _timer.ScheduleVector2Interpolation(
            () => _pressableObject.transform.position,
            p => _pressableObject.transform.position = p,
            () => _pressableObjectInitialPosition + _pressDelta,
            _pressTime)
            .Then(() => StateChanged?.Invoke(true));
    }

    private void OnUnpressed(Collision2D collision)
    {
        _pressDelayedAction?.Cancel();
        _pressDelayedAction = _timer.Schedule(() =>
        {
            StateChanged?.Invoke(false);
            _pressDelayedAction = _timer.ScheduleVector2Interpolation(
                () => _pressableObject.transform.position,
                p => _pressableObject.transform.position = p,
                () => _pressableObjectInitialPosition,
                _pressTime);
        }, _unpressDelayTime);
    }
}
