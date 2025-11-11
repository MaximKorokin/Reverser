public class LevelFailGameState : GameState
{
    private readonly Timer _timer;

    public LevelFailGameState(
        UIInputHandler uiInputHandler,
        Timer timer) : base(uiInputHandler)
    {
        _timer = timer;
    }

    protected override void EnableInternal(object parameter)
    {
        base.EnableInternal(parameter);

        _timer.Schedule(() => SwitchState(typeof(LevelStartGameState)), 1);

        Logger.Log("=== FAIL ===");
    }

    protected override void DisableInternal()
    {
        base.DisableInternal();
    }
}