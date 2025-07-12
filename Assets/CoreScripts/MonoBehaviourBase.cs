using System;
using UnityEngine;

public abstract class MonoBehaviourBase : MonoBehaviour
{
    public event Action OnDestroying;

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {

    }

    protected virtual void OnEnable()
    {

    }

    protected virtual void OnDisable()
    {

    }

    protected virtual void Update()
    {

    }

    protected virtual void FixedUpdate()
    {

    }

    protected virtual void OnDestroy()
    {
        OnDestroying?.Invoke();
    }

    protected virtual T GetRequiredComponent<T>() where T : Component
    {
        return RequireUtils.GetRequiredComponent<T>(gameObject);
    }
}
