using System;
using UnityEngine;

public class MainMenuGameState : GameState, IDisposable
{
    private readonly LevelSelectionController _levelSelectionController;
    private readonly LevelConstructor _levelConstructor;
    private readonly LevelSharedContext _levelSharedContext;

    private readonly LevelStartGameState _levelStartGameState;

    public MainMenuGameState(
        UIInputHandler uiInputHandler,
        LevelConstructor levelConstructor,
        LevelSharedContext levelSharedContext,
        LevelSelectionController levelSelectionController,
        LevelStartGameState levelStartGameState) : base(uiInputHandler)
    {
        _levelSelectionController = levelSelectionController;
        _levelConstructor = levelConstructor;
        _levelSharedContext = levelSharedContext;
        _levelStartGameState = levelStartGameState;

        _levelSelectionController.GenerateButtons();
        _levelSelectionController.gameObject.SetActive(false);
    }

    private void OnLevelSelected(LevelData data)
    {
        _levelConstructor.Costruct(data);
        _levelSharedContext.LevelData = data;
        SwitchState(_levelStartGameState);
    }

    protected override void EnableInternal()
    {
        base.EnableInternal();

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
        Application.Quit();
    }

    protected override void OnSubmitInputRecieved()
    {
    }

    public void Dispose()
    {
        _levelSelectionController.LevelSelected -= OnLevelSelected;
    }
}
