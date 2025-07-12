using UnityEngine;

public class ComponentStateProcessorFactory
{
    private readonly TimeControlSettings _settings;

    public ComponentStateProcessorFactory(TimeControlSettings settings)
    {
        _settings = settings;
    }

    public ComponentStateRecorder GetComponentStateRecorder(Component component)
    {
        return component switch
        {
            Transform transform => new TransformStateRecorder(transform, _settings),
            SpriteRenderer spriteRenderer => new SpriteRendererStateRecorder(spriteRenderer, _settings),
            Collider2D collider2D => new Collider2DStateRecorder(collider2D, _settings),
            _ => null,
        };
    }

    public ComponentStatePlayer GetComponentStatePlayer(Component component)
    {
        return component switch
        {
            Transform transform => new TransformStatePlayer(transform, _settings),
            SpriteRenderer spriteRenderer => new SpriteRendererStatePlayer(spriteRenderer, _settings),
            Collider2D collider2D => new Collider2DStatePlayer(collider2D, _settings),
            _ => null,
        };
    }
}
