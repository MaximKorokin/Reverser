using System;
using UnityEngine;

public abstract class UIBehaviourBase : MonoBehaviourBase
{
    public Canvas Canvas => GetLazy(() => GetRequiredComponentInParent<Canvas>());
    public RectTransform RectTransform => GetLazy(() => transform as RectTransform);

    public event Action OnShown;
    public event Action OnHidden;

    public virtual void Show()
    {
        gameObject.SetActive(true);
        OnShown?.Invoke();
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
        OnHidden?.Invoke();
    }
}
