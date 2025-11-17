public class LevelCompleteGameState : GameState
{
    private readonly Timer _timer;

    public LevelCompleteGameState(
        LevelOverlayService levelOverlayService,
        Timer timer)
    {
        this.KeepSynchronized(
            levelOverlayService,
            () => levelOverlayService.SetOverlay(LevelOverlayService.OverlayType.LevelStart));
        _timer = timer;
    }

    protected override void EnableInternal(object parameter)
    {
        base.EnableInternal(parameter);

        _timer.Schedule(() => SwitchState(typeof(LevelCleanupGameState), typeof(MainMenuGameState)), 1);
    }
}