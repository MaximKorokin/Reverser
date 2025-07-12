using UnityEngine;
using VContainer;

public class Pickable : MonoBehaviourBase
{
    [SerializeField]
    private float _transferTime;

    private Rigidbody2D _rigidbody;
    private Timer _timer;
    private Transform _levelParent;

    [Inject]
    private void Construct(Timer timer, Transform levelParent)
    {
        _timer = timer;
        _levelParent = levelParent;
    }

    protected override void Awake()
    {
        base.Awake();
        _rigidbody = GetRequiredComponent<Rigidbody2D>();
    }

    public bool TryPick(Transform parent)
    {
        if (!enabled) return false;

        _rigidbody.simulated = false;
        transform.SetParent(parent, true);

        _timer.ScheduleVector2Interpolation(
            () => transform.position,
            p => transform.position = p,
            () => parent.transform.position,
            _transferTime).Then(() =>
        {
            _rigidbody.simulated = true;
            _rigidbody.bodyType = RigidbodyType2D.Kinematic;
        });

        return true;
    }

    public bool TryPlace(Vector2 deltaPosition)
    {
        if (!enabled) return false;

        var parent = transform.parent;
        transform.parent = _levelParent.transform;

        _timer.ScheduleVector2Interpolation(
            () => transform.position,
            p => transform.position = p,
            () => (Vector2)parent.position + deltaPosition,
            _transferTime).Then(() =>
        {
            _rigidbody.bodyType = RigidbodyType2D.Dynamic;
        });

        return true;
    }
}
