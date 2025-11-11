using System;
using System.Linq;
using System.Runtime.ConstrainedExecution;
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

        foreach(var levelObject in levelData.LevelObjects)
        {
            var prefab = _prefabsManager.ToLevelPrefab(levelObject.Name);
            _instantiationFactory(prefab, levelObject.Position).transform.SetParent(_levelParent, true);
        }
    }
}
