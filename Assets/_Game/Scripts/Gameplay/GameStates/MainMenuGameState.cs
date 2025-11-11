using System;
using UnityEngine;

public class MainMenuGameState : GameState, IDisposable
{
    private readonly LevelSelectionController _levelSelectionController;
    private readonly LevelSharedContext _levelSharedContext;

    public MainMenuGameState(
        UIInputHandler uiInputHandler,
        LevelSelectionController levelSelectionController,
        LevelSharedContext levelSharedContext) : base(uiInputHandler)
    {
        _levelSelectionController = levelSelectionController;
        _levelSharedContext = levelSharedContext;

        _levelSelectionController.GenerateButtons();
        _levelSelectionController.gameObject.SetActive(false);
    }

    private void OnLevelSelected(LevelData data)
    {
        _levelSharedContext.LevelData = data;
        SwitchState(typeof(LevelStartGameState));
    }

    protected override void EnableInternal(object parameter)
    {
        base.EnableInternal(parameter);

        _levelSelectionController.gameObject.SetActive(true);
        _levelSelectionController.LevelSelected -= OnLevelSelected;
        _levelSelectionController.LevelSelected += OnLevelSelected;
    }

    protected override void DisableInternal()
    {
        base.DisableInternal();

        _levelSelectionController.gameObject.SetActive(false);
        _levelSelectionController.LevelSelected -= OnLevelSelected;
    }

    protected override void OnCancelInputRecieved()
    {
        base.OnCancelInputRecieved();
        Application.Quit();
    }

    public void Dispose()
    {
        _levelSelectionController.LevelSelected -= OnLevelSelected;
    }
}
