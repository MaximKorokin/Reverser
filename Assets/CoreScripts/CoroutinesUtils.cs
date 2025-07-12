﻿using System;
using System.Collections;
using UnityEngine;

public static class CoroutinesUtils
{
    public static CoroutineWrapper StartCoroutineSafe(this MonoBehaviour behaviour, IEnumerator enumerator, Action finalAction = null)
    {
        // SafeCoroutine uses wrapper variable, so it is declared before creation
        CoroutineWrapper wrapper = null;
        wrapper = new(behaviour.StartCoroutine(SafeCoroutine()), finalAction);
        return wrapper;

        IEnumerator SafeCoroutine()
        {
            // Needs this condition in case of enumerator is empty and therefore wrapper is not created before Stop call
            if (enumerator.MoveNext())
            {
                yield return enumerator.Current;
            }
            else
            {
                yield return null;
            }

            while (enumerator.MoveNext())
            {
                yield return enumerator.Current;
            }
            wrapper.Stop();
        }
    }

    public static void StopCoroutine(this MonoBehaviour behaviour, CoroutineWrapper wrapper)
    {
        behaviour.StopCoroutine(wrapper.Coroutine);
        wrapper.Stop();
    }

    public static IEnumerator WaitForSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    public static IEnumerator YieldNull()
    {
        yield return null;
    }

    public static IEnumerator WaitForAsyncOperation(AsyncOperation operation)
    {
        while (operation != null && !operation.isDone)
        {
            yield return null;
        }
    }

    public static IEnumerator InterpolationCoroutine(Func<float> currentValueGetter, Action<float> nextValueSetter, Func<float> targetValueGetter, float time)
    {
        float initialValue = currentValueGetter();
        float elapsed = 0f;

        while (elapsed < time)
        {
            elapsed += Time.unscaledDeltaTime;
            var rate = Mathf.Clamp01(elapsed / time);
            var newValue = Mathf.Lerp(initialValue, targetValueGetter(), rate);
            nextValueSetter(newValue);

            yield return null;
        }
        nextValueSetter(targetValueGetter());
    }
}

public class CoroutineWrapper
{
    public Coroutine Coroutine { get; private set; }
    public Action FinalAction { get; private set; }
    public bool HasFinished { get; private set; }

    public CoroutineWrapper(Coroutine coroutine, Action finalAction = null)
    {
        Coroutine = coroutine;
        FinalAction = finalAction;
        HasFinished = false;
    }

    public void Stop()
    {
        if (!HasFinished)
        {
            HasFinished = true;
            FinalAction?.Invoke();
        }
    }
}
