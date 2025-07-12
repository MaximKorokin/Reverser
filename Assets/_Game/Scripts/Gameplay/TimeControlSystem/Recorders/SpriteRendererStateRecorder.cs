using UnityEngine;

public class SpriteRendererStateRecorder : ComponentStateRecorder
{
    private readonly SpriteRenderer _spriteRenderer;

    public SpriteRendererStateRecorder(SpriteRenderer spriteRenderer, TimeControlSettings settings) : base(settings)
    {
        _spriteRenderer = spriteRenderer;
    }

    protected override ComponentState RecordStateInternal()
    {
        return new SpriteRendererState(_spriteRenderer.sprite, _spriteRenderer.color, _spriteRenderer.flipX);
    }
}
