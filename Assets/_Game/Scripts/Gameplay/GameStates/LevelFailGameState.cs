public class LevelFailGameState : GameState
{
    private readonly Timer _timer;
    private readonly LevelConstructor _levelConstructor;

    public LevelFailGameState(
        UIInputHandler uiInputHandler,
        Timer timer,
        LevelConstructor levelConstructor) : base(uiInputHandler)
    {
        _timer = timer;
        _levelConstructor = levelConstructor;
    }

    protected override void EnableInternal()
    {
        base.EnableInternal();

        _timer.Schedule(() => SwitchState(typeof(LevelStartGameState)), 1);

        Logger.Log("=== FAIL ===");
    }

    protected override void DisableInternal()
    {
        base.DisableInternal();

        _levelConstructor.Clear();
    }
}