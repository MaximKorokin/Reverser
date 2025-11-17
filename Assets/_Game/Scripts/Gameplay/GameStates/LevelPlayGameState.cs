public class LevelPlayGameState : GameState
{
    private readonly LevelSharedContext _levelSharedContext;

    public LevelPlayGameState(
        PlayPauseService playPauseController,
        LevelSharedContext levelSharedContext)
    {
        _levelSharedContext = levelSharedContext;

        this.KeepSynchronized(
            playPauseController,
            () => playPauseController.Resume());
    }

    protected override void EnableInternal(object parameter)
    {
        base.EnableInternal(parameter);

        _levelSharedContext.LevelCompleted += OnLevelCompleted;
        _levelSharedContext.LevelFailed += OnLevelFailed;
    }

    protected override void DisableInternal()
    {
        base.DisableInternal();

        _levelSharedContext.LevelCompleted -= OnLevelCompleted;
        _levelSharedContext.LevelFailed -= OnLevelFailed;
    }

    private void OnLevelCompleted()
    {
        SwitchState(typeof(LevelCompleteGameState));
    }

    private void OnLevelFailed()
    {
        SwitchState(typeof(LevelFailGameState));
    }
}
