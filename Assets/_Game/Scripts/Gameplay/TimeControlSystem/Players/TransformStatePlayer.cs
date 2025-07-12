using UnityEngine;

public class TransformStatePlayer : ComponentStatePlayer
{
    private readonly Transform _transform;

    public TransformStatePlayer(Transform transform, TimeControlSettings settings) : base(settings)
    {
        _transform = transform;
    }

    public override void PlayInterpolatedState(ComponentState state1, ComponentState state2, float time)
    {
        var transformState1 = RequireUtils.CastRequired<ComponentState, TransformState>(state1);
        var transformState2 = RequireUtils.CastRequired<ComponentState, TransformState>(state2);

        var interpolatedState = new TransformState(
            MyMath.Interpolate(transformState1.Position, transformState2.Position, transformState1.Time, transformState2.Time, time, Vector3.Lerp),
            MyMath.Interpolate(transformState1.EulerAngles, transformState2.EulerAngles, transformState1.Time, transformState2.Time, time, Vector3.Lerp),
            MyMath.Interpolate(transformState1.Scale, transformState2.Scale, transformState1.Time, transformState2.Time, time, Vector3.Lerp),
            MyMath.InterpolateAbsolute(transformState1.Parent, transformState2.Parent, transformState1.Time, transformState2.Time, time));
        PlayState(interpolatedState);
    }

    public override void PlayState(ComponentState state)
    {
        var transformState = RequireUtils.CastRequired<ComponentState, TransformState>(state);
        _transform.position = transformState.Position;
        _transform.eulerAngles = transformState.EulerAngles;
        _transform.localScale = transformState.Scale;
        if (_transform.parent != transformState.Parent)
        {
            _transform.parent = transformState.Parent;
        }
    }
}
