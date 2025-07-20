using UnityEngine;

public abstract class UIBehaviourBase : MonoBehaviourBase
{
    public RectTransform RectTransform => GetLazy(() => transform as RectTransform);

    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
}
