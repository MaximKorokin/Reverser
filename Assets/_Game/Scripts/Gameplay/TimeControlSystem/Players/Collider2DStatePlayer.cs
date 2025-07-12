using UnityEngine;

public class Collider2DStatePlayer : ComponentStatePlayer
{
    private readonly Collider2D _collider;

    public Collider2DStatePlayer(Collider2D collider, TimeControlSettings settings) : base(settings)
    {
        _collider = collider;
    }

    public override void PlayInterpolatedState(ComponentState state1, ComponentState state2, float time)
    {
        PlayState(MyMath.InterpolateAbsolute(state1, state2, state1.Time, state2.Time, time));
    }

    public override void PlayState(ComponentState state)
    {
        var collider2DState = RequireUtils.CastRequired<ComponentState, Collider2DState>(state);
        
        _collider.isTrigger = collider2DState.IsTrigger;
    }
}
