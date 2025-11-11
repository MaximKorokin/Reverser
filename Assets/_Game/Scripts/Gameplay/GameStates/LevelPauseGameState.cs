public class LevelPauseGameState : GameState
{
    public LevelPauseGameState(UIInputHandler uiInputHandler) : base(uiInputHandler)
    {
    }

    protected override void OnCancelInputRecieved()
    {
        base.OnCancelInputRecieved();

        SwitchState(typeof(LevelCleanupGameState), typeof(MainMenuGameState));
    }

    protected override void OnSubmitInputRecieved()
    {
        base.OnSubmitInputRecieved();

        SwitchState(typeof(LevelPlayGameState));
    }
}
