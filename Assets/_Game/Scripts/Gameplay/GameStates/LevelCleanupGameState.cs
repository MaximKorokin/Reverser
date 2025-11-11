using System;

/// <summary>
/// Takes a <see cref="GameState"/> as parameter and switches to it automatically
/// </summary>
public class LevelCleanupGameState : GameState
{
    private readonly PlayPauseService _playPauseService;
    private readonly LevelConstructor _levelConstructor;

    public LevelCleanupGameState(
        UIInputHandler uiInputHandler,
        PlayPauseService playPauseService,
        LevelConstructor levelConstructor) : base(uiInputHandler)
    {
        _playPauseService = playPauseService;
        _levelConstructor = levelConstructor;
    }

    protected override void EnableInternal(object parameter)
    {
        base.EnableInternal(parameter);

        _playPauseService.DisableService();
        _levelConstructor.Clear();

        if (parameter is Type type && type.IsSubclassOf(typeof(GameState)))
        {
            SwitchState(type);
        }
    }
}
