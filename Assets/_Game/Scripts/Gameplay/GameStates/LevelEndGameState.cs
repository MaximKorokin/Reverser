public class LevelEndGameState : GameState
{
    public LevelEndGameState(UIInputHandler uiInputHandler) : base(uiInputHandler)
    {

    }

    protected override void EnableInternal()
    {
        base.EnableInternal();

        Logger.Log("=== END ===");
    }

    protected override void OnCancelInputRecieved()
    {

    }

    protected override void OnSubmitInputRecieved()
    {

    }
}