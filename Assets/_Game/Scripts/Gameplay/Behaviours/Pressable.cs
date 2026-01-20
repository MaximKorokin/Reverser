using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class Pressable : MonoBehaviourBase, IStateful
{
    [SerializeField]
    private Collision2DTriggerDetector _pressableObject;
    [SerializeField]
    private Vector2 _pressDelta;
    [SerializeField]
    private float _pressTime;
    [SerializeField]
    private float _unpressDelayTime;

    private HashSet<Pressing> _pressingObjects = new();
    private Timer.DelayedAction _pressDelayedAction;
    private Vector2 _pressableObjectInitialPosition;
    private Timer _timer;

    public bool _currentState = false;
    public bool CurrentState
    {
        get => _currentState;
        private set
        {
            if (_currentState == value) return;
            _currentState = value;
            StateChanged?.Invoke(_currentState);
        }
    }

    public event Action<bool> StateChanged;

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
        if (!collision.collider.gameObject.TryGetComponent<Pressing>(out var pressingObject)) return;

        _pressingObjects.Add(pressingObject);

        _pressDelayedAction?.Cancel();
        _pressDelayedAction = _timer.ScheduleVector2Interpolation(
            () => _pressableObject.transform.position,
            p => _pressableObject.transform.position = p,
            () => _pressableObjectInitialPosition + _pressDelta,
            _pressTime)
            .Then(() => CurrentState = true);
    }

    private void OnUnpressed(Collision2D collision)
    {
        if (!collision.collider.gameObject.TryGetComponent<Pressing>(out var pressingObject)) return;

        _pressingObjects.Remove(pressingObject);
        if (_pressingObjects.Count > 0) return;

        _pressDelayedAction?.Cancel();
        _pressDelayedAction = _timer.Schedule(() =>
        {
            CurrentState = false;
            _pressDelayedAction = _timer.ScheduleVector2Interpolation(
                () => _pressableObject.transform.position,
                p => _pressableObject.transform.position = p,
                () => _pressableObjectInitialPosition,
                _pressTime);
        }, _unpressDelayTime);
    }
}
