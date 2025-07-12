using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviourBase
{
    private readonly Dictionary<DelayedAction, CoroutineWrapper> _activeCoroutines = new();

    public DelayedAction Schedule(Action callback, float delay)
    {
        var pointer = new DelayedAction(this);
        _activeCoroutines.Add(pointer, this.StartCoroutineSafe(
            RunTimer(callback, delay),
            () => pointer.Cancel()));
        return pointer;
    }

    public DelayedAction ScheduleInterpolation(Func<float> currentValueGetter, Action<float> nextValueSetter, Func<float> targetValueGetter, float time)
    {
        var pointer = new DelayedAction(this);
        _activeCoroutines.Add(pointer, this.StartCoroutineSafe(
            CoroutinesUtils.InterpolationCoroutine(currentValueGetter, nextValueSetter, targetValueGetter, time),
            () => pointer.Cancel()));
        return pointer;
    }

    public void Cancel(DelayedAction obj)
    {
        if (obj != null && _activeCoroutines.TryGetValue(obj, out var coroutine))
        {
            this.StopCoroutine(coroutine);
        }
    }

    private IEnumerator RunTimer(Action callback, float delay)
    {
        yield return new WaitForSeconds(delay);
        callback?.Invoke();
    }

    public class DelayedAction
    {
        private readonly Timer _timer;
        private readonly DelayedAction[] _wrappedActions;

        private Action _callback;

        public DelayedAction(Timer timer, params DelayedAction[] wrappedActions)
        {
            _timer = timer;
            _wrappedActions = wrappedActions;
        }

        public void Then(Action callback)
        {
            _callback = callback;
        }

        public void Cancel()
        {
            _timer.Cancel(this);
            _wrappedActions.ForEach(x => _timer.Cancel(x));

            _callback?.Invoke();
        }
    }
}
