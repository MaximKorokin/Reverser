public class LevelEndGameState : GameState
{
    private readonly Timer _timer;
    private readonly LevelConstructor _levelConstructor;

    public LevelEndGameState(
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

        _timer.Schedule(() => SwitchState(typeof(MainMenuGameState)), 1);

        Logger.Log("=== END ===");
    }

    protected override void DisableInternal()
    {
        base.DisableInternal();

        _levelConstructor.Clear();
    }

    protected override void OnCancelInputRecieved()
    {

    }

    protected override void OnSubmitInputRecieved()
    {

    }
}