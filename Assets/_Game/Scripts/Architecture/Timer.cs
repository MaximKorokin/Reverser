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
            () => pointer.Complete()));
        return pointer;
    }

    public DelayedAction Schedule(Action callback, float delay)
    {
        var pointer = new DelayedAction(this);
        _activeCoroutines.Add(pointer, this.StartCoroutineSafe(
            CoroutinesUtils.WaitForSeconds(callback, delay),
            () => pointer.Complete()));
        return pointer;
    }

    public DelayedAction ScheduleInterpolation(Func<float> currentValueGetter, Action<float> nextValueSetter, Func<float> targetValueGetter, float time)
    {
        var pointer = new DelayedAction(this);
        _activeCoroutines.Add(pointer, this.StartCoroutineSafe(
            CoroutinesUtils.InterpolationCoroutine(currentValueGetter, nextValueSetter, targetValueGetter, time),
            () => pointer.Complete()));
        return pointer;
    }

    public DelayedAction Wrap(params DelayedAction[] wrappedActions)
    {
        var wrapper = new DelayedAction(this, wrappedActions);
        _activeCoroutines.Add(wrapper, null);
        return wrapper;
    }

    public bool Cancel(DelayedAction obj)
    {
        if (obj != null && _activeCoroutines.TryGetValue(obj, out var coroutine))
        {
            _activeCoroutines.Remove(obj);
            if (coroutine != null) this.StopCoroutine(coroutine);
            return true;
        }
        return false;
    }

    public class DelayedAction
    {
        private readonly Timer _timer;
        private readonly DelayedAction[] _wrappedActions;

        private readonly List<Action> _callbacks = new();

        public DelayedAction(Timer timer, params DelayedAction[] wrappedActions)
        {
            _timer = timer;
            _wrappedActions = wrappedActions;
        }

        public DelayedAction Then(Action callback)
        {
            _callbacks.Add(callback);
            return this;
        }

        public void Complete()
        {
            if (!_timer.Cancel(this)) return;

            _wrappedActions.ForEach(x => x.Complete());
            _callbacks?.ForEach(x => x.Invoke());
        }

        public void Cancel()
        {
            if (!_timer.Cancel(this)) return;

            _wrappedActions.ForEach(x => x.Cancel());
        }
    }
}
