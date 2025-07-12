using System;
using System.Linq;

public class GameStatesController
{
    private readonly GameState[] _states;

    public GameStatesController(
        MainMenuGameState mainMenuGameState,
        LevelStartGameState levelStartGameState,
        LevelMainGameState levelMainGameState,
        LevelEndGameState levelEndGameState)
    {
        _states = new GameState[]
        {
            mainMenuGameState,
            levelStartGameState,
            levelMainGameState,
            levelEndGameState
        };

        _states.ForEach(x => x.SwitchStateRequested += OnSwitchStateRequested);
    }

    private void OnSwitchStateRequested(GameState fromState, Type toStateType)
    {
        var toState = _states.FirstOrDefault(x => x.GetType() == toStateType);
        if (!fromState.IsEnabled || toState.IsEnabled)
        {
            Logger.Warn($"Trying to switch {nameof(GameState)} of type {fromState.GetType()} to type {toStateType} but conditions are not met");
            return;
        }

        Logger.Log(fromState, toState);

        fromState.Disable();
        toState.Enable();
    }

    public void SetState(Type stateType)
    {
        var state = _states.FirstOrDefault(x => x.GetType() == stateType);

        _states.Except(state.Yield()).Where(x => x.IsEnabled).ForEach(x => x.Disable());

        state.Enable();
    }
}
