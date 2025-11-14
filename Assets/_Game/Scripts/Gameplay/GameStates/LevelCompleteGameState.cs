public class LevelCompleteGameState : GameState
{
    private readonly Timer _timer;
    private readonly LevelOverlayService _levelOverlayService;

    public LevelCompleteGameState(
        LevelOverlayService levelOverlayService,
        Timer timer)
    {
        _levelOverlayService = levelOverlayService;
        _timer = timer;
    }

    protected override void EnableInternal(object parameter)
    {
        base.EnableInternal(parameter);

        _levelOverlayService.EnableService();
        _levelOverlayService.SetOverlay(LevelOverlayService.OverlayType.LevelComplete);

        _timer.Schedule(() => SwitchState(typeof(LevelCleanupGameState), typeof(MainMenuGameState)), 1);
    }

    protected override void DisableInternal()
    {
        base.DisableInternal();

        _levelOverlayService.DisableService();
    }
}