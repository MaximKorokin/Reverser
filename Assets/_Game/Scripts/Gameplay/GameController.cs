using System.Globalization;
using UnityEngine;
using VContainer.Unity;

public class GameController : IStartable
{
    private readonly GameStatesController _gameStatesController;

    public GameController(GameStatesController gameStatesController)
    {
        _gameStatesController = gameStatesController;
    }

    public void Start()
    {
        CultureInfo.CurrentCulture = new CultureInfo("en-US");
        Application.targetFrameRate = 60;

        _gameStatesController.SetState(typeof(MainMenuGameState));
    }
}
