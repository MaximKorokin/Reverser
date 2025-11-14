public class MainMenuGameState : GameState
{
    private readonly LevelSelectionService _levelSelectionService;
    private readonly LevelSharedContext _levelSharedContext;
    private readonly ExitGameService _exitGameService;

    public MainMenuGameState(
        LevelSelectionService levelSelectionController,
        LevelSharedContext levelSharedContext,
        ExitGameService exitGameService)
    {
        _levelSelectionService = levelSelectionController;
        _levelSharedContext = levelSharedContext;
        _exitGameService = exitGameService;
    }

    private void OnLevelSelected(LevelData data)
    {
        _levelSharedContext.LevelData = data;
        SwitchState(typeof(LevelStartGameState));
    }

    protected override void EnableInternal(object parameter)
    {
        base.EnableInternal(parameter);

        _levelSelectionService.EnableService();
        _levelSelectionService.LevelSelected -= OnLevelSelected;
        _levelSelectionService.LevelSelected += OnLevelSelected;

        _exitGameService.EnableService();
    }

    protected override void DisableInternal()
    {
        base.DisableInternal();

        _levelSelectionService.DisableService();
        _levelSelectionService.LevelSelected -= OnLevelSelected;

        _exitGameService.DisableService();
    }
}
