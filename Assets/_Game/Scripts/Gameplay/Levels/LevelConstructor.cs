using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelConstructor
{
    private readonly LevelSharedContext _levelSharedContext;
    private readonly Transform _levelParent;
    private readonly Func<GameObject, Vector2, GameObject> _instantiationFactory;
    private readonly LevelPrefabsManager _prefabsManager;

    public LevelConstructor(
        LevelSharedContext levelSharedContext,
        Transform levelParent,
        Func<GameObject, Vector2, GameObject> instantiationFactory,
        LevelPrefabsManager prefabsManager)
    {
        _levelSharedContext = levelSharedContext;
        _levelParent = levelParent;
        _instantiationFactory = instantiationFactory;
        _prefabsManager = prefabsManager;
    }

    public void Clear()
    {
        _levelParent.Cast<Transform>().ForEach(x => UnityEngine.Object.Destroy(x.gameObject));
    }

    public void ConstructLevel()
    {
        // todo: maybe these lines should be in LevelStartGameState
        _levelSharedContext.LevelTimeCounter.Reset(_levelSharedContext.LevelData.LevelHalfDuration * 2);
        _levelSharedContext.LevelTimeCounter.SetPaused(true);

        ConstructLevel(_levelSharedContext.LevelData);
    }

    private void ConstructLevel(LevelData levelData)
    {
        Clear();

        _levelSharedContext.LevelData = levelData;

        if (levelData == null) return;

        var objectsMap = new Dictionary<string, (GameObject GameObject, LevelObject LevelObject)>();
        foreach (var levelObject in levelData.LevelObjects)
        {
            var prefab = _prefabsManager.ToLevelPrefab(levelObject.Name);
            var newObject = _instantiationFactory(prefab, levelObject.Position);
            newObject.transform.SetParent(_levelParent, true);
            objectsMap.Add(levelObject.Id ?? Guid.NewGuid().ToString(), (newObject, levelObject));
        }

        foreach (var (gameObject, levelObject) in objectsMap.Values)
        {
            var bindable = gameObject.GetComponent<IStateBindable>();

            if (levelObject.Bindings != null && levelObject.Bindings.Count > 0)
            {
                if (bindable != null)
                {
                    foreach (var toBindId in levelObject.Bindings)
                    {
                        var toBind = objectsMap[toBindId];
                        if (toBind.GameObject.TryGetComponent<IStateful>(out var stateful))
                        {
                            bindable.Bind(stateful);
                        }
                        else
                        {
                            Logger.Error($"Object {toBind.LevelObject.Name} with id {toBind.LevelObject.Id} cannot be bound because it does not implement {nameof(IStateful)}");
                        }
                    }
                }
                else
                {
                    Logger.Error($"Object {levelObject.Name} with id {levelObject.Id} has bindings but does not contain {nameof(IStateBindable)}");
                }
            }

            if (bindable != null)
            {
                bindable.SetBindInterpretation(levelObject.ToggleBind);
            }
        }
    }
}
