public class LevelCompleteGameState : GameState
{
    private readonly Timer _timer;

    public LevelCompleteGameState(
        UIInputHandler uiInputHandler,
        Timer timer) : base(uiInputHandler)
    {
        _timer = timer;
    }

    protected override void EnableInternal(object parameter)
    {
        base.EnableInternal(parameter);

        _timer.Schedule(() => SwitchState(typeof(LevelCleanupGameState), typeof(MainMenuGameState)), 1);

        Logger.Log("=== COMPLETE ===");
    }
}