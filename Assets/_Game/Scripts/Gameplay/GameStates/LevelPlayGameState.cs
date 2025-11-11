public class LevelPlayGameState : GameState
{
    private readonly PlayPauseService _playPauseController;
    private readonly LevelSharedContext _levelSharedContext;

    public LevelPlayGameState(
        UIInputHandler uiInputHandler,
        PlayPauseService playPauseController,
        LevelSharedContext levelSharedContext) : base(uiInputHandler)
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

    protected override void OnCancelInputRecieved()
    {
        base.OnCancelInputRecieved();

        SwitchState(typeof(LevelPauseGameState));
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
