public class LevelStartGameState : GameState
{
    private readonly TimeControlMediator _timeControlMediator;
    private readonly LevelConstructor _levelConstructor;

    public LevelStartGameState(
        LevelConstructor levelConstructor,
        TimeControlMediator timeControlMediator,
        LevelOverlayService levelOverlayService,
        WaitForInputService waitForInputService)
    {
        _levelConstructor = levelConstructor;
        _timeControlMediator = timeControlMediator;

        this.KeepSynchronized(
            levelOverlayService,
            () => levelOverlayService.SetOverlay(LevelOverlayService.OverlayType.LevelStart));
        this.KeepSynchronized(
            waitForInputService,
            () => waitForInputService.InputReceived += OnInputReceived,
            () => waitForInputService.InputReceived -= OnInputReceived);
    }

    protected override void EnableInternal(object parameter)
    {
        base.EnableInternal(parameter);
        _timeControlMediator.SetTimeFlowMode(TimeFlowMode.Paused);
        _levelConstructor.ConstructLevel();
    }

    private void OnInputReceived()
    {
        SwitchState(typeof(LevelPlayGameState));
    }
}
