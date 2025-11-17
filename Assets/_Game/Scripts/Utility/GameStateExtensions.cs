using System;

public static class GameStateExtensions
{
    public static void KeepSynchronized(this GameState gameState, ServiceBase service, Action onEnabled = null, Action onDisabled = null)
    {
        gameState.Enabled += service.Enable;
        gameState.Disabled += service.Disable;

        if (onEnabled != null) gameState.Enabled += onEnabled;
        if (onDisabled != null) gameState.Disabled += onDisabled;
    }
}
