public class LevelPlayGameState : GameState
{
    private readonly PlayPauseService _playPauseController;
    private readonly LevelSharedContext _levelSharedContext;

    public LevelPlayGameState(
        PlayPauseService playPauseController,
        LevelSharedContext levelSharedContext)
    {
        _playPauseController = playPauseController;
        _levelSharedContext = levelSharedContext;
    }

    protected override void EnableInternal(object parameter)
    {
        base.EnableInternal(parameter);

        _playPauseController.EnableService();
        _playPauseController.Resume();
        
        _levelSharedContext.LevelCompleted += OnLevelCompleted;
        _levelSharedContext.LevelFailed += OnLevelFailed;
    }

    protected override void DisableInternal()
    {
        base.DisableInternal();

        _playPauseController.Pause();

        _levelSharedContext.LevelCompleted -= OnLevelCompleted;
        _levelSharedContext.LevelFailed -= OnLevelFailed;
    }

    private void OnLevelCompleted()
    {
        SwitchState(typeof(LevelCompleteGameState));
        _playPauseController.DisableService();
    }

    private void OnLevelFailed()
    {
        SwitchState(typeof(LevelFailGameState));
        _playPauseController.DisableService();
    }
}
