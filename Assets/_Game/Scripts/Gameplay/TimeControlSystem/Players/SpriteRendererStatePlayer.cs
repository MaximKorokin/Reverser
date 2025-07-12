using UnityEngine;

public class SpriteRendererStatePlayer : ComponentStatePlayer
{
    private readonly SpriteRenderer _spriteRenderer;

    public SpriteRendererStatePlayer(SpriteRenderer spriteRenderer, TimeControlSettings settings) : base(settings)
    {
        _spriteRenderer = spriteRenderer;
    }

    public override void PlayInterpolatedState(ComponentState state1, ComponentState state2, float time)
    {
        PlayState(MyMath.InterpolateAbsolute(state1, state2, state1.Time, state2.Time, time));
    }

    public override void PlayState(ComponentState state)
    {
        var spriteRendererState = RequireUtils.CastRequired<ComponentState, SpriteRendererState>(state);

        _spriteRenderer.sprite = spriteRendererState.Sprite;
        _spriteRenderer.color = spriteRendererState.Color;
        _spriteRenderer.flipX = spriteRendererState.FlipX;
    }
}
