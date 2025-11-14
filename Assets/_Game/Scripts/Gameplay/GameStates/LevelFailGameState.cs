public class LevelFailGameState : GameState
{
    private readonly LevelOverlayService _levelOverlayService;
    private readonly Timer _timer;

    public LevelFailGameState(
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
        _levelOverlayService.SetOverlay(LevelOverlayService.OverlayType.LevelFail);

        _timer.Schedule(() => SwitchState(typeof(LevelStartGameState)), 1);
    }

    protected override void DisableInternal()
    {
        base.DisableInternal();

        _levelOverlayService.DisableService();
    }
}