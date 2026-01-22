using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelPrefabsManager
{
    private readonly List<GameObject> _prefabs = new();

    public IEnumerable<GameObject> Prefabs => _prefabs;

    public LevelPrefabsManager(LevelPrefabs levelPrefabs)
    {
        _prefabs = levelPrefabs.ToList();
    }

    public string ToLevelPrefabName(GameObject prefab)
    {
        var obj = _prefabs.FirstOrDefault(x => x == prefab);
        if (obj == null)
        {
            Logger.Error($"{prefab} is not present in {nameof(LevelPrefabs)}");
            return "";
        }

        return obj.name;
    }

    public GameObject ToLevelPrefab(string name)
    {
        var prefab = _prefabs.FirstOrDefault(x => x.name == name);
        if (prefab == null)
        {
            Logger.Error($"{name} is not present in {nameof(LevelPrefabs)}");
        }
        return prefab;
    }
}
