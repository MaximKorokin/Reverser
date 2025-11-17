public class LevelFailGameState : GameState
{
    private readonly Timer _timer;

    public LevelFailGameState(
        LevelOverlayService levelOverlayService,
        Timer timer)
    {
        this.KeepSynchronized(
            levelOverlayService,
            () => levelOverlayService.SetOverlay(LevelOverlayService.OverlayType.LevelFail));
        _timer = timer;
    }

    protected override void EnableInternal(object parameter)
    {
        base.EnableInternal(parameter);

        _timer.Schedule(() => SwitchState(typeof(LevelStartGameState)), 1);
    }
}