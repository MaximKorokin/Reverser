using UnityEngine;
using VContainer;

public class Disappearable : MonoBehaviourBase, IStateBindable
{
    private SpriteRenderer _spriteRenderer;
    private Collider2D _collider2D;
    private Timer _timer;

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
        _collider2D.enabled = state;
        _spriteRenderer.color = new(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, state ? 1 : 0.2f);
    }
}
