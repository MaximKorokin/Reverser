using System;
using UnityEngine;

public static class TimerExtensions
{
    public static Timer.DelayedAction ScheduleVector2Interpolation(this Timer timer, Func<Vector2> currentValueGetter, Action<Vector2> nextValueSetter, Func<Vector2> targetValueGetter, float time)
    {
        var action1 = timer.ScheduleInterpolation(() => currentValueGetter().x, x => nextValueSetter(new(x, currentValueGetter().y)), () => targetValueGetter().x, time);
        var action2 = timer.ScheduleInterpolation(() => currentValueGetter().y, y => nextValueSetter(new(currentValueGetter().x, y)), () => targetValueGetter().y, time);
        var actionsWrapper = new Timer.DelayedAction(timer, action1, action2);
        action1.Then(() => actionsWrapper.Cancel());
        return actionsWrapper;
    }

    public static Timer.DelayedAction SetActiveNextFrame(this GameObject gameObject, bool value, Timer timer)
    {
        return timer.Schedule(() => gameObject.SetActive(value), 0);
    }
}
