using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class LevelPrefabsManager
{
    private readonly Dictionary<GameObject, LevelPrefabInfoAttribute> _prefabs = new();

    public IEnumerable<GameObject> Prefabs => _prefabs.Keys;

    public LevelPrefabsManager(LevelPrefabs levelPrefabs)
    {
        foreach (var (info, value) in levelPrefabs
            .GetType()
            .GetAllProperties(flags: BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
            .Select(x => (info: x.GetCustomAttribute(typeof(LevelPrefabInfoAttribute)), value: x.GetValue(levelPrefabs)))
            .Where(x => x.value is Component or GameObject))
        {
            GameObject gameObject;
            if (value is GameObject obj) gameObject = obj;
            else if (value is Component component) gameObject = component.gameObject;
            else gameObject = null;

            _prefabs.Add(gameObject, info as LevelPrefabInfoAttribute);
        }
    }

    public bool IsValidLevel(IEnumerable<GameObject> gameObjects)
    {
        foreach (var gameObject in gameObjects)
        {
            if (!_prefabs.TryGetValue(gameObject, out var info))
            {
                Logger.Error($"{gameObject} is not present in {nameof(LevelPrefabs)}");
                continue;
            }
        }
        return true;
    }

    public LevelData ToLevelData(IEnumerable<(GameObject, Vector2)> prefabPositionPairs)
    {
        var levelData = new LevelData
        {
            LevelObjects = new()
        };
        foreach (var (prefab, position) in prefabPositionPairs)
        {
            if (!_prefabs.TryGetValue(prefab, out var info))
            {
                Logger.Error($"{prefab} is not present in {nameof(LevelPrefabs)}");
                continue;
            }

            // avoiding excessive float error (like 3.000000238418579f)
            var roundedPosition = new Vector2((float)Math.Round(position.x, 2), (float)Math.Round(position.y, 2));
            levelData.LevelObjects.Add(new() { Name = info.Name, Position = roundedPosition });
        }
        return levelData;
    }

    public GameObject ToLevelPrefab(string name)
    {
        var prefab = _prefabs.FirstOrDefault(x => x.Value.Name == name);
        if (prefab.Key == null)
        {
            Logger.Error($"{name} is not present in {nameof(LevelPrefabs)}");
        }
        return prefab.Key;
    }
}
