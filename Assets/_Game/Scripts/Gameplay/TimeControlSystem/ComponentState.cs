using UnityEngine;

public class ComponentState
{
    public readonly float Time;

    public ComponentState()
    {
        Time = UnityEngine.Time.time;
    }
}

public class TransformState : ComponentState
{
    public readonly Vector3 Position;
    public readonly Vector3 EulerAngles;
    public readonly Vector3 Scale;
    public readonly Transform Parent;

    public TransformState(Vector3 position, Vector3 eulerAngles, Vector3 scale, Transform parent)
    {
        Position = position;
        EulerAngles = eulerAngles;
        Scale = scale;
        Parent = parent;
    }
}

public class SpriteRendererState : ComponentState
{
    public readonly Sprite Sprite;
    public readonly Color Color;
    public readonly bool FlipX;

    public SpriteRendererState(Sprite sprite, Color color, bool flipX)
    {
        Sprite = sprite;
        Color = color;
        FlipX = flipX;
    }
}

public class Collider2DState : ComponentState
{
    public readonly bool IsTrigger;

    public Collider2DState(bool isTrigger)
    {
        IsTrigger = isTrigger;
    }
}
