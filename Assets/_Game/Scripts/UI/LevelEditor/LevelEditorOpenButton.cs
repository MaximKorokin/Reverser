using UnityEngine.UI;
using VContainer;

public class LevelEditorOpenButton : MonoBehaviourBase
{
    private GameStatesController _gameStatesController;
    private LevelConstructor _levelConstructor;

    [Inject]
    private void Construct(
        GameStatesController gameStatesController,
        LevelConstructor levelConstructor)
    {
        _gameStatesController = gameStatesController;
        _levelConstructor = levelConstructor;

        _gameStatesController.GameStateChanged += OnGameStateChanged;
        OnDestroying += () => _gameStatesController.GameStateChanged -= OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState state)
    {
        gameObject.SetActive(state.GetType() != typeof(LevelEditorGameState));
    }

    private void OpenLevelEditor()
    {
        _levelConstructor.Clear();
        _gameStatesController.SetState(typeof(LevelEditorGameState));
    }

    protected override void Awake()
    {
        base.Awake();

        GetRequiredComponent<Button>().onClick.AddListener(() => OpenLevelEditor());
    }
}
