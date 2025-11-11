using System;
using System.Collections.Generic;
using System.Linq;

public class GameStatesController
{
    private readonly Func<Type, GameState> _gameStatesResolver;

    private readonly HashSet<GameState> _savedStates = new();

    public event Action<GameState> GameStateChanged;

    public GameStatesController(Func<Type, GameState> gameStatesResolver)
    {
        _gameStatesResolver = gameStatesResolver;
    }

    private void OnSwitchStateRequested(GameState fromState, Type toStateType, object parameter)
    {
        var toState = _gameStatesResolver(toStateType);
        if (toState == null || !fromState.IsEnabled || toState.IsEnabled)
        {
            Logger.Warn($"Trying to switch {nameof(GameState)} of type {fromState.GetType()} to type {toStateType} but conditions are not met");
            return;
        }

        fromState.Disable();
        SetStateInternal(toState, parameter);
    }

    public void SetState(Type stateType, object parameter = null)
    {
        var state = _gameStatesResolver(stateType);
        if (state == null)
        {
            Logger.Warn($"Trying to set {nameof(GameState)} of type {stateType} but it is null");
            return;
        }

        _savedStates.Except(state.Yield()).Where(x => x.IsEnabled).ForEach(x => x.Disable());
        SetStateInternal(state, parameter);
    }

    private void SetStateInternal(GameState gameState, object parameter)
    {
        if (_savedStates.Add(gameState))
        {
            gameState.SwitchStateRequested += OnSwitchStateRequested;
        }

        gameState.Enable(parameter);
        GameStateChanged?.Invoke(gameState);
    }
}
