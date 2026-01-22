using UnityEngine;
using VContainer;

public class Disappearable : MonoBehaviourBase, IStateBindable
{
    private SpriteRenderer _spriteRenderer;
    private Collider2D _collider2D;
    private Timer _timer;

    private bool _inverseState;
    private bool _lastState;

    [Inject]
    private void Construct(Timer timer)
    {
        _timer = timer;
    }

    protected override void Awake()
    {
        base.Awake();

        _spriteRenderer = GetRequiredComponentInChildren<SpriteRenderer>();
        _collider2D = GetRequiredComponent<Collider2D>();
    }

    public void OnStateChanged(bool state)
    {
        if (!this || !gameObject) return;

        _lastState = state;
        if (_inverseState) state = !state;

        _collider2D.isTrigger = !state;
        _spriteRenderer.color = new(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, state ? 1 : 0.2f);
    }

    public void SetBindInterpretation(bool inversed)
    {
        _inverseState = inversed;
        OnStateChanged(_lastState);
    }
}
