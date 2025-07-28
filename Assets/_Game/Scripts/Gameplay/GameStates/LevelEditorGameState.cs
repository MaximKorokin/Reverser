public class LevelEditorGameState : GameState
{
    private readonly LevelEditorController _levelEditorController;

    public LevelEditorGameState(UIInputHandler uiInputHandler, LevelEditorController levelEditorController) : base(uiInputHandler)
    {
        _levelEditorController = levelEditorController;

        _levelEditorController.CloseEditorRequested += () => SwitchState(typeof(MainMenuGameState));
        _levelEditorController.LoadLevelRequested += () => SwitchState(typeof(LevelStartGameState));
    }

    protected override void DisableInternal()
    {
        base.DisableInternal();

        _levelEditorController.gameObject.SetActive(false);
    }

    protected override void EnableInternal()
    {
        base.EnableInternal();

        _levelEditorController.gameObject.SetActive(true);
    }
}
