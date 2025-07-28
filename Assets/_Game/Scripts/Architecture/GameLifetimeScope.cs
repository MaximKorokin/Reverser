using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    [SerializeField]
    private LevelPrefabs _levelPrefabs;
    [SerializeField]
    private LevelDataProvider _levelDataProvider;
    [SerializeField]
    private TimeControlSettings _timeControlSettings;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<GameController>();
        builder.RegisterComponentOnNewGameObject<Timer>(Lifetime.Scoped);
        builder.RegisterComponentInHierarchy<CameraController>();

        RegisterUI(builder);
        RegisterLevelConstructing(builder);
        RegisterLevelEditor(builder);
        RegisterGameStates(builder);
        RegisterInput(builder);
        RegisterTimeControlSystem(builder);

        builder.RegisterInstance(_levelPrefabs);
        builder.Register<LevelPrefabsManager>(Lifetime.Scoped);
    }

    private void RegisterLevelConstructing(IContainerBuilder builder)
    {
        var levelParent = new GameObject { name = "LevelObjectsParent" };
        builder.RegisterInstance(levelParent.transform);

        builder.Register<LevelConstructor>(Lifetime.Scoped);
        builder.Register<LevelSharedContext>(Lifetime.Scoped);
        builder.RegisterInstance(_levelDataProvider);

        builder.RegisterFactory<GameObject, Vector2, GameObject>(resolver => (c, v) => resolver.Instantiate(c, v, Quaternion.identity), Lifetime.Singleton);
    }

    private void RegisterLevelEditor(IContainerBuilder builder)
    {
        builder.RegisterComponentInHierarchy<LevelEditorOpenButton>();
        builder.RegisterComponentInHierarchy<LevelEditorController>();
    }

    private void RegisterUI(IContainerBuilder builder)
    {
        builder.RegisterComponentInHierarchy<LevelSelectionController>();

        builder.RegisterFactory<int, LevelData>(resolver => i => _levelDataProvider.GetLevelData(i), Lifetime.Transient);
    }

    private void RegisterInput(IContainerBuilder builder)
    {
        builder.Register<InputSystemActions>(Lifetime.Singleton);

        builder.Register<PlayerInputHandler>(Lifetime.Transient);
        builder.Register<UIInputHandler>(Lifetime.Transient);
    }

    private void RegisterTimeControlSystem(IContainerBuilder builder)
    {
        builder.RegisterInstance(_timeControlSettings);
        builder.Register<TimeControlMediator>(Lifetime.Scoped);
        builder.Register<ComponentStateProcessorFactory>(Lifetime.Scoped);
    }

    private void RegisterGameStates(IContainerBuilder builder)
    {
        builder.Register<GameStatesController>(Lifetime.Scoped);

        builder.RegisterFactory<Type, GameState>(resolver => t => resolver.Resolve(t) as GameState, Lifetime.Scoped);

        builder.Register<MainMenuGameState>(Lifetime.Scoped);

        builder.Register<LevelStartGameState>(Lifetime.Scoped);
        builder.Register<LevelPlayGameState>(Lifetime.Scoped);
        builder.Register<LevelPauseGameState>(Lifetime.Scoped);
        builder.Register<LevelCompleteGameState>(Lifetime.Scoped);
        builder.Register<LevelFailGameState>(Lifetime.Scoped);

        builder.Register<LevelEditorGameState>(Lifetime.Scoped);
    }
}
