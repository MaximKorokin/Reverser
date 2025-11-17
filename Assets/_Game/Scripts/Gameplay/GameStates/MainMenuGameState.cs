public class MainMenuGameState : GameState
{
    private readonly LevelSharedContext _levelSharedContext;

    public MainMenuGameState(
        LevelSelectionService levelSelectionController,
        LevelSharedContext levelSharedContext,
        ExitGameService exitGameService)
    {
        this.KeepSynchronized(
            levelSelectionController,
            () => levelSelectionController.LevelSelected += OnLevelSelected,
            () => levelSelectionController.LevelSelected -= OnLevelSelected);
        this.KeepSynchronized(exitGameService);

        _levelSharedContext = levelSharedContext;
    }

    private void OnLevelSelected(LevelData data)
    {
        _levelSharedContext.LevelData = data;
        SwitchState(typeof(LevelStartGameState));
    }
}
