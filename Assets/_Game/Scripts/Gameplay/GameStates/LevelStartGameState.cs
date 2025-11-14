public class LevelStartGameState : GameState
{
    private readonly TimeControlMediator _timeControlMediator;
    private readonly LevelConstructor _levelConstructor;
    private readonly LevelOverlayService _levelOverlayService;
    private readonly Timer _timer;

    public LevelStartGameState(
        LevelConstructor levelConstructor,
        TimeControlMediator timeControlMediator,
        LevelOverlayService levelOverlayService,
        Timer timer)
    {
        _levelConstructor = levelConstructor;
        _timeControlMediator = timeControlMediator;
        _levelOverlayService = levelOverlayService;
        _timer = timer;
    }

    protected override void EnableInternal(object parameter)
    {
        base.EnableInternal(parameter);
        _timeControlMediator.SetTimeFlowMode(TimeFlowMode.Paused);
        _levelConstructor.ConstructLevel();

        _levelOverlayService.EnableService();
        _levelOverlayService.SetOverlay(LevelOverlayService.OverlayType.LevelStart);

        _timer.Schedule(() => SwitchState(typeof(LevelPlayGameState)), 1);
    }

    protected override void DisableInternal()
    {
        base.DisableInternal();

        _levelOverlayService.DisableService();
    }
}
