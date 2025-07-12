using UnityEngine;

public class Collider2DStateRecorder : ComponentStateRecorder
{
    private readonly Collider2D _collider;

    public Collider2DStateRecorder(Collider2D collider, TimeControlSettings settings) : base(settings)
    {
        _collider = collider;
    }

    protected override ComponentState RecordStateInternal()
    {
        return new Collider2DState(_collider.isTrigger);
    }
}
