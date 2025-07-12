using System;
using UnityEngine;
using VContainer;

public class TimeControlComponentSwitcher : MonoBehaviourBase
{
    [SerializeField]
    private TimeFlowModeInterpretation _timeFlowModeInterpretation;
    [SerializeField]
    private TimeControlBehaviour _components;

    [Inject]
    private void Construct(TimeControlMediator timeControlMediator)
    {
        timeControlMediator.TimeFlowModeChanged += OnTimeFlowModeChanged;
        OnTimeFlowModeChanged(TimeFlowMode.None, timeControlMediator.TimeFlowMode);
    }

    private void OnTimeFlowModeChanged(TimeFlowMode previousMode, TimeFlowMode newMode)
    {
        if (previousMode == newMode) return;
        
        var enabled = _timeFlowModeInterpretation.InterpretTimeFlowMode(newMode) == TimeFlowMode.Forward;

        if (_components.HasFlag(TimeControlBehaviour.Collider2D) && TryGetComponent<Collider2D>(out var collider))
        {
            collider.enabled = enabled;
        }
        if (_components.HasFlag(TimeControlBehaviour.Animator) && TryGetComponent<Animator>(out var animator))
        {
            // Animator resets when disabled
            if (enabled) animator.Rebind();
            animator.enabled = enabled;
        }
        if (_components.HasFlag(TimeControlBehaviour.Pickable) && TryGetComponent<Pickable>(out var pickable))
        {
            pickable.enabled = enabled;
        }
        if (_components.HasFlag(TimeControlBehaviour.Rigidbody2DSimulated) && TryGetComponent<Rigidbody2D>(out var rigidbody1))
        {
            rigidbody1.simulated = enabled;
            rigidbody1.linearVelocity = Vector3.zero;
        }
        if (_components.HasFlag(TimeControlBehaviour.Rigidbody2DType) && TryGetComponent<Rigidbody2D>(out var rigidbody2))
        {
            rigidbody2.bodyType = enabled ? RigidbodyType2D.Dynamic : RigidbodyType2D.Kinematic;
            rigidbody2.linearVelocity = Vector3.zero;
        }
    }

    [Flags]
    private enum TimeControlBehaviour
    {
        Collider2D = 2,
        Animator = 4,
        Pickable = 8,
        Rigidbody2DType = 16,
        Rigidbody2DSimulated = 32,
    }
}
