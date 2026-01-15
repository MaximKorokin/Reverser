using System;
using System.Collections.Generic;

public class Timer : MonoBehaviourBase
{
    private readonly Dictionary<DelayedAction, CoroutineWrapper> _activeCoroutines = new();

    public DelayedAction ScheduleRepeating(Action callback, float delay, Func<bool> endCondition = null)
    {
        var pointer = new DelayedAction(this);
        _activeCoroutines.Add(pointer, this.StartCoroutineSafe(
            CoroutinesUtils.Loop(callback, delay, endCondition),
            () => pointer.Cancel()));
        return pointer;
    }

    public DelayedAction Schedule(Action callback, float delay)
    {
        var pointer = new DelayedAction(this);
        _activeCoroutines.Add(pointer, this.StartCoroutineSafe(
            CoroutinesUtils.WaitForSeconds(callback, delay),
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
