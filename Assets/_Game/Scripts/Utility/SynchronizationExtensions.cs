using System;
using UnityEngine;

public static class SynchronizationExtensions
{
    public static void KeepSynchronized(this GameState gameState, ServiceBase service, Action onEnabled = null, Action onDisabled = null)
    {
        gameState.Enabled += service.Enable;
        gameState.Disabled += service.Disable;

        if (onEnabled != null) gameState.Enabled += onEnabled;
        if (onDisabled != null) gameState.Disabled += onDisabled;
    }

    public static void KeepSynchronized(this ViewBase view, GameObject gameObject, Action onEnabled = null, Action onDisabled = null)
    {
        view.Enabled += () => gameObject.SetActive(true);
        view.Disabled += () => gameObject.SetActive(false);

        if (onEnabled != null) view.Enabled += onEnabled;
        if (onDisabled != null) view.Disabled += onDisabled;
    }
}
