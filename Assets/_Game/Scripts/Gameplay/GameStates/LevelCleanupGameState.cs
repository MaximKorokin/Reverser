using System;

/// <summary>
/// Takes a <see cref="GameState"/> as parameter and switches to it automatically
/// </summary>
public class LevelCleanupGameState : GameState
{
    private readonly GamePauseService _playPauseService;
    private readonly LevelConstructor _levelConstructor;

    public LevelCleanupGameState(
        GamePauseService playPauseService,
        LevelConstructor levelConstructor)
    {
        _playPauseService = playPauseService;
        _levelConstructor = levelConstructor;
    }

    protected override void EnableInternal(object parameter)
    {
        base.EnableInternal(parameter);

        _playPauseService.Disable();
        _levelConstructor.Clear();

        if (parameter is Type type && type.IsSubclassOf(typeof(GameState)))
        {
            SwitchState(type);
        }
    }
}
