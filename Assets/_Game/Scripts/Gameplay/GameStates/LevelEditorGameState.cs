public class LevelEditorGameState : GameState
{
    private readonly LevelEditorController _levelEditorController;

    public LevelEditorGameState(LevelEditorController levelEditorController) 
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

    protected override void EnableInternal(object parameter)
    {
        base.EnableInternal(parameter);

        _levelEditorController.gameObject.SetActive(true);
    }
}
