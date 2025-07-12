using UnityEngine;
using VContainer.Unity;

public class GameController : ITickable, IInitializable
{
    private readonly GameStatesController _gameStatesController;

    public GameController(GameStatesController gameStatesController)
    {
        _gameStatesController = gameStatesController;
    }

    public void Initialize()
    {
        Application.targetFrameRate = 60;

        _gameStatesController.SetState(typeof(MainMenuGameState));
    }

    public void Tick()
    {

    }
}
