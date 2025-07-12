using UnityEngine;

public class TransformStateRecorder : ComponentStateRecorder
{
    private readonly Transform _transform;

    public TransformStateRecorder(Transform transform, TimeControlSettings settings) : base(settings)
    {
        _transform = transform;
    }

    protected override ComponentState RecordStateInternal()
    {
        return new TransformState(_transform.position, _transform.eulerAngles, _transform.localScale, _transform.parent);
    }
}
