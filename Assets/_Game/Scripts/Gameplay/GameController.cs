using UnityEngine;
using VContainer.Unity;

public class GameController : ITickable, IInitializable
{
    private readonly MainMenuGameState _mainMenuGameState;

    public GameController(MainMenuGameState mainMenuGameState)
    {
        _mainMenuGameState = mainMenuGameState;
    }

    public void Initialize()
    {
        Application.targetFrameRate = 60;
        _mainMenuGameState.Enable();
    }

    public void Tick()
    {

    }
}
